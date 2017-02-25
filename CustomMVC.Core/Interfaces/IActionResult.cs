namespace CustomMVC.Core.Interfaces
{
    public interface IActionResult : IInvocable
    {
        IRenderable Action { get; set; }
    }
}
