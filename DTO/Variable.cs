using System.Collections.Generic;

namespace CamundaClient.Dto
{
    public class Variable
    {
        // lower case to generate JSON we need
        public string Type { get; set; }
        public object Value { get; set; }
        public object ValueInfo { get; set; }
    }

}
