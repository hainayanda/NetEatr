
namespace NetEatr.Builder
{
    /// <summary>
    /// Static class which contains builder for HttpRequest
    /// </summary>
    public static class HttpRequestBuilder
    {
        /// <summary>
        /// Getter to get HttpGet object
        /// </summary>
        public static HttpRequest HttpGet
        {
            get
            {
                return new HttpRequest("GET");
            }
        }

        /// <summary>
        /// Getter to get HttpPost object
        /// </summary>
        public static HttpRequestWithBody HttpPost
        {
            get
            {
                return new HttpRequestWithBody("POST");
            }
        }

        /// <summary>
        /// Getter to get HttpPut object
        /// </summary>
        public static HttpRequestWithBody HttpPut
        {
            get
            {
                return new HttpRequestWithBody("PUT");
            }
        }
    }
}
