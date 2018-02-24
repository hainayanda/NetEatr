using NetEatr.Digester;
using System;
using System.Threading.Tasks;

namespace NetEatr.Base
{
    /// <summary>
    /// Interface for HttpExecutor that handle execution in HttpRequest
    /// </summary>
    public interface IHttpExecutor
    {
        /// <summary>
        /// Method to get asynchronous execution in form of task
        /// </summary>
        /// <returns>Task which have return type of Response</returns>
        Task<Response> GetAsyncExecute();

        /// <summary>
        /// Method to get asynchronous execution in form of task
        /// </summary>
        /// <typeparam name="T">Type of object generated for Json Parsing</typeparam>
        /// <returns>Task which have return type of RestResponse of T</returns>
        Task<RestResponse<T>> GetAsyncExecute<T>();

        /// <summary>
        /// Method to execute HttpRequest asynchronously
        /// </summary>
        /// <param name="onFinished">Delegate to run when execution is finished</param>
        void AsyncExecute(Action<Response> onFinished);

        /// <summary>
        /// Method to execute HttpRequest asynchronously
        /// </summary>
        /// <typeparam name="T">Type of object generated for Json Parsing</typeparam>
        /// <param name="onFinished">Delegate to run when execution is finished</param>
        void AsyncExecute<T>(Action<RestResponse<T>> onFinished);

        /// <summary>
        /// Method to execute HttpRequest asynchronously
        /// </summary>
        void AsyncExecute();

        /// <summary>
        /// Method to execute HttpRequest synchronously
        /// </summary>
        /// <returns>Response object</returns>
        Response AwaitExecute();

        /// <summary>
        /// Method to execute HttpRequest synchronously
        /// </summary>
        /// <typeparam name="T">Type of object generated for Json Parsing</typeparam>
        /// <returns>RestResponse of T</returns>
        RestResponse<T> AwaitExecute<T>();
    }
}
