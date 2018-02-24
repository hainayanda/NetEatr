using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NetEatr.Digester
{
    public class RestResponse<T> : Response
    {
        public RestResponse(HttpWebResponse webResponse) : base(webResponse) { }

        public RestResponse(HttpWebResponse webResponse, Exception exception) : base(webResponse, exception) { }

        private T _JsonBody = default(T);
        public T JsonBody
        {
            get
            {
                if (_JsonBody == null)
                {
                    try
                    {
                        _JsonBody = JsonConvert.DeserializeObject<T>(RawBody);
                        if (_JsonBody == null) _JsonBody = JsonBodyUsingContractResolver();
                    }
                    catch
                    {
                        _JsonBody = JsonBodyUsingContractResolver();
                    }
                }
                return _JsonBody;
            }
        }

        private T JsonBodyUsingContractResolver()
        {
            return JsonConvert.DeserializeObject<T>(RawBody,
                            new JsonSerializerSettings
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            });
        }
    }
}
