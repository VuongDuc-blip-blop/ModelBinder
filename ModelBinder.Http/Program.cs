using System.Net;
using System.Text;

namespace ModelBinder.Http
{
    public class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8081/");

            listener.Start();
            Console.WriteLine("Listening on http://localhost:8081/");

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                if (request.HttpMethod == "GET")
                {
                    string responseString = "Hello,World";
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                    HttpListenerResponse response = context.Response;

                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "text/plain";

                    using (Stream output = response.OutputStream)
                    {
                        output.Write(buffer, 0, buffer.Length);
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    context.Response.Close();
                }
            }
        }
    }
}
