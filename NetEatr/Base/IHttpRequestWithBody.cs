using System.Collections.Generic;
using System.Xml;

namespace NetEatr.Base
{
    /// <summary>
    /// Interface of HttpRequest which have a body
    /// </summary>
    /// <typeparam name="T">Type of object that extends this interface</typeparam>
    public interface IHttpRequestWithBody<T> : IHttpRequest<T> where T : IHttpExecutor
    {
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        string Body { get; set; }

        /// <summary>
        /// Method to add string body
        /// </summary>
        /// <param name="body"></param>
        /// <returns>itself</returns>
        T AddBody(string body);

        /// <summary>
        /// Method to add form url as body
        /// it will be encoded to url form
        /// itl will set the content into urlformencoded
        /// </summary>
        /// <param name="forms"></param>
        /// <returns>itself</returns>
        T AddFormUrlEncoded(IDictionary<string, string> forms);

        /// <summary>
        /// Method to add object as body
        /// it will be parsed to Json
        /// it will set the content into application/json
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj"></param>
        /// <returns>itself</returns>
        T AddJsonBody<V>(V obj);

        /// <summary>
        /// /// Method to add xml as body
        /// itl will set the content into xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>itself</returns>
        T AddXmlBody(XmlDocument xml);
    }
}