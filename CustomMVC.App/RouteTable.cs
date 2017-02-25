namespace CustomMVC.App
{
    using System.Collections.Generic;
    using CustomHttpServer.Enums;
    using CustomHttpServer.Models;
    using CustomMVC.Core.Routers;

    public class RouteTable
    {
        public static IEnumerable<Route> Routes => new[]
        {
            new Route()
            {
                Name = "Controller/Action/GET",
                Method = RequestMethod.GET,
                UrlRegex = @"^/(.+)/(.+)",
                Callable = new ControllerRouter().Handle
            },
            new Route()
            {
                Name = "Controller/Action/POST",
                Method = RequestMethod.POST,
                UrlRegex = @"^/(.+)/(.+)",
                Callable = new ControllerRouter().Handle
            }
        };
    }
}
