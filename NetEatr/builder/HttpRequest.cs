using System;
using System.Collections.Generic;
using System.Net;
using NetEatr.Digester;
using System.Threading.Tasks;
using NetEatr.Base;

namespace NetEatr.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpRequest : IHttpRequest<HttpRequest>
    {

        private string Method;
        internal HttpRequest(string method)
        {
            Method = method;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HttpRequest AddAuthorization(string token)
        {
            return AddHeaders("Authorization", "bearer " + token);
        }


        private Dictionary<string, string> Headers;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpRequest AddHeaders(string key, string value)
        {
            if (Headers == null) Headers = new Dictionary<string, string>();
            Headers.Add(key, value);
            return this;
        }

        private Dictionary<string, string> Parameters;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpRequest AddParam(string key, string value)
        {
            if (Parameters == null) Parameters = new Dictionary<string, string>();
            Parameters.Add(key, value);
            return this;
        }

        private static string AddParameters(string toUrl, Dictionary<string, string> withParams)
        {
            var parameters = "";
            foreach (var biConsumer in withParams)
            {
                var key = Uri.EscapeDataString(biConsumer.Key);
                var value = Uri.EscapeDataString(biConsumer.Value);
                parameters += key + "=" + value + "&";
            }
            if (parameters.Length >= 0) parameters.Remove(parameters.Length - 1);
            if (parameters.Length >= 0) parameters = "?" + parameters;
            return toUrl + parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public HttpRequest SetHeaders(Dictionary<string, string> headers)
        {
            Headers = headers;
            return this;
        }

        private Action<HttpWebRequest> OnBeforeSending = (_) => { };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="onBeforeSending"></param>
        /// <returns></returns>
        public HttpRequest SetOnBeforeSending(Action<HttpWebRequest> onBeforeSending)
        {
            OnBeforeSending = onBeforeSending;
            return this;
        }
        
        private Action<Exception> OnException;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="onException"></param>
        /// <returns></returns>
        public HttpRequest SetOnException(Action<Exception> onException)
        {
            OnException = onException;
            return this;
        }
        
        private Action<float> OnProgress = (_) => { };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="onProgress"></param>
        /// <returns></returns>
        public HttpRequest SetOnProgress(Action<float> onProgress)
        {
            OnProgress = onProgress;
            return this;
        }

        private Type TypeOfResponse = null;
        private Action<Response> OnResponded = (_) => { };
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="onResponded"></param>
        /// <returns></returns>
        public HttpRequest SetOnResponded<V>(Action<RestResponse<V>> onResponded)
        {
            TypeOfResponse = typeof(RestResponse<V>);
            OnResponded = (response) => {
                onResponded((RestResponse<V>)response);
            };
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onResponded"></param>
        /// <returns></returns>
        public HttpRequest SetOnResponded(Action<Response> onResponded)
        {
            TypeOfResponse = null;
            OnResponded = onResponded;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public HttpRequest SetParams(Dictionary<string, string> parameters)
        {
            Parameters = parameters;
            return this;
        }

        private int Timeout;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public HttpRequest SetTimeout(int timeout)
        {
            Timeout = timeout;
            return this;
        }

        private string Url;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HttpRequest SetUrl(string url)
        {
            Url = url;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<Response> GetAsyncExecute()
        {
            var task = new Task<Response>(() =>
            {
                return AwaitExecute();
            });
            task.Start();
            return task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<RestResponse<T>> GetAsyncExecute<T>()
        {
            var task = new Task<RestResponse<T>>(() =>
            {
                return AwaitExecute<T>();
            });
            task.Start();
            return task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onFinished"></param>
        public void AsyncExecute(Action<Response> onFinished)
        {
            new Task(() =>
            {
                var response = AwaitExecute();
                onFinished(response);
            }).Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onFinished"></param>
        public void AsyncExecute<T>(Action<RestResponse<T>> onFinished)
        {
            new Task(() =>
            {
                var response = AwaitExecute<T>();
                onFinished(response);
            }).Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AsyncExecute()
        {
            new Task(() => AwaitExecute()).Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Response AwaitExecute()
        {
            try
            {
                OnProgress(0.0f);
                var url = AddParameters(Url, Parameters);
                OnProgress(0.166667f);
                var request = WebRequest.CreateHttp(url);
                OnProgress(0.333334f);
                request.Method = Method;
                OnProgress(0.500001f);
                OnBeforeSending(request);
                var task = request.GetResponseAsync();
                OnProgress(0.666667f);
                var webResponse = (HttpWebResponse)task.Result;
                OnProgress(0.833333f);
                if (TypeOfResponse == null)
                {
                    var response = new Response(webResponse);
                    OnResponded(response);
                    OnProgress(1f);
                    return response;
                }
                else
                {
                    var response = (Response)Activator.CreateInstance(TypeOfResponse, webResponse);
                    OnResponded(response);
                    OnProgress(1f);
                    return response;
                }
            }
            catch (Exception exception)
            {
                if (exception is WebException webException)
                {
                    if (webException.Status == WebExceptionStatus.Timeout)
                    {
                        if (OnTimeout == null) throw exception;
                        else OnTimeout();
                    }
                    else
                    {
                        if (OnException == null) throw exception;
                        else OnException(exception);
                    }
                }
                else
                {
                    if (OnException == null) throw exception;
                    else OnException(exception);
                }
                var response = new Response(null, exception);
                OnProgress(1f);
                return response;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public RestResponse<T> AwaitExecute<T>()
        {
            try
            {
                OnProgress(0.0f);
                var url = AddParameters(Url, Parameters);
                OnProgress(0.166667f);
                var request = WebRequest.CreateHttp(url);
                OnProgress(0.333334f);
                request.Method = Method;
                OnProgress(0.500001f);
                OnBeforeSending(request);
                var task = request.GetResponseAsync();
                OnProgress(0.666667f);
                var webResponse = (HttpWebResponse)task.Result;
                OnProgress(0.833333f);
                var response = new RestResponse<T>(webResponse);
                OnResponded(response);
                OnProgress(1f);
                return response;
            }
            catch (Exception exception)
            {
                if (exception is WebException webException)
                {
                    if (webException.Status == WebExceptionStatus.Timeout)
                    {
                        if (OnTimeout == null) throw exception;
                        else OnTimeout();
                    }
                    else
                    {
                        if (OnException == null) throw exception;
                        else OnException(exception);
                    }
                }
                else
                {
                    if (OnException == null) throw exception;
                    else OnException(exception);
                }
                var response = new RestResponse<T>(null, exception);
                OnProgress(1f);
                return response;
            }
        }

        private Action OnTimeout;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="onTimeout"></param>
        /// <returns></returns>
        public HttpRequest SetOnTimeout(Action onTimeout)
        {
            OnTimeout = onTimeout;
            return this;
        }
    }
}
