using System;
using System.IO;
using System.Net;
using System.Xml;

namespace NetEatr.Digester
{
    /// <summary>
    /// Object contains response from HttpRequest
    /// </summary>
    public class Response
    {
        private XmlDocument ParsedXml;

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="webResponse"></param>
        public Response(HttpWebResponse webResponse)
        {
            if (webResponse != null)
            {
                RawResponse = webResponse;
                StatusCode = webResponse.StatusCode.GetHashCode();
                RawBody = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
            }
        }

        /// <summary>
        /// Constructor when there is an exception
        /// </summary>
        /// <param name="webResponse"></param>
        /// <param name="exception"></param>
        public Response(HttpWebResponse webResponse, Exception exception) : this(webResponse)
        {
            Exception = exception;
        }

        /// <summary>
        /// Body in form of XmlDocument
        /// it will be throw exception if the body is not xml
        /// </summary>
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

        /// <summary>
        /// will contains null if there is no exception
        /// </summary>
        public Exception Exception { get; protected set; }

        /// <summary>
        /// true if response have an exception
        /// </summary>
        public bool HadException
        {
            get
            {
                return Exception != null;
            }
        }

        /// <summary>
        /// will return true if success
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return StatusCode >= 200 && StatusCode <= 299;
            }
        }

        /// <summary>
        /// will return true if the body is xml
        /// </summary>
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

        /// <summary>
        /// Body in form of string
        /// </summary>
        public string RawBody { get; protected set; }

        /// <summary>
        /// Raw response
        /// </summary>
        public HttpWebResponse RawResponse { get; protected set; }

        /// <summary>
        /// status code of Http response
        /// </summary>
        public int StatusCode { get; protected set; }
    }
}