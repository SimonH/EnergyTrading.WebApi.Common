using System;

namespace EnergyTrading.WebApi.Common.Client
{
    public abstract class ServiceGatewayBase : RestGatewayBase
    {
        protected ServiceGatewayBase(string baseUri, string path)
        {
            if (string.IsNullOrWhiteSpace(baseUri))
            {
                throw new ArgumentOutOfRangeException(nameof(baseUri));
            }
            new Uri(baseUri); // tests that baseUri is a valid uri string
            // conformity strip of trailing / here and only add it if there is a path
            this.ServiceUri = baseUri.EndsWith("/") ? baseUri.Substring(0, baseUri.Length -1) : baseUri;
            if (path != null)
            {
                var withoutSlash = path.StartsWith("/") ? path.Substring(1) : path;
                if (!string.IsNullOrWhiteSpace(withoutSlash))
                {
                    ServiceUri += "/" + withoutSlash;
                }
            }
        }

        protected string ServiceUri { get; }
    }
}