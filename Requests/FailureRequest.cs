using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamundaClient.Requests
{
    class FailureRequest
    {
        public string WorkerId { get; set; }
        public string ErrorMessage { get; set; }
        public int Retries { get; set; }
        public long RetryTimeout { get; set; }
    }
}
