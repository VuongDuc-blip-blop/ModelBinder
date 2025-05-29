using System.Net;
using System.Text;

namespace ModelBinder.Http
{
    public class HttpServer
    {
        private HttpListener _listener;
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
                switch (request.HttpMethod)
                {
                    case "GET":
                        {

                            switch (request.Url.AbsolutePath)
                            {
                                case "/hello":
                                    {
                                        byte[] buffer = Encoding.UTF8.GetBytes("Hello World!");
                                        response.ContentLength64 = buffer.Length;
                                        response.ContentType = "text/plain";
                                        await outputStream.WriteAsync(buffer, 0, buffer.Length);
                                    }
                                    break;
                                case "/status":
                                    {
                                        byte[] buffer = Encoding.UTF8.GetBytes("Server is runnning.");
                                        response.ContentLength64 = buffer.Length;
                                        response.ContentType = "text/plain";
                                        await outputStream.WriteAsync(buffer, 0, buffer.Length);
                                    }
                                    break;
                                case "POST":
                                    {
                                        byte[] buffer = Encoding.UTF8.GetBytes("POST received");
                                        response.ContentLength64 = buffer.Length;
                                        response.ContentType = "text/plain";
                                        await outputStream.WriteAsync(buffer, 0, buffer.Length);

                                    }
                                    break;
                                default:
                                    {
                                        response.StatusCode = (int)HttpStatusCode.NotFound;
                                        byte[] buffer = Encoding.UTF8.GetBytes("NOT FOUND");
                                        response.ContentLength64 = buffer.Length;
                                        response.ContentType = "text/plain";
                                        await outputStream.WriteAsync(buffer, 0, buffer.Length);
                                    }
                                    break;

                            }
                            response.Close();

                        }
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                        response.Close();
                        break;
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
