using System.Collections.Generic;

namespace CamundaClient.Worker
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct)
    ]
    public sealed class ExternalTaskVariableRequirementsAttribute : System.Attribute
    {
        public List<string> VariablesToFetch { get; }

        public ExternalTaskVariableRequirementsAttribute()
        {
            VariablesToFetch = new List<string>();
        }

        public ExternalTaskVariableRequirementsAttribute(params string[] variablesToFetch)
        {
            VariablesToFetch = new List<string>(variablesToFetch);
        }

    }
}
