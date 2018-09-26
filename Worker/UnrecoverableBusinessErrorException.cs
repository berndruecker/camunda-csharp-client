using System;

namespace CamundaClient.Worker
{
    public class UnrecoverableBusinessErrorException : Exception
    {
        public string BusinessErrorCode { get; set; }
        
        public UnrecoverableBusinessErrorException(string businessErrorCode, string message)
        : base(message)
        {
            BusinessErrorCode = businessErrorCode;
        }

    }
}
