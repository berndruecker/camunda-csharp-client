using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamundaClient.Requests
{
    class BpmnErrorRequest
    {
        public string WorkerId { get; set; }
        public string ErrorCode { get; set; }
    }
}
