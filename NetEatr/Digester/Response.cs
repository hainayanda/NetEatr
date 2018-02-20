using System;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace NetEatr.Digester
{
    public class Response
    {
        public HttpWebResponse RawResponse { get; protected set; }

        public string RawBody { get; protected set; }

        public int StatusCode { get; protected set; }

        public bool IsSuccess
        {
            get
            {
                return StatusCode >= 200 && StatusCode <= 299;
            }
        }

        public Exception Exception { get; protected set; }

        public bool HadException
        {
            get
            {
                return Exception != null;
            }
        }

        public Response(HttpWebResponse webResponse)
        {
            if(webResponse != null)
            {
                RawResponse = webResponse;
                StatusCode = webResponse.StatusCode.GetHashCode();
                RawBody = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
            }
        }

        public Response(HttpWebResponse webResponse, Exception exception) : this(webResponse)
        {
            Exception = exception;
        }
    }

    public class RestResponse<T> : Response
    {
        public RestResponse(HttpWebResponse webResponse) : base(webResponse) { }

        public RestResponse(HttpWebResponse webResponse, Exception exception) : base(webResponse, exception) { }

        public T JsonBody
        {
            get
            {
                return JsonConvert.DeserializeObject<T>(RawBody);
            }
        }
    }


}
