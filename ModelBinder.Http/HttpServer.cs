using ModelBinder.Core;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelBinder.Http
{
    public class HttpServer
    {
        private HttpListener _listener;
        private readonly Dictionary<string, Func<RequestContext, HttpListenerResponse, Task>> _handler = new();
        private readonly Router _router;


        public void RegisterHandler(string path, Func<RequestContext, HttpListenerResponse, Task> handler)
        {
            _handler[path] = handler;
        }
        public HttpServer(string prefix)
        {
            if (!HttpListener.IsSupported)
            {
                throw new Exception("The system is not supported");
            }
            if (prefix == null || prefix.Length == 0)
            {
                throw new ArgumentException("prefix");
            }
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix);
            Console.CancelKeyPress += (s, e) =>
            {
                _listener.Stop();
                _listener.Close();
                Environment.Exit(0);
            };
            _router = new Router();

            RegisterHandler("/hello", _router.HelloPathHandler);
            RegisterHandler("/user", _router.UserPathHandler);
            RegisterHandler("/test", _router.TestPathHandler);
            RegisterHandler("/form", _router.FormPathHandler);
            RegisterHandler("Default", _router.InvalidPathHandler);

        }

        public async Task StartAsync()
        {
            try
            {
                _listener.Start();
                while (true)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}: Waiting a client to connect");
                    HttpListenerContext request = _listener.GetContext();
                    await ProcessRequestAsync(request);



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task ProcessRequestAsync(HttpListenerContext context)
        {
            try
            {
                RequestContext request = new(context.Request);
                var response = context.Response;

                var outputStream = response.OutputStream;


                string path = request.Path;

                if (path == "/hello")
                {
                    await _handler["/hello"].Invoke(request, response);
                }
                else if (Regex.IsMatch(path, @"^/user(?:/(\d+))?$"))
                {
                    await _handler["/user"].Invoke(request, response);
                }
                else if (path == "/test" && request.Query.Count > 0)
                {
                    await _handler["/test"].Invoke(request, response);
                }
                else if (path == "/form")
                {
                    await _handler["/form"].Invoke(request, response);
                }
                else
                {
                    await _handler["Default"].Invoke(request, response);
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                byte[] buffer = Encoding.UTF8.GetBytes(ex.Message);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.ContentType = "text/plain";
                using (var outputStream = context.Response.OutputStream)
                {
                    await outputStream.WriteAsync(buffer, 0, buffer.Length);
                }
                context.Response.Close();
            }


        }

    }


}
