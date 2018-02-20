using NetEatr.Digester;
using System;
using System.Threading.Tasks;

namespace NetEatr.Base
{
    public interface IHttpExecutor
    {
        Task<Response> GetAsyncExecute();
        Task<RestResponse<T>> GetAsyncExecute<T>();
        void AsyncExecute(Action<Response> onFinished);
        void AsyncExecute<T>(Action<RestResponse<T>> onFinished);
        void AsyncExecute();
        Response AwaitExecute();
        RestResponse<T> AwaitExecute<T>();
    }
}
