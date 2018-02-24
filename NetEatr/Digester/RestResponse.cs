using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NetEatr.Digester
{
    /// <summary>
    /// object that contains json
    /// </summary>
    /// <typeparam name="T">Type of Json container</typeparam>
    public class RestResponse<T> : Response
    {
        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="webResponse"></param>
        public RestResponse(HttpWebResponse webResponse) : base(webResponse) { }

        /// <summary>
        /// Secondary constructor when there is an exception
        /// </summary>
        /// <param name="webResponse"></param>
        /// <param name="exception"></param>
        public RestResponse(HttpWebResponse webResponse, Exception exception) : base(webResponse, exception) { }

        private T _JsonBody = default(T);

        /// <summary>
        /// Parsed string of Json in object of T
        /// </summary>
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
