using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CamundaClient.Worker;

namespace CamundaClient.Dto
{
    public class ExternalTaskWorkerInfo
    {
        public int Retries { get; internal set; }
        public long RetryTimeout { get; internal set; }
        public Type Type { get; internal set; }
        public string TopicName { get; internal set; }
        public List<string> VariablesToFetch { get; internal set; }
        public IExternalTaskAdapter TaskAdapter { get; internal set; }
    }
}
