using System.Collections.Generic;

namespace CamundaClient.Dto
{
    public class Deployment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }

        public override string ToString() => $"Deployment [Id={Id}, Name={Name}]";
    }

}
