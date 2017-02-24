namespace CustomHttpServer
{
    using System.IO;
    using CustomHttpServer.Enums;
    using CustomHttpServer.Models;

    public static class HttpResponseBuilder
    {
        public static HttpResponse InternalServerError()
        {
            string content = File.ReadAllText("E:\\Repos\\CustomHttpServer\\CustomHttpServer\\Resources\\Pages\\500.html"); //TODO: Check link!

            return new HttpResponse()
            {
                StatusCode = ResponseStatusCode.InternalServerError,
                ContentAsUtf8 = content
            };
        }

        public static HttpResponse NotFound()
        {
            string content = File.ReadAllText("E:\\Repos\\CustomHttpServer\\CustomHttpServer\\Resources\\Pages\\404.html");

            return new HttpResponse()
            {
                StatusCode = ResponseStatusCode.NotFound,
                ContentAsUtf8 = content
            };
        }
    }
}
