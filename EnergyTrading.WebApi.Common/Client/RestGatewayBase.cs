using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
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

        public Task<TReturn> GetAsync<TReturn>(string uri)
        {
            return CallClient<TReturn>(c => c.GetAsync(uri));
        }

        protected async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default(T);
                }
                var contentString = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(contentString))
                {
                    return default(T);
                }
                T ret;
                try
                {
                    ret = await response.Content.ReadAsAsync<T>(new MediaTypeFormatter[] { jsonMediaFormatter });
                }
                catch (Exception exception)
                {
                    throw new WebClientException("Could not read response as type : " + typeof(T) + ", Message : " + ToExceptionMessage(exception));
                }
                if (ret == null)
                {
                    throw new WebClientException("Could not read response as type : " + typeof(T));
                }
                return ret;
            }

            var errorMessage = response.ReasonPhrase;
            if (response.Content.Headers.ContentType.MediaType.Contains("json"))
            {
                Fault fault;
                try
                {
                    fault = await response.Content.ReadAsAsync<Fault>(new MediaTypeFormatter[] { jsonMediaFormatter });
                }
                catch (Exception exception)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    throw new WebClientException("Could not obtain error as Fault. Exception : " + ToExceptionMessage(exception) + ", Content : " + content);
                }
                if (fault != null)
                {
                    errorMessage = fault.ErrorMessage;
                }
                else
                {
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            else
            {
                errorMessage = await response.Content.ReadAsStringAsync();
            }
            throw new WebClientException(errorMessage);
        }

        protected async Task<TReturn> CallClient<TReturn>(Func<HttpClient, Task<HttpResponseMessage>> callFunc)
        {
            if (callFunc == null)
            {
                return default(TReturn);
            }
            try
            {
                using (var client = CreateHttpClient())
                {
                    var response = await callFunc(client);
                    return await HandleResponse<TReturn>(response);
                }
            }
            catch (Exception e)
            {
                throw new WebClientException(e.Message, e);
            }
        }

        protected bool TryAction<TInput, TOutput>(string url, TInput item, out TOutput result, Func<string, TInput, TOutput> action)
        {
            result = default(TOutput);
            if ((!typeof(TInput).IsValueType && item == null) || action == null)
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

        public static string ToExceptionMessage(Exception exception)
        {
            var aggregate = exception as AggregateException;
            if (aggregate != null)
            {
                aggregate.Flatten(); // flatten first to get rid of inner AggregateExceptions
                return aggregate.InnerExceptions.Aggregate(string.Empty, (s, e) =>
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        return e.Message;
                    }
                    return s + " ;; " + e.Message;
                });
            }
            return exception.Message;
        }

        public static bool TryAction(Action action, out string exceptionMessage)
        {
            exceptionMessage = null;
            try
            {
                action();
                return true;
            }
            catch (Exception exception)
            {
                exceptionMessage = ToExceptionMessage(exception);
                return false;
            }
        }

        protected async Task<TReturn> PostAsync<TInput, TReturn>(string url, TInput item) 
        {
            return await CallClient<TReturn>(c => c.PostAsync(url, item, jsonMediaFormatter));
        }

        protected TReturn Post<TInput, TReturn>(string url, TInput item) 
        {
            return PostAsync<TInput, TReturn>(url, item).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        protected bool TryPost<TInput, TOutput>(string url, TInput item, out TOutput result) 
        {
            return TryAction(url, item, out result, Post<TInput, TOutput>);
        }

        protected async Task<TReturn> PutAsync<TInput, TReturn>(string url, TInput item) 
        {
            return await CallClient<TReturn>(c => c.PutAsync(url, item, jsonMediaFormatter));
        }

        protected TReturn Put<TInput, TReturn>(string url, TInput item) 
        {
            return PutAsync<TInput, TReturn>(url, item).Result;
        }

        protected bool TryPut<TInput, TReturn>(string url, TInput item, out TReturn result) 
        {
            return TryAction(url, item, out result, Put<TInput, TReturn>);
        }

        protected async Task<TReturn> DeleteAsync<TReturn>(string url)
        {
            return await CallClient<TReturn>(c => c.DeleteAsync(url));
        }

        protected TReturn Delete<TReturn>(string url)
        {
            return DeleteAsync<TReturn>(url).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        protected bool TryDelete<TReturn>(string url)
        {
            string message;
            return TryAction(() => Delete<TReturn>(url), out message);;
        }
    }
}