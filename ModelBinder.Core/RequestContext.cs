using System.Net;

namespace ModelBinder.Core
{
    public class RequestContext
    {
        public string HttpMethod { get; set; }
        public string Path { get; set; }
        public Dictionary<string, string> Query { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public string Body { get; set; }

        public Dictionary<string, string> RouteParameters { get; set; }

        public RequestContext(string method, string path, Dictionary<string, string> queryParams, Dictionary<string, string> headers, string body, Dictionary<string, string> routeParams)
        {
            HttpMethod = method;
            Path = path;
            Query = queryParams;
            Headers = headers;
            Body = body;
            RouteParameters = routeParams;
        }

        public RequestContext(HttpListenerRequest request)
        {
            HttpMethod = request.HttpMethod;
            Path = request.Url.AbsolutePath;
            Query = request.QueryString.AllKeys.ToDictionary(k => k, k => request.QueryString[k]);
            Headers = request.Headers.AllKeys.ToDictionary(k => k, k => request.Headers[k]);
            Body = request.InputStream.ToString();

        }
    }
}
