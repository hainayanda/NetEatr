
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetEatr.Digester;
using NetEatr.Builder;
using NetEatrTest.Model;
using System.Threading;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NetEatrTest
{
    [TestClass]
    public class HttpGetTest
    {
        private const string URL = "http://staging-webservice.catch.co.id/user/get";
        private const string HEADER_KEY = "API-KEY";
        private const string HEADER_VALUE = "default";
        private const string PARAM_KEY = "username";
        private const string PARAM_VALUE = "kryokrait";
        private const int TIMEOUT = 20000;

        [TestMethod]
        public void AsyncExecuteTest()
        {
            var countDown = new CountdownEvent(1);
            RestResponse<User> reservedResponse = null;
            var isRunBeforeSending = false;
            var isTimeout = false;
            var isException = false;
            var progressCount = 0;
            var finalProgress = 0f;

            HttpRequestBuilder.HttpGet.SetUrl(URL).AddHeaders(HEADER_KEY, HEADER_VALUE)
                .AddParam(PARAM_KEY, PARAM_VALUE)
                .SetOnProgress((progress) =>
                {
                    progressCount++;
                    finalProgress = progress;
                })
                .SetOnBeforeSending((_) =>
                {
                    isRunBeforeSending = true;
                })
                .SetOnException((_) =>
                {
                    isException = true;
                })
                .SetOnTimeout(() =>
                {
                    isTimeout = true;
                })
                .SetOnResponded<User>((response) =>
                {
                    reservedResponse = response;
                    countDown.Signal();
                })
                .AsyncExecute();
            countDown.Wait(TIMEOUT);
            IsTrue(isRunBeforeSending, "is not run before sending");
            IsTrue(progressCount >= 5, "progress count is not really incremented");
            IsTrue(Math.Abs(finalProgress - 1f) <= 0.01f, "progress is not finished");
            IsFalse(isTimeout);
            IsFalse(isException);
            IsNotNull(reservedResponse);
            IsFalse(reservedResponse.HadException);
            IsTrue(reservedResponse.IsSuccess, "request failed");
            IsTrue(reservedResponse.JsonBody.UserName == PARAM_VALUE, "userName is " + reservedResponse.JsonBody.UserName);
        }

        [TestMethod]
        public void AsyncExecuteTest1()
        {
            var countDown = new CountdownEvent(1);
            RestResponse<User> reservedResponse = null;
            RestResponse<User> onFinishedResponse = null;
            var isRunBeforeSending = false;
            var isTimeout = false;
            var isException = false;
            var progressCount = 0;
            var finalProgress = 0f;
            HttpRequestBuilder.HttpGet.SetUrl(URL).AddHeaders(HEADER_KEY, HEADER_VALUE)
                .AddParam(PARAM_KEY, PARAM_VALUE)
                .SetOnProgress((progress) =>
                {
                    progressCount++;
                    finalProgress = progress;
                })
                .SetOnBeforeSending((_) =>
                {
                    isRunBeforeSending = true;
                })
                .SetOnException((_) =>
                {
                    isException = true;
                })
                .SetOnTimeout(() =>
                {
                    isTimeout = true;
                })
                .SetOnResponded<User>((response) =>
                {
                    reservedResponse = response;
                })
                .AsyncExecute<User>((response) =>
                {
                    onFinishedResponse = response;
                    countDown.Signal();
                });
            countDown.Wait(TIMEOUT);
            IsTrue(isRunBeforeSending, "is not run before sending");
            IsTrue(progressCount >= 5, "progress count is not really incremented");
            IsTrue(Math.Abs(finalProgress - 1f) <= 0.01f, "progress is not finished");
            IsFalse(isTimeout);
            IsFalse(isException);
            IsNotNull(reservedResponse);
            IsTrue(onFinishedResponse == reservedResponse, "response is different");
            IsFalse(reservedResponse.HadException);
            IsTrue(reservedResponse.IsSuccess, "request failed");
            IsTrue(reservedResponse.JsonBody.UserName == PARAM_VALUE);
        }

        [TestMethod]
        public void AsyncExecuteTest2()
        {
            var countDown = new CountdownEvent(1);
            Response reservedResponse = null;
            Response onFinishedResponse = null;
            var isRunBeforeSending = false;
            var isTimeout = false;
            var isException = false;
            var progressCount = 0;
            var finalProgress = 0f;
            HttpRequestBuilder.HttpGet.SetUrl(URL).AddHeaders(HEADER_KEY, HEADER_VALUE)
                .AddParam(PARAM_KEY, PARAM_VALUE)
                .SetOnProgress((progress) =>
                {
                    progressCount++;
                    finalProgress = progress;
                })
                .SetOnBeforeSending((_) =>
                {
                    isRunBeforeSending = true;
                })
                .SetOnException((_) =>
                {
                    isException = true;
                })
                .SetOnTimeout(() =>
                {
                    isTimeout = true;
                })
                .SetOnResponded((response) =>
                {
                    reservedResponse = response;
                })
                .AsyncExecute((response) =>
                {
                    onFinishedResponse = response;
                    countDown.Signal();
                });
            countDown.Wait(TIMEOUT);
            IsTrue(isRunBeforeSending, "is not run before sending");
            IsTrue(progressCount >= 5, "progress count is not really incremented");
            IsTrue(Math.Abs(finalProgress - 1f) <= 0.01f, "progress is not finished");
            IsFalse(isTimeout);
            IsFalse(isException);
            IsNotNull(reservedResponse);
            IsTrue(onFinishedResponse == reservedResponse, "response is different");
            IsFalse(reservedResponse.HadException);
            IsTrue(reservedResponse.IsSuccess, "request failed");
        }

        [TestMethod]
        public void AwaitExecuteTest()
        {
            RestResponse<User> reservedResponse = null;
            var isRunBeforeSending = false;
            var isTimeout = false;
            var isException = false;
            var progressCount = 0;
            var finalProgress = 0f;
            var onFinishedResponse = HttpRequestBuilder.HttpGet.SetUrl(URL).AddHeaders(HEADER_KEY, HEADER_VALUE)
                .AddParam(PARAM_KEY, PARAM_VALUE)
                .SetOnProgress((progress) =>
                {
                    progressCount++;
                    finalProgress = progress;
                })
                .SetOnBeforeSending((_) =>
                {
                    isRunBeforeSending = true;
                })
                .SetOnException((_) =>
                {
                    isException = true;
                })
                .SetOnTimeout(() =>
                {
                    isTimeout = true;
                })
                .SetOnResponded<User>((response) =>
                {
                    reservedResponse = response;
                })
                .AwaitExecute<User>();
            IsTrue(isRunBeforeSending);
            IsTrue(progressCount >= 5);
            IsTrue(Math.Abs(finalProgress - 1f) <= 0.01f);
            IsFalse(isTimeout);
            IsFalse(isException);
            IsNotNull(reservedResponse);
            IsTrue(onFinishedResponse == reservedResponse);
            IsFalse(reservedResponse.HadException);
            IsTrue(reservedResponse.IsSuccess);
            IsTrue(reservedResponse.JsonBody.UserName == PARAM_VALUE);
        }

        [TestMethod]
        public void AwaitExecuteTest1()
        {
            Response reservedResponse = null;
            var isRunBeforeSending = false;
            var isTimeout = false;
            var isException = false;
            var progressCount = 0;
            var finalProgress = 0f;
            var onFinishedResponse = HttpRequestBuilder.HttpGet.SetUrl(URL).AddHeaders(HEADER_KEY, HEADER_VALUE)
                .AddParam(PARAM_KEY, PARAM_VALUE)
                .SetOnProgress((progress) =>
                {
                    progressCount++;
                    finalProgress = progress;
                })
                .SetOnBeforeSending((_) =>
                {
                    isRunBeforeSending = true;
                })
                .SetOnException((_) =>
                {
                    isException = true;
                })
                .SetOnTimeout(() =>
                {
                    isTimeout = true;
                })
                .SetOnResponded((response) =>
                {
                    reservedResponse = response;
                })
                .AwaitExecute();
            IsTrue(isRunBeforeSending);
            IsTrue(progressCount >= 5);
            IsTrue(Math.Abs(finalProgress - 1f) <= 0.01f);
            IsFalse(isTimeout);
            IsFalse(isException);
            IsNotNull(reservedResponse);
            IsTrue(onFinishedResponse == reservedResponse);
            IsFalse(reservedResponse.HadException);
            IsTrue(reservedResponse.IsSuccess);
        }

        [TestMethod]
        public void GetAsyncExecuteTest()
        {
            Response reservedResponse = null;
            var isRunBeforeSending = false;
            var isTimeout = false;
            var isException = false;
            var progressCount = 0;
            var finalProgress = 0f;
            var task = HttpRequestBuilder.HttpGet.SetUrl(URL).AddHeaders(HEADER_KEY, HEADER_VALUE)
                .AddParam(PARAM_KEY, PARAM_VALUE)
                .SetOnProgress((progress) =>
                {
                    progressCount++;
                    finalProgress = progress;
                })
                .SetOnBeforeSending((_) =>
                {
                    isRunBeforeSending = true;
                })
                .SetOnException((_) =>
                {
                    isException = true;
                })
                .SetOnTimeout(() =>
                {
                    isTimeout = true;
                })
                .SetOnResponded((response) =>
                {
                    reservedResponse = response;
                })
                .GetAsyncExecute();
            var onFinishedResponse = task.Result;
            IsTrue(isRunBeforeSending);
            IsTrue(progressCount >= 5);
            IsTrue(Math.Abs(finalProgress - 1f) <= 0.01f);
            IsFalse(isTimeout);
            IsFalse(isException);
            IsNotNull(reservedResponse);
            IsTrue(onFinishedResponse == reservedResponse);
            IsFalse(reservedResponse.HadException);
            IsTrue(reservedResponse.IsSuccess);
        }

        [TestMethod]
        public void GetAsyncExecuteTest1()
        {
            RestResponse<User> reservedResponse = null;
            var isRunBeforeSending = false;
            var isTimeout = false;
            var isException = false;
            var progressCount = 0;
            var finalProgress = 0f;
            var task = HttpRequestBuilder.HttpGet.SetUrl(URL).AddHeaders(HEADER_KEY, HEADER_VALUE)
                .AddParam(PARAM_KEY, PARAM_VALUE)
                .SetOnProgress((progress) =>
                {
                    progressCount++;
                    finalProgress = progress;
                })
                .SetOnBeforeSending((_) =>
                {
                    isRunBeforeSending = true;
                })
                .SetOnException((_) =>
                {
                    isException = true;
                })
                .SetOnTimeout(() =>
                {
                    isTimeout = true;
                })
                .SetOnResponded<User>((response) =>
                {
                    reservedResponse = response;
                })
                .GetAsyncExecute<User>();
            var onFinishedResponse = task.Result;
            IsTrue(isRunBeforeSending);
            IsTrue(progressCount >= 5);
            IsTrue(Math.Abs(finalProgress - 1f) <= 0.01f);
            IsFalse(isTimeout);
            IsFalse(isException);
            IsNotNull(reservedResponse);
            IsTrue(onFinishedResponse == reservedResponse);
            IsFalse(reservedResponse.HadException);
            IsTrue(reservedResponse.IsSuccess);
        }

    }
}
