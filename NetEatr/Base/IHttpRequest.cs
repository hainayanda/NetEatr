using NetEatr.Digester;
using System;
using System.Collections.Generic;
using System.Net;

namespace NetEatr.Base
{
    public interface IHttpRequest<T> : IHttpExecutor where T : IHttpExecutor
    {
        T SetUrl(string url);
        
        T SetParams(Dictionary<string, string> parameters);
        
        T AddParam(string key, string value);
        
        T SetHeaders(Dictionary<string, string> headers);
        
        T AddHeaders(string key, string value);
        
        T AddAuthorization(string token);
        
        T SetTimeout(int timeout);

        T SetOnProgress(Action<float> onProgress);

        T SetOnBeforeSending(Action<HttpWebRequest> onBeforeSending);

        T SetOnResponded<V>(Action<V> onResponded) where V : Response;

        T SetOnException(Action<Exception> onException);

        T SetOnTimeout(Action onTimeout);
    }
}
