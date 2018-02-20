using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetEatr.Digester;
using Newtonsoft.Json;
using NetEatr.Base;

namespace NetEatr.Builder
{
    public class HttpRequestWithBody : IHttpRequestWithBody<HttpRequestWithBody>
    {
        private HttpRequest httpRequestDelegate;
        internal HttpRequestWithBody(string method)
        {
            httpRequestDelegate = new HttpRequest(method);
        }

        public HttpRequestWithBody AddAuthorization(string token)
        {
            httpRequestDelegate.AddAuthorization(token);
            return this;
        }

        private string Body;
        public HttpRequestWithBody AddBody(string body)
        {
            Body = body;
            return this;
        }

        public HttpRequestWithBody AddFormUrlEncoded(Dictionary<string, string> forms)
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

        public HttpRequestWithBody AddHeaders(string key, string value)
        {
            httpRequestDelegate.AddHeaders(key, value);
            return this;
        }

        public HttpRequestWithBody AddJsonBody<V>(V obj)
        {
            Body = JsonConvert.SerializeObject(obj);
            return AddHeaders("content-type", "application/json");
        }

        public HttpRequestWithBody AddParam(string key, string value)
        {
            httpRequestDelegate.AddParam(key, value);
            return this;
        }

        public HttpRequestWithBody SetHeaders(Dictionary<string, string> headers)
        {
            httpRequestDelegate.SetHeaders(headers);
            return this;
        }

        Action<HttpWebRequest> OnBeforeSending = (HttpWebRequest request) => { };
        public HttpRequestWithBody SetOnBeforeSending(Action<HttpWebRequest> onBeforeSending)
        {
            OnBeforeSending = onBeforeSending;
            return this;
        }

        public HttpRequestWithBody SetOnException(Action<Exception> onException)
        {
            httpRequestDelegate.SetOnException(onException);
            return this;
        }

        public HttpRequestWithBody SetOnProgress(Action<float> onProgress)
        {
            httpRequestDelegate.SetOnProgress(onProgress);
            return this;
        }

        public HttpRequestWithBody SetOnResponded<V>(Action<V> onResponded) where V : Response
        {
            httpRequestDelegate.SetOnResponded(onResponded);
            return this;
        }

        public HttpRequestWithBody SetParams(Dictionary<string, string> parameters)
        {
            httpRequestDelegate.SetParams(parameters);
            return this;
        }

        public HttpRequestWithBody SetTimeout(int timeout)
        {
            httpRequestDelegate.SetTimeout(timeout);
            return this;
        }

        public HttpRequestWithBody SetUrl(string url)
        {
            httpRequestDelegate.SetUrl(url);
            return this;
        }

        public void AsyncExecute()
        {
            httpRequestDelegate.AsyncExecute();
        }

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
            httpRequestDelegate.SetOnBeforeSending(onBeforeSendingOverride);
            return httpRequestDelegate.AwaitExecute();
        }

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
            httpRequestDelegate.SetOnBeforeSending(onBeforeSendingOverride);
            return httpRequestDelegate.AwaitExecute<T>();
        }

        public Task<Response> GetAsyncExecute()
        {
            return httpRequestDelegate.GetAsyncExecute();
        }

        public Task<RestResponse<T>> GetAsyncExecute<T>()
        {
            return httpRequestDelegate.GetAsyncExecute<T>();
        }

        public void AsyncExecute(Action<Response> onFinished)
        {
            httpRequestDelegate.AsyncExecute(onFinished);
        }

        public void AsyncExecute<T>(Action<RestResponse<T>> onFinished)
        {
            httpRequestDelegate.AsyncExecute(onFinished);
        }

        public HttpRequestWithBody SetOnTimeout(Action onTimeout)
        {
            httpRequestDelegate.SetOnTimeout(onTimeout);
            return this;
        }
    }
}
