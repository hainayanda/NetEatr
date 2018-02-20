using System.Collections.Generic;

namespace NetEatr.Base
{
    public interface IHttpRequestWithBody<T> : IHttpRequest<T> where T : IHttpExecutor
    {
        T AddBody(string body);

        T AddJsonBody<V>(V obj);

        T AddFormUrlEncoded(Dictionary<string, string> forms);
    }
}
