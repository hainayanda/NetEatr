using NetEatr.Digester;
using System;
using System.Collections.Generic;
using System.Net;

namespace NetEatr.Base
{
    /// <summary>
    /// Interface of HttpRequest
    /// </summary>
    /// <typeparam name="T">Type of object that extends this interface</typeparam>
    public interface IHttpRequest<T> : IHttpExecutor where T : IHttpExecutor
    {
        /// <summary>
        /// Method to set the url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>itself</returns>
        T SetUrl(string url);

        /// <summary>
        /// Method to set url parameter
        /// it will be automatically encoded the parameter
        /// </summary>
        /// <param name="parameters">dictionary of key - value</param>
        /// <returns>itself</returns>
        T SetParams(Dictionary<string, string> parameters);

        /// <summary>
        /// Method to add parameter
        /// it will be automatically encoded the parameter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>itself</returns>
        T AddParam(string key, string value);

        /// <summary>
        /// Method to set headers
        /// </summary>
        /// <param name="headers">dicitionary of header in form of key - value</param>
        /// <returns>itself</returns>
        T SetHeaders(Dictionary<string, string> headers);

        /// <summary>
        /// Method to add headers
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>itself</returns>
        T AddHeaders(string key, string value);

        /// <summary>
        /// Method to add authorization
        /// it will automatically using a bearer oAuth authorization
        /// </summary>
        /// <param name="token"></param>
        /// <returns>itself</returns>
        T AddAuthorization(string token);

        /// <summary>
        /// Method to add timeout
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns>itself</returns>
        T SetTimeout(int timeout);

        /// <summary>
        /// Method to set delegate to run every progress
        /// progress start with 0 and end in 1
        /// </summary>
        /// <param name="onProgress"></param>
        /// <returns>itself</returns>
        T SetOnProgress(Action<float> onProgress);

        /// <summary>
        /// Method to set delegate to run right before sending
        /// </summary>
        /// <param name="onBeforeSending"></param>
        /// <returns>itself</returns>
        T SetOnBeforeSending(Action<HttpWebRequest> onBeforeSending);

        /// <summary>
        /// Method to set the delegate to run right after get response
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="onResponded"></param>
        /// <returns>itself</returns>
        T SetOnResponded<V>(Action<RestResponse<V>> onResponded);

        /// <summary>
        /// Method to set the delegate to run right after get response
        /// </summary>
        /// <param name="onResponded"></param>
        /// <returns>itself</returns>
        T SetOnResponded(Action<Response> onResponded);

        /// <summary>
        /// Method to set the delegate to run rigth after exception
        /// </summary>
        /// <param name="onException"></param>
        /// <returns>itself</returns>
        T SetOnException(Action<Exception> onException);

        /// <summary>
        /// Method to set the delegate to run right after timeout
        /// </summary>
        /// <param name="onTimeout"></param>
        /// <returns>itself</returns>
        T SetOnTimeout(Action onTimeout);
    }
}
