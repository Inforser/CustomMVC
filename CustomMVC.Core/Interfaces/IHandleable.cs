namespace CustomMVC.Core.Interfaces
{
    using CustomHttpServer.Models;

    public interface IHandleable
    {
        HttpResponse Handle(HttpRequest request);
    }
}
