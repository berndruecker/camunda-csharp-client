using CamundaClient.Dto;
using System.Collections.Generic;

namespace CamundaClient.Worker
{

    public interface IExternalTaskAdapter
    {
        void Execute(ExternalTask externalTask, ref Dictionary<string, object> resultVariables);
    }


}
