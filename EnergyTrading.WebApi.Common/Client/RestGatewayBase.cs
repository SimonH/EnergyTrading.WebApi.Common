using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using EnergyTrading.Filtering;
using EnergyTrading.WebApi.Common.Faults;
using Newtonsoft.Json;

namespace EnergyTrading.WebApi.Common.Client
{
    public abstract class RestGatewayBase
    {
        protected readonly JsonMediaTypeFormatter jsonMediaFormatter = new JsonMediaTypeFormatter { SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All } };

        protected HttpClient CreateHttpClient()
        {
            // used from ET.Mdm.Client to try and prevent proxy errors but did not work
            var handler = new HttpClientHandler { UseDefaultCredentials = true };

            return new HttpClient(handler);
        }

        protected T HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default(T);
                }
                var ret = response.Content.ReadAsAsync<T>(new MediaTypeFormatter[] { this.jsonMediaFormatter }).Result;
                if (ret == null)
                {
                    throw new WebClientException("Could not read response as type : " + typeof(T));
                }
                return ret;
            }

            var errorMessage = response.ReasonPhrase;
            if (response.Content.Headers.ContentType.MediaType.Contains("json"))
            {
                var fault = response.Content.ReadAsAsync<Fault>(new MediaTypeFormatter[] { this.jsonMediaFormatter }).Result;
                if (fault != null)
                {
                    errorMessage = fault.ErrorMessage;
                }
            }
            else
            {
                errorMessage = response.Content.ReadAsStringAsync().Result;
            }
            throw new WebClientException(errorMessage);
        }

        protected async Task<T> CallClient<T>(string url, T item, Func<HttpClient, Task<HttpResponseMessage>> callFunc)
        {
            if (callFunc == null)
            {
                return default(T);
            }
            try
            {
                using (var client = this.CreateHttpClient())
                {
                    var response = await callFunc(client); ;
                    return this.HandleResponse<T>(response);
                }
            }
            catch (Exception e)
            {
                throw new WebClientException(e.Message, e);
            }
        }

        protected bool TryAction<T>(string url, T item, out T result, Func<string, T, T> action)
        {
            result = default(T);
            if ((!typeof(T).IsValueType && item == null) || action == null)
            {
                return false;
            }

            try
            {
                result = action(url, item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected async Task<T> PostAsync<T>(string url, T item) 
        {
            return await CallClient(url, item, c => c.PostAsync(url, item, this.jsonMediaFormatter));
        }

        protected T Post<T>(string url, T item) 
        {
            return PostAsync(url, item).Result;
        }

        protected bool TryPost<T>(string url, T item, out T result) 
        {
            return TryAction(url, item, out result, this.Post);
        }

        protected async Task<T> PutAsync<T>(string url, T item) 
        {
            return await CallClient(url, item, c => c.PutAsync(url, item, jsonMediaFormatter));
        }

        protected T Put<T>(string url, T item) 
        {
            return PutAsync(url, item).Result;
        }

        protected bool TryPut<T>(string url, T item, out T result) 
        {
            return this.TryAction(url, item, out result, this.Put);
        }
    }
}