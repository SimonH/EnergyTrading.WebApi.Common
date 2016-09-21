using System;

namespace EnergyTrading.WebApi.Common.Client
{
    public abstract class ServiceGatewayBase : RestGatewayBase
    {
        protected ServiceGatewayBase(string baseUri, string path)
        {
            ServiceUri = HttpPath.Combine(baseUri, path);
        }

        protected string ServiceUri { get; }
    }
}