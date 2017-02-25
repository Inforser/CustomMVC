namespace CustomMVC.App
{
    using CustomHttpServer;
    using CustomMVC.Core;

    public class StartUp
    {
        public static void Main()
        {
            HttpServer server = new HttpServer(8081, RouteTable.Routes);
            MvcEngine.Run(server);
        }
    }
}
