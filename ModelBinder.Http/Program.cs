namespace ModelBinder.Http
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            HttpServer myServer = new HttpServer("http://localhost:8081/");
            await myServer.StartAsync();
        }
    }
}
