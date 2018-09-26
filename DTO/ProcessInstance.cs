using System.Collections.Generic;

namespace CamundaClient.Dto
{
    public class ProcessInstance
    {
        public string Id { get; set; }
        public string BusinessKey { get; set; }

        public override string ToString() => $"ProcessInstance [Id={Id}, BusinessKey={BusinessKey}]";
    }

}
