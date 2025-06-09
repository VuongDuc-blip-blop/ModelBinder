using ModelBinder.Core;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelBinder.Http
{
    public class Router
    {
        public async Task HelloPathHandler(RequestContext request, HttpListenerResponse response)
        {

            Console.WriteLine($"Method: {request.HttpMethod}");
            Console.WriteLine($"Path: {request.Path}");

            Console.WriteLine("Query Parameters:");
            foreach (var kvp in request.Query)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("Headers:");
            foreach (var kvp in request.Headers)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine($"Body: {request.Body}");

            Console.WriteLine("Route Parameters:");
            foreach (var kvp in request.RouteParameters)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }


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

        public async Task UserPathHandler(RequestContext request, HttpListenerResponse response)
        {
            Console.WriteLine($"Method: {request.HttpMethod}");
            Console.WriteLine($"Path: {request.Path}");

            Console.WriteLine("Query Parameters:");
            foreach (var kvp in request.Query)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("Headers:");
            foreach (var kvp in request.Headers)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine($"Body: {request.Body}");

            Console.WriteLine("Route Parameters:");
            foreach (var kvp in request.RouteParameters)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            bool haveId = false;
            var outputStream = response.OutputStream;



            var match = Regex.Match(request.Path, @"^/user/(\d+)$");
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

        public async Task InvalidPathHandler(RequestContext request, HttpListenerResponse response)
        {
            Console.WriteLine($"Method: {request.HttpMethod}");
            Console.WriteLine($"Path: {request.Path}");

            Console.WriteLine("Query Parameters:");
            foreach (var kvp in request.Query)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("Headers:");
            foreach (var kvp in request.Headers)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine($"Body: {request.Body}");

            Console.WriteLine("Route Parameters:");
            foreach (var kvp in request.RouteParameters)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            var outputStream = response.OutputStream;

            byte[] buffer = Encoding.UTF8.GetBytes("Not Found");
            response.ContentLength64 = buffer.Length;
            response.ContentType = "text/plain";
            response.StatusCode = (int)HttpStatusCode.NotFound;

            await outputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }

        public async Task TestPathHandler(RequestContext request, HttpListenerResponse response)
        {
            Console.WriteLine($"Method: {request.HttpMethod}");
            Console.WriteLine($"Path: {request.Path}");

            Console.WriteLine("Query Parameters:");
            foreach (var kvp in request.Query)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("Headers:");
            foreach (var kvp in request.Headers)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine($"Body: {request.Body}");

            Console.WriteLine("Route Parameters:");
            foreach (var kvp in request.RouteParameters)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            var outputStream = response.OutputStream;

            foreach (var kvp in request.RouteParameters)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }


            byte[] buffer = Encoding.UTF8.GetBytes("Test");
            response.ContentLength64 = buffer.Length;
            response.ContentType = "text/plain";
            response.StatusCode = (int)HttpStatusCode.NotFound;

            await outputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }

        public async Task FormPathHandler(RequestContext request, HttpListenerResponse response)
        {
            Console.WriteLine($"Method: {request.HttpMethod}");
            Console.WriteLine($"Path: {request.Path}");

            Console.WriteLine("Query Parameters:");
            foreach (var kvp in request.Query)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("Headers:");
            foreach (var kvp in request.Headers)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine($"Body: {request.Body}");

            Console.WriteLine("Form Data:");
            foreach (var kvp in request.FormData)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("Route Parameters:");
            foreach (var kvp in request.RouteParameters)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            var outputStream = response.OutputStream;

            foreach (var kvp in request.RouteParameters)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }


            byte[] buffer = Encoding.UTF8.GetBytes("Form Received");
            response.ContentLength64 = buffer.Length;
            response.ContentType = "text/plain";
            response.StatusCode = (int)HttpStatusCode.NotFound;

            await outputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }
    }
}
