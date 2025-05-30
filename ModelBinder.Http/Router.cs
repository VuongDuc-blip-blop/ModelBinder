using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelBinder.Http
{
    public class Router
    {
        public async Task HelloPathHandler(HttpListenerContext context)
        {

            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;


            var outputStream = response.OutputStream;
            if (request.HttpMethod == "GET")
            {

                byte[] buffer = Encoding.UTF8.GetBytes("Hello, World!");
                response.ContentLength64 = buffer.Length;
                response.ContentType = "text/plain";
                response.StatusCode = (int)HttpStatusCode.OK;

                await outputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            else
            {
                byte[] buffer = Encoding.UTF8.GetBytes("Method Not Allowed");
                response.ContentLength64 = buffer.Length;
                response.ContentType = "text/plain";
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                await outputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            response.Close();
        }

        public async Task UserPathHandler(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            bool haveId = false;
            var outputStream = response.OutputStream;



            var match = Regex.Match(request.Url.AbsolutePath, @"^/user/(\d+)$");
            if (match.Success)
            {
                int.TryParse(match.Groups[1].Value, out int userId);
                if (request.HttpMethod == "GET")
                {
                    byte[] buffer = Encoding.UTF8.GetBytes($"User ID: {userId}");
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "text/plain";
                    response.StatusCode = (int)HttpStatusCode.OK;

                    await outputStream.WriteAsync(buffer, 0, buffer.Length);
                }
                else
                {
                    byte[] buffer = Encoding.UTF8.GetBytes("Method Not Allowed");
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "text/plain";
                    response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    await outputStream.WriteAsync(buffer, 0, buffer.Length);
                }

            }
            else
            {
                if (request.HttpMethod == "POST")
                {
                    byte[] buffer = Encoding.UTF8.GetBytes("User created");
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "text/plain";
                    response.StatusCode = (int)HttpStatusCode.OK;
                    await outputStream.WriteAsync(buffer, 0, buffer.Length);
                }
                else
                {
                    byte[] buffer = Encoding.UTF8.GetBytes("Method Not Allowed");
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "text/plain";
                    response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    await outputStream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            response.Close();
        }

        public async Task InvalidPathHandler(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            var outputStream = response.OutputStream;

            byte[] buffer = Encoding.UTF8.GetBytes("Not Found");
            response.ContentLength64 = buffer.Length;
            response.ContentType = "text/plain";
            response.StatusCode = (int)HttpStatusCode.NotFound;

            await outputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }
    }
}
