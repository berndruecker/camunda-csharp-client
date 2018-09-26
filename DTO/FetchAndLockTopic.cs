using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamundaClient.Dto
{
    public class FetchAndLockTopic
    {
        public string TopicName { get; set; }
        public long LockDuration { get; set; }
        public IEnumerable<string> Variables { get; set; }
    }
}
