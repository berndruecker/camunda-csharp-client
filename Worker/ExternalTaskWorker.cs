using CamundaClient.Dto;
using CamundaClient.Service;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CamundaClient.Worker
{
    public class ExternalTaskWorker : IDisposable
    {
        private string workerId = Guid.NewGuid().ToString(); // TODO: Make configurable

        private Timer taskQueryTimer;
        private long pollingIntervalInMilliseconds = 50; // every 50 milliseconds
        private int maxDegreeOfParallelism = 2;
        private int maxTasksToFetchAtOnce = 10;
        private long lockDurationInMilliseconds = 1 * 60 * 1000; // 1 minute
        private ExternalTaskService externalTaskService;
        private ExternalTaskWorkerInfo taskWorkerInfo;

        public ExternalTaskWorker(ExternalTaskService externalTaskService, ExternalTaskWorkerInfo taskWorkerInfo)
        {
            this.externalTaskService = externalTaskService;
            this.taskWorkerInfo = taskWorkerInfo;
        }

        public void DoPolling()
        {
            // Query External Tasks
            try {
                var tasks = externalTaskService.FetchAndLockTasks(workerId, maxTasksToFetchAtOnce, taskWorkerInfo.TopicName, lockDurationInMilliseconds, taskWorkerInfo.VariablesToFetch);

                // run them in parallel with a max degree of parallelism
                Parallel.ForEach(
                    tasks,
                    new ParallelOptions { MaxDegreeOfParallelism = this.maxDegreeOfParallelism },
                    externalTask => Execute(externalTask)
                );
            }
            catch (EngineException ex)
            {
                // Most probably server is not running or request is invalid
                Console.WriteLine(ex.Message);
            }

            // schedule next run (if not stopped in between)
            if (taskQueryTimer!=null) {
                taskQueryTimer.Change(TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(Timeout.Infinite));
            }
        }

        private void Execute(ExternalTask externalTask)
        {
            Dictionary<string, object> resultVariables = new Dictionary<string, object>();

            Console.WriteLine($"Execute External Task from topic '{taskWorkerInfo.TopicName}': {externalTask}...");
            try
            {
                taskWorkerInfo.TaskAdapter.Execute(externalTask, ref resultVariables);
                Console.WriteLine($"...finished External Task {externalTask.Id}");
                externalTaskService.Complete(workerId, externalTask.Id, resultVariables);
            }
            catch (UnrecoverableBusinessErrorException ex)
            {
                Console.WriteLine($"...failed with business error code from External Task  {externalTask.Id}");
                externalTaskService.Error(workerId, externalTask.Id, ex.BusinessErrorCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"...failed External Task  {externalTask.Id}");
                var retriesLeft = taskWorkerInfo.Retries; // start with default
                if (externalTask.Retries.HasValue) // or decrement if retries are already set
                {
                    retriesLeft = externalTask.Retries.Value - 1;
                }
                externalTaskService.Failure(workerId, externalTask.Id, ex.Message, retriesLeft, taskWorkerInfo.RetryTimeout);
            }
        }

        public void StartWork()
        {
            this.taskQueryTimer = new Timer(_ => DoPolling(), null, pollingIntervalInMilliseconds, Timeout.Infinite);
        }

        public void StopWork()
        {
            this.taskQueryTimer.Dispose();
            this.taskQueryTimer = null;
        }

        public void Dispose()
        {
            if (this.taskQueryTimer !=null)
            {
                this.taskQueryTimer.Dispose();
            }
        }
    }
}
