using System;
using System.Runtime.Serialization;

namespace EnergyTrading.WebApi.Common.Client
{
    [Serializable]
    public class WebClientException : Exception
    {
        public WebClientException()
        {
        }

        public WebClientException(string message) : base(message)
        {
        }

        public WebClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WebClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}