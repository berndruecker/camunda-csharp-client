using CamundaClient.Service;
using CamundaClient.Worker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamundaClient
{

    public class CamundaEngineClient
    {
        public static string DEFAULT_URL = "http://localhost:8080/engine-rest/engine/default/";
        public static string COCKPIT_URL = "http://localhost:8080/camunda/app/cockpit/default/";

        private IList<ExternalTaskWorker> _workers = new List<ExternalTaskWorker>();
        private CamundaClientHelper _camundaClientHelper;

        public CamundaEngineClient() : this(new Uri(DEFAULT_URL), null, null) { }

        public CamundaEngineClient(Uri restUrl, string userName, string password)
        {
            _camundaClientHelper = new CamundaClientHelper(restUrl, userName, password);
        }

        public BpmnWorkflowService BpmnWorkflowService => new BpmnWorkflowService(_camundaClientHelper);

        public HumanTaskService HumanTaskService => new HumanTaskService(_camundaClientHelper);

        public RepositoryService RepositoryService => new RepositoryService(_camundaClientHelper);

        public ExternalTaskService ExternalTaskService => new ExternalTaskService(_camundaClientHelper);

        public void Startup()
        {
            this.StartWorkers();
            this.RepositoryService.AutoDeploy();
        }

        public void Shutdown()
        {
            this.StopWorkers();
        }

        public void StartWorkers()
        {
            var assembly = System.Reflection.Assembly.GetEntryAssembly();
            var externalTaskWorkers = RetrieveExternalTaskWorkerInfo(assembly);

            foreach (var taskWorkerInfo in externalTaskWorkers)
            {
                Console.WriteLine($"Register Task Worker for Topic '{taskWorkerInfo.TopicName}'");
                ExternalTaskWorker worker = new ExternalTaskWorker(ExternalTaskService, taskWorkerInfo);
                _workers.Add(worker);
                worker.StartWork();
            }
        }

        private static IEnumerable<Dto.ExternalTaskWorkerInfo> RetrieveExternalTaskWorkerInfo(System.Reflection.Assembly assembly)
        {
            // find all classes with CustomAttribute [ExternalTask("name")]
            var externalTaskWorkers =
                from t in assembly.GetTypes()
                let externalTaskTopicAttribute = t.GetCustomAttributes(typeof(ExternalTaskTopicAttribute), true).FirstOrDefault() as ExternalTaskTopicAttribute
                let externalTaskVariableRequirements = t.GetCustomAttributes(typeof(ExternalTaskVariableRequirementsAttribute), true).FirstOrDefault() as ExternalTaskVariableRequirementsAttribute
                where externalTaskTopicAttribute != null
                select new Dto.ExternalTaskWorkerInfo
                {
                    Type = t,
                    TopicName = externalTaskTopicAttribute.TopicName,
                    Retries = externalTaskTopicAttribute.Retries,
                    RetryTimeout = externalTaskTopicAttribute.RetryTimeout,
                    VariablesToFetch = externalTaskVariableRequirements?.VariablesToFetch,
                    TaskAdapter = t.GetConstructor(Type.EmptyTypes)?.Invoke(null) as IExternalTaskAdapter
                };
            return externalTaskWorkers;
        }

        public void StopWorkers()
        {
            foreach (ExternalTaskWorker worker in _workers)
            {
                worker.StopWork();
            }
        }

        // HELPER METHODS

    }
}