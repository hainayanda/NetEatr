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
        public static HttpRequest HttpGet => new HttpGet();

        /// <summary>
        /// Getter to get HttpPost object
        /// </summary>
        public static HttpRequestWithBody HttpPost => new HttpPost();

        /// <summary>
        /// Getter to get HttpPut object
        /// </summary>
        public static HttpRequestWithBody HttpPut => new HttpPut();
    }

    /// <summary>
    /// HttpGet class
    /// </summary>
    /// <seealso cref="NetEatr.Builder.HttpRequest" />
    public class HttpGet : HttpRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGet"/> class.
        /// </summary>
        public HttpGet() : base("GET") { }
    }

    /// <summary>
    /// HttpGet class
    /// </summary>
    /// <seealso cref="NetEatr.Builder.HttpRequestWithBody" />
    public class HttpPost : HttpRequestWithBody
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPost"/> class.
        /// </summary>
        public HttpPost() : base("POST") { }
    }

    /// <summary>
    /// HttpPut class
    /// </summary>
    /// <seealso cref="NetEatr.Builder.HttpRequestWithBody" />
    public class HttpPut : HttpRequestWithBody
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPut"/> class.
        /// </summary>
        public HttpPut() : base("POST") { }
    }
}