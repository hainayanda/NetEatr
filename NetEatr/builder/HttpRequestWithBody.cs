using NetEatr.Base;
using NetEatr.Digester;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NetEatr.Builder
{
    /// <summary>
    /// Http Request with body
    /// </summary>
    public class HttpRequestWithBody : IHttpRequestWithBody<HttpRequestWithBody>
    {
        private string _Body;

        private HttpRequest Delegate;

        private Action<HttpWebRequest> OnBeforeSending = (HttpWebRequest request) => { };

        internal HttpRequestWithBody(string method)
        {
            Delegate = new HttpRequest(method);
        }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        /// <exception cref="ArgumentNullException">Body cannot be null</exception>
        public string Body
        {
            get => _Body;
            set
            {
                if (value == null) throw new ArgumentNullException("Body cannot be null");
                else _Body = value;
            }
        }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public IDictionary<string, string> Headers
        {
            get => Delegate.Headers;
            set => Delegate.Headers = value;
        }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IDictionary<string, string> Parameters
        {
            get => Delegate.Parameters;
            set => Delegate.Parameters = value;
        }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        public int Timeout
        {
            get => Delegate.Timeout;
            set => Delegate.Timeout = value;
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url
        {
            get => Delegate.Url;
            set => Delegate.Url = value;
        }

        /// <summary>
        /// Method to add authorization
        /// it will automatically using a bearer oAuth authorization
        /// </summary>
        /// <param name="token"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequestWithBody AddAuthorization(string token)
        {
            Delegate.AddAuthorization(token);
            return this;
        }

        /// <summary>
        /// Method to add string body
        /// </summary>
        /// <param name="body"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequestWithBody AddBody(string body)
        {
            Body = body;
            return this;
        }

        /// <summary>
        /// Method to add form url as body
        /// it will be encoded to url form
        /// itl will set the content into urlformencoded
        /// </summary>
        /// <param name="forms"></param>
        /// <returns>
        /// itself
        /// </returns>
        /// <exception cref="ArgumentNullException">Forms cannot be null</exception>
        public HttpRequestWithBody AddFormUrlEncoded(IDictionary<string, string> forms)
        {
            if (forms == null) throw new ArgumentNullException("Forms cannot be null");
            var form = "";
            foreach (var biConsumer in forms)
            {
                var key = Uri.EscapeDataString(biConsumer.Key);
                var value = Uri.EscapeDataString(biConsumer.Value);
                form += key + "=" + value + "&";
            }
            if (form.Length >= 0) form.Remove(form.Length - 1);
            Body = form;
            return AddHeaders("content-type", "application/x-www-form-urlencoded");
        }

        /// <summary>
        /// Method to add headers
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequestWithBody AddHeaders(string key, string value)
        {
            Delegate.AddHeaders(key, value);
            return this;
        }

        /// <summary>
        /// Method to add object as body
        /// it will be parsed to Json
        /// it will set the content into application/json
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequestWithBody AddJsonBody<V>(V obj)
        {
            Body = JsonConvert.SerializeObject(obj);
            return AddHeaders("content-type", "application/json");
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
        public HttpRequestWithBody AddParam(string key, string value)
        {
            Delegate.AddParam(key, value);
            return this;
        }

        /// <summary>
        /// Method to add xml as body
        /// itl will set the content into xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequestWithBody AddXmlBody(XmlDocument xml)
        {
            Body = xml.ToString();
            return AddHeaders("content-type", "text/xml; encoding='utf-8'");
        }

        /// <summary>
        /// Method to execute HttpRequest asynchronously
        /// </summary>
        public void AsyncExecute()
        {
            Delegate.AsyncExecute();
        }

        /// <summary>
        /// Method to execute HttpRequest asynchronously
        /// </summary>
        /// <param name="onFinished">Delegate to run when execution is finished</param>
        public void AsyncExecute(Action<Response> onFinished)
        {
            Delegate.AsyncExecute(onFinished);
        }

        /// <summary>
        /// Method to execute HttpRequest asynchronously
        /// </summary>
        /// <typeparam name="T">Type of object generated for Json Parsing</typeparam>
        /// <param name="onFinished">Delegate to run when execution is finished</param>
        public void AsyncExecute<T>(Action<RestResponse<T>> onFinished)
        {
            Delegate.AsyncExecute(onFinished);
        }

        /// <summary>
        /// Method to execute HttpRequest synchronously
        /// </summary>
        /// <returns>
        /// Response object
        /// </returns>
        public Response AwaitExecute()
        {
            Action<HttpWebRequest> onBeforeSendingOverride = (HttpWebRequest request) =>
            {
                var encoding = new ASCIIEncoding();
                var bytes = encoding.GetBytes(Body);
                var stream = request.GetRequestStreamAsync().Result;
                stream.Write(bytes, 0, bytes.Length);
                OnBeforeSending(request);
            };
            Delegate.SetOnBeforeSending(onBeforeSendingOverride);
            return Delegate.AwaitExecute();
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
            Action<HttpWebRequest> onBeforeSendingOverride = (HttpWebRequest request) =>
            {
                var encoding = new ASCIIEncoding();
                var bytes = encoding.GetBytes(Body);
                var stream = request.GetRequestStreamAsync().Result;
                stream.Write(bytes, 0, bytes.Length);
                OnBeforeSending(request);
            };
            Delegate.SetOnBeforeSending(onBeforeSendingOverride);
            return Delegate.AwaitExecute<T>();
        }

        /// <summary>
        /// Method to get asynchronous execution in form of task
        /// </summary>
        /// <returns>
        /// Task which have return type of Response
        /// </returns>
        public Task<Response> GetAsyncExecute()
        {
            return Delegate.GetAsyncExecute();
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
            return Delegate.GetAsyncExecute<T>();
        }

        /// <summary>
        /// Method to set headers
        /// </summary>
        /// <param name="headers">dictionary of header in form of key - value</param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequestWithBody SetHeaders(IDictionary<string, string> headers)
        {
            Delegate.SetHeaders(headers);
            return this;
        }

        /// <summary>
        /// Method to set delegate to run right before sending
        /// </summary>
        /// <param name="onBeforeSending"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequestWithBody SetOnBeforeSending(Action<HttpWebRequest> onBeforeSending)
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
        public HttpRequestWithBody SetOnException(Action<Exception> onException)
        {
            Delegate.SetOnException(onException);
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
        public HttpRequestWithBody SetOnProgress(Action<float> onProgress)
        {
            Delegate.SetOnProgress(onProgress);
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
        public HttpRequestWithBody SetOnResponded<V>(Action<RestResponse<V>> onResponded)
        {
            Delegate.SetOnResponded(onResponded);
            return this;
        }

        /// <summary>
        /// Method to set the delegate to run right after get response
        /// </summary>
        /// <param name="onResponded"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequestWithBody SetOnResponded(Action<Response> onResponded)
        {
            Delegate.SetOnResponded(onResponded);
            return this;
        }

        /// <summary>
        /// Method to set the delegate to run right after timeout
        /// </summary>
        /// <param name="onTimeout"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequestWithBody SetOnTimeout(Action onTimeout)
        {
            Delegate.SetOnTimeout(onTimeout);
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
        public HttpRequestWithBody SetParams(IDictionary<string, string> parameters)
        {
            Delegate.SetParams(parameters);
            return this;
        }

        /// <summary>
        /// Method to add timeout
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequestWithBody SetTimeout(int timeout)
        {
            Delegate.SetTimeout(timeout);
            return this;
        }

        /// <summary>
        /// Method to set the url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>
        /// itself
        /// </returns>
        public HttpRequestWithBody SetUrl(string url)
        {
            Delegate.SetUrl(url);
            return this;
        }
    }
}