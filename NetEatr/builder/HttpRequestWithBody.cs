using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetEatr.Digester;
using Newtonsoft.Json;
using NetEatr.Base;
using System.Xml;

namespace NetEatr.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpRequestWithBody : IHttpRequestWithBody<HttpRequestWithBody>
    {
        private HttpRequest httpRequestDelegate;
        internal HttpRequestWithBody(string method)
        {
            httpRequestDelegate = new HttpRequest(method);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HttpRequestWithBody AddAuthorization(string token)
        {
            httpRequestDelegate.AddAuthorization(token);
            return this;
        }

        private string Body;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public HttpRequestWithBody AddBody(string body)
        {
            Body = body;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public HttpRequestWithBody AddXmlBody(XmlDocument xml)
        {
            Body = xml.ToString();
            return AddHeaders("content-type", "text/xml; encoding='utf-8'");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpRequestWithBody AddHeaders(string key, string value)
        {
            httpRequestDelegate.AddHeaders(key, value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpRequestWithBody AddJsonBody<V>(V obj)
        {
            Body = JsonConvert.SerializeObject(obj);
            return AddHeaders("content-type", "application/json");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpRequestWithBody AddParam(string key, string value)
        {
            httpRequestDelegate.AddParam(key, value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public HttpRequestWithBody SetHeaders(Dictionary<string, string> headers)
        {
            httpRequestDelegate.SetHeaders(headers);
            return this;
        }

        Action<HttpWebRequest> OnBeforeSending = (HttpWebRequest request) => { };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="onBeforeSending"></param>
        /// <returns></returns>
        public HttpRequestWithBody SetOnBeforeSending(Action<HttpWebRequest> onBeforeSending)
        {
            OnBeforeSending = onBeforeSending;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onException"></param>
        /// <returns></returns>
        public HttpRequestWithBody SetOnException(Action<Exception> onException)
        {
            httpRequestDelegate.SetOnException(onException);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onProgress"></param>
        /// <returns></returns>
        public HttpRequestWithBody SetOnProgress(Action<float> onProgress)
        {
            httpRequestDelegate.SetOnProgress(onProgress);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public HttpRequestWithBody SetParams(Dictionary<string, string> parameters)
        {
            httpRequestDelegate.SetParams(parameters);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public HttpRequestWithBody SetTimeout(int timeout)
        {
            httpRequestDelegate.SetTimeout(timeout);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HttpRequestWithBody SetUrl(string url)
        {
            httpRequestDelegate.SetUrl(url);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AsyncExecute()
        {
            httpRequestDelegate.AsyncExecute();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<Response> GetAsyncExecute()
        {
            return httpRequestDelegate.GetAsyncExecute();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<RestResponse<T>> GetAsyncExecute<T>()
        {
            return httpRequestDelegate.GetAsyncExecute<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onFinished"></param>
        public void AsyncExecute(Action<Response> onFinished)
        {
            httpRequestDelegate.AsyncExecute(onFinished);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onFinished"></param>
        public void AsyncExecute<T>(Action<RestResponse<T>> onFinished)
        {
            httpRequestDelegate.AsyncExecute(onFinished);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onTimeout"></param>
        /// <returns></returns>
        public HttpRequestWithBody SetOnTimeout(Action onTimeout)
        {
            httpRequestDelegate.SetOnTimeout(onTimeout);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="onResponded"></param>
        /// <returns></returns>
        public HttpRequestWithBody SetOnResponded<V>(Action<RestResponse<V>> onResponded)
        {
            httpRequestDelegate.SetOnResponded(onResponded);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onResponded"></param>
        /// <returns></returns>
        public HttpRequestWithBody SetOnResponded(Action<Response> onResponded)
        {
            httpRequestDelegate.SetOnResponded(onResponded);
            return this;
        }
    }
}
