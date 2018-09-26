using CamundaClient.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using CamundaClient.Requests;

namespace CamundaClient.Service
{

    public class RepositoryService
    {
        private CamundaClientHelper helper;

        public RepositoryService(CamundaClientHelper helper)
        {
            this.helper = helper;
        }


        public List<ProcessDefinition> LoadProcessDefinitions(bool onlyLatest)
        {
            var http = helper.HttpClient();
            HttpResponseMessage response = http.GetAsync("process-definition/?latestVersion=" + (onlyLatest ? "true" : "false")).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<IEnumerable<ProcessDefinition>>(response.Content.ReadAsStringAsync().Result);

                // Could be extracted into separate method call if you run a lot of process definitions and want to optimize performance
                foreach (ProcessDefinition pd in result)
                {
                    http = helper.HttpClient();
                    HttpResponseMessage response2 = http.GetAsync("process-definition/" + pd.Id + "/startForm").Result;
                    var startForm = JsonConvert.DeserializeObject<StartForm>(response2.Content.ReadAsStringAsync().Result);

                    pd.StartFormKey = startForm.Key;
                }
                return new List<ProcessDefinition>(result);
            }
            else
            {
                return new List<ProcessDefinition>();
            }

        }


        public String LoadProcessDefinitionXml(String processDefinitionId)
        {
            var http = helper.HttpClient();
            HttpResponseMessage response = http.GetAsync("process-definition/" + processDefinitionId + "/xml").Result;
            if (response.IsSuccessStatusCode)
            {
                ProcessDefinitionXml processDefinitionXml = JsonConvert.DeserializeObject<ProcessDefinitionXml>(response.Content.ReadAsStringAsync().Result);
                return processDefinitionXml.Bpmn20Xml;
            }
            else
            {
                return null;
            }            
        }

        public void DeleteDeployment(string deploymentId)
        {
            HttpClient http = helper.HttpClient();
            HttpResponseMessage response = http.DeleteAsync("deployment/" + deploymentId + "?cascade=true").Result;
            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = response.Content.ReadAsStringAsync();
                throw new EngineException(response.ReasonPhrase);
            }
        }

        public string Deploy(string deploymentName, List<object> files)
        {
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("deployment-name", deploymentName);
            postParameters.Add("deployment-source", "C# Process Application");
            postParameters.Add("enable-duplicate-filtering", "true");
            postParameters.Add("data", files);

            // Create request and receive response
            string postURL = helper.RestUrl + "deployment/create";
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, helper.RestUsername, helper.RestPassword, postParameters);

            using (var reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
            {
                var deployment = JsonConvert.DeserializeObject<Deployment>(reader.ReadToEnd());
                return deployment.Id;
            }
        }

        public void AutoDeploy()
        {
            Assembly thisExe = Assembly.GetEntryAssembly();
            string[] resources = thisExe.GetManifestResourceNames();

            if (resources.Length == 0)
            {
                return;
            }

            List<object> files = new List<object>();
            foreach (string resource in resources)
            {
                // TODO Check if Camunda relevant (BPMN, DMN, HTML Forms)

                // Read and add to Form for Deployment                
                files.Add(FileParameter.FromManifestResource(thisExe, resource));

                Console.WriteLine("Adding resource to deployment: " + resource);
            }

            Deploy(thisExe.GetName().Name, files);

            Console.WriteLine("Deployment to Camunda BPM succeeded.");

        }

    }
}
