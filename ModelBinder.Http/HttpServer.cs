using ModelBinder.Core;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelBinder.Http
{
    public class HttpServer
    {
        private HttpListener _listener;
        private readonly Dictionary<string, Func<HttpListenerContext, Task>> _handler = new();
        private readonly Router _router;
        private readonly RequestContext _requestContext;

        public void RegisterHandler(string path, Func<HttpListenerContext, Task> handler)
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
                var request = context.Request;
                var response = context.Response;

                var outputStream = response.OutputStream;


                string path = request.Url.AbsolutePath;

                if (path == "/hello")
                {
                    await _handler["/hello"].Invoke(context);
                }
                else if (Regex.IsMatch(path, @"^/user(?:/(\d+))?$"))
                {
                    await _handler["/user"].Invoke(context);
                }
                else
                {
                    await _handler["Default"].Invoke(context);
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
