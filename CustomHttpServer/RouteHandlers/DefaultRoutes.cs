namespace CustomHttpServer.RouteHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomHttpServer.Enums;
    using CustomHttpServer.Models;

    public static class DefaultRoutes
    {
        private static IEnumerable<Route> DefRoutes => new List<Route>()
        {
            new Route()
                {
                    Name = "Hello page",
                    UrlRegex = @"^/hello$",
                    Method = RequestMethod.GET,
                    Callable = (HttpRequest request) => new HttpResponse()
                    {
                        ContentAsUtf8 = "<h3>Hello from HttpServer :)</h3>",
                        StatusCode = ResponseStatusCode.OK
                    }
                },
                new Route()
                {
                    Name = "FileSystem Static Handler",
                    UrlRegex = @"^/(.*)$",
                    Method = RequestMethod.GET,
                    Callable =
                        new FileSystemRouteHandler() {RouteUrlRegex = @"^/(.*)$", BasePath = @"E:\Repos"}
                            .Handle,
                }
        };

        public static IEnumerable<Route> AppendDefaultRoutes(IEnumerable<Route> inputRoutes) => inputRoutes.Concat(DefRoutes);
    }
}
