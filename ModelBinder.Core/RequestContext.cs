using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

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

        public Dictionary<string, string> FormData { get; set; }


        public RequestContext(string method, string path, Dictionary<string, string> queryParams, Dictionary<string, string> headers, string body, Dictionary<string, string> routeParams)
        {
            HttpMethod = method;
            Path = path;
            Query = queryParams ?? new Dictionary<string, string>();
            Headers = headers ?? new Dictionary<string, string>();
            Body = body;
            RouteParameters = routeParams ?? new Dictionary<string, string>();
        }

        public RequestContext(HttpListenerRequest request)
        {
            HttpMethod = request.HttpMethod;
            Path = request.Url.AbsolutePath;
            Query = ParseQueryString(request.Url.ToString());
            Headers = request.Headers.AllKeys.ToDictionary(k => k, k => request.Headers[k]) ?? new Dictionary<string, string>();
            Body = string.Empty;


            if (request.HasEntityBody && request.ContentLength64 > 0)
            {
                byte[] buffer = new byte[request.ContentLength64];
                int bytesRead = 0;

                while (bytesRead < request.ContentLength64)
                {
                    int read = request.InputStream.Read(buffer, bytesRead, (int)request.ContentLength64 - bytesRead);
                    if (read == 0) break;
                    bytesRead += read;
                }

                Encoding encoding = request.ContentEncoding ?? Encoding.UTF8;
                Body = encoding.GetString(buffer, 0, bytesRead);
            }

            if (request.HasEntityBody && request.ContentType?.Contains("application/x-www-form-urlencoded") == true)
            {
                FormData = ParseFormData(Body);
            }

            RouteParameters = new Dictionary<string, string>();
            MatchCollection matches = Regex.Matches(request.Url.AbsolutePath, @"\/([^\/=]+)=([^\/]+)");
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    string key = match.Groups[1].Value;
                    string value = match.Groups[2].Value;
                    RouteParameters[key] = value;
                }
            }


        }
        public Dictionary<string, string> ParseQueryString(string url)
        {
            Dictionary<string, string> queryPairs = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(url))
                return queryPairs;
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                if (!Uri.TryCreate("http://localhost:8081" + url, UriKind.Absolute, out uri))
                    return queryPairs;
            }

            string query = uri.Query;
            if (query.StartsWith("?"))
            {
                query = query.Substring(1);
            }
            var pairs = query.Split('&', StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var kv = pair.Split('=', 2);
                string key = HttpUtility.UrlDecode(kv[0]);
                string value = kv.Length > 1 ? HttpUtility.UrlDecode(kv[1]) : string.Empty;
                if (!string.IsNullOrEmpty(key))
                    queryPairs[key] = value;
            }


            return queryPairs;
        }

        public Dictionary<string, string> ParseFormData(string formContent)
        {
            Dictionary<string, string> formPairs = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(formContent))
                return formPairs;

            var submittedPairs = formContent.Split('&', StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in submittedPairs)
            {
                var kv = pair.Split('=', 2);
                string key = kv[0];
                string value = kv.Length > 1 ? HttpUtility.UrlDecode(kv[1], Encoding.UTF8) : string.Empty;
                if (!string.IsNullOrEmpty(key))
                    formPairs[key] = value;
            }
            return formPairs;
        }


    }

}
