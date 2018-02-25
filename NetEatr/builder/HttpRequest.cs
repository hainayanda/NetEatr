using NetEatr.Base;
using NetEatr.Digester;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace NetEatr.Builder
{
    /// <summary>
    /// Basic HttpRequest Object
    /// </summary>
    public class HttpRequest : IHttpRequest<HttpRequest>
    {
        private IDictionary<string, string> _Headers;

        private IDictionary<string, string> _Parameters;

        private int _Timeout = 10000;

        private string _Url;

        private string Method;

        private Action<HttpWebRequest> OnBeforeSending = (_) => { };

        private Action<Exception> OnException;

        private Action<float> OnProgress = (_) => { };

        private Action<Response> OnResponded = (_) => { };

        private Action OnTimeout;

        private Type TypeOfResponse = null;

        internal HttpRequest(string method)
        {
            Method = method;
        }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        /// <exception cref="ArgumentNullException">Headers cannot be null</exception>
        public IDictionary<string, string> Headers
        {
            get => _Headers;
            set
            {
                if (value == null) throw new ArgumentNullException("Headers cannot be null");
                else _Headers = value;
            }
        }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        /// <exception cref="ArgumentNullException">Headers cannot be null</exception>
        public IDictionary<string, string> Parameters
        {
            get => _Parameters;
            set
            {
                if (value == null) throw new ArgumentNullException("Headers cannot be null");
                else _Parameters = value;
            }
        }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        /// <exception cref="ArgumentException">Timeout is too small</exception>
        public int Timeout
        {
            get => _Timeout;
            set
            {
                if (value < 100) throw new ArgumentException("Timeout is too small");
                else _Timeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        /// <exception cref="ArgumentNullException">Url cannot be null</exception>
        public string Url
        {
            get => _Url;
            set
            {
                if (value == null) throw new ArgumentNullException("Url cannot be null");
                else _Url = value;
            }
        }

        /// <summary>
        /// Method to add authorization
        /// it will automatically using a bearer oAuth authorization
        /// </summary>
        /// <param name="token"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest AddAuthorization(string token)
        {
            return AddHeaders("Authorization", "bearer " + token);
        }

        /// <summary>
        /// Method to add headers
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest AddHeaders(string key, string value)
        {
            if (Headers == null) Headers = new Dictionary<string, string>();
            Headers.Add(key, value);
            return this;
        }

        /// <summary>
        /// Method to add parameter
        /// it will be automatically encoded the parameter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest AddParam(string key, string value)
        {
            if (Parameters == null) Parameters = new Dictionary<string, string>();
            Parameters.Add(key, value);
            return this;
        }

        /// <summary>
        /// Method to execute HttpRequest asynchronously
        /// </summary>
        /// <param name="onFinished">Delegate to run when execution is finished</param>
        public void AsyncExecute(Action<Response> onFinished)
        {
            new Task(() =>
            {
                var response = AwaitExecute();
                onFinished(response);
            }).Start();
        }

        /// <summary>
        /// Method to execute HttpRequest asynchronously
        /// </summary>
        /// <typeparam name="T">Type of object generated for Json Parsing</typeparam>
        /// <param name="onFinished">Delegate to run when execution is finished</param>
        public void AsyncExecute<T>(Action<RestResponse<T>> onFinished)
        {
            new Task(() =>
            {
                var response = AwaitExecute<T>();
                onFinished(response);
            }).Start();
        }

        /// <summary>
        /// Method to execute HttpRequest asynchronously
        /// </summary>
        public void AsyncExecute()
        {
            new Task(() => AwaitExecute()).Start();
        }

        /// <summary>
        /// Method to execute HttpRequest synchronously
        /// </summary>
        /// <returns>
        /// Response object
        /// </returns>
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
        /// Method to execute HttpRequest synchronously
        /// </summary>
        /// <typeparam name="T">Type of object generated for Json Parsing</typeparam>
        /// <returns>
        /// RestResponse of T
        /// </returns>
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

        /// <summary>
        /// Method to get asynchronous execution in form of task
        /// </summary>
        /// <returns>
        /// Task which have return type of Response
        /// </returns>
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
        /// Method to get asynchronous execution in form of task
        /// </summary>
        /// <typeparam name="T">Type of object generated for Json Parsing</typeparam>
        /// <returns>
        /// Task which have return type of RestResponse of T
        /// </returns>
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
        /// Method to set headers
        /// </summary>
        /// <param name="headers">dictionary of header in form of key - value</param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest SetHeaders(IDictionary<string, string> headers)
        {
            Headers = headers;
            return this;
        }

        /// <summary>
        /// Method to set delegate to run right before sending
        /// </summary>
        /// <param name="onBeforeSending"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest SetOnBeforeSending(Action<HttpWebRequest> onBeforeSending)
        {
            OnBeforeSending = onBeforeSending;
            return this;
        }

        /// <summary>
        /// Method to set the delegate to run rigth after exception
        /// </summary>
        /// <param name="onException"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest SetOnException(Action<Exception> onException)
        {
            OnException = onException;
            return this;
        }

        /// <summary>
        /// Method to set delegate to run every progress
        /// progress start with 0 and end in 1
        /// </summary>
        /// <param name="onProgress"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest SetOnProgress(Action<float> onProgress)
        {
            OnProgress = onProgress;
            return this;
        }

        /// <summary>
        /// Method to set the delegate to run right after get response
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="onResponded"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest SetOnResponded<V>(Action<RestResponse<V>> onResponded)
        {
            TypeOfResponse = typeof(RestResponse<V>);
            OnResponded = (response) =>
            {
                onResponded((RestResponse<V>)response);
            };
            return this;
        }

        /// <summary>
        /// Method to set the delegate to run right after get response
        /// </summary>
        /// <param name="onResponded"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest SetOnResponded(Action<Response> onResponded)
        {
            TypeOfResponse = null;
            OnResponded = onResponded;
            return this;
        }

        /// <summary>
        /// Method to set the delegate to run right after timeout
        /// </summary>
        /// <param name="onTimeout"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest SetOnTimeout(Action onTimeout)
        {
            OnTimeout = onTimeout;
            return this;
        }

        /// <summary>
        /// Method to set url parameter
        /// it will be automatically encoded the parameter
        /// </summary>
        /// <param name="parameters">dictionary of key - value</param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest SetParams(IDictionary<string, string> parameters)
        {
            Parameters = parameters;
            return this;
        }

        /// <summary>
        /// Method to add timeout
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest SetTimeout(int timeout)
        {
            Timeout = timeout;
            return this;
        }

        /// <summary>
        /// Method to set the url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequest SetUrl(string url)
        {
            Url = url;
            return this;
        }

        private static string AddParameters(string toUrl, IDictionary<string, string> withParams)
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
    }
}