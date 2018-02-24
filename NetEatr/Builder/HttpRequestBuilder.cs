
namespace NetEatr.Builder
{
    public static class HttpRequestBuilder
    {
        public static HttpRequest HttpGet
        {
            get
            {
                return new HttpRequest("GET");
            }
        }

        public static HttpRequestWithBody HttpPost
        {
            get
            {
                return new HttpRequestWithBody("POST");
            }
        }

        public static HttpRequestWithBody HttpPut
        {
            get
            {
                return new HttpRequestWithBody("PUT");
            }
        }
    }
}
