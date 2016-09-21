using System;

namespace EnergyTrading.WebApi.Common.Client
{
    public static class HttpPath
    {
        public static string Combine(string uriPart, string path)
        {
            if (string.IsNullOrWhiteSpace(uriPart))
            {
                throw new ArgumentException($"{nameof(uriPart)} cannot be null or White Space.", nameof(uriPart));
            }
            try
            {
                new Uri(uriPart);
            }
            catch
            {
                throw new ArgumentException($"{nameof(uriPart)} must be a valid uri string.", nameof(uriPart));
            }

            var ret = uriPart.EndsWith("/") ? uriPart.Substring(0, uriPart.Length - 1) : uriPart;
            if (path != null)
            {
                var withoutSlash = path.StartsWith("/") ? path.Substring(1) : path;
                if (!string.IsNullOrWhiteSpace(withoutSlash))
                {
                    ret += "/" + withoutSlash;
                }
            }
            return ret;
        }
    }
}