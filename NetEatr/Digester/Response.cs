using System;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Serialization;

namespace NetEatr.Digester
{
    public class Response
    {
        public HttpWebResponse RawResponse { get; protected set; }

        public string RawBody { get; protected set; }

        private XmlDocument ParsedXml;
        public XmlDocument BodyAsXmlDoc
        {
            get
            {
                if (ParsedXml == null)
                {
                    var xml = new XmlDocument();
                    xml.LoadXml(RawBody);
                    return xml;
                }
                else return ParsedXml;
            }
        }

        public bool IsXml
        {
            get
            {
                if (ParsedXml != null)
                {
                    return true;
                }
                else if (!string.IsNullOrEmpty(RawBody) && RawBody.TrimStart().StartsWith("<") && RawBody.TrimEnd().EndsWith(">"))
                {
                    try
                    {
                        var xml = new XmlDocument();
                        xml.LoadXml(RawBody);
                        ParsedXml = xml;
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else return false;
            }
        }

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
            if (webResponse != null)
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
}
