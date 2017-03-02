namespace CustomMVC.Core.ViewEngine
{
    using System;
    using System.Reflection;
    using CustomMVC.Core.Interfaces;

    public class ActionResult : IActionResult
    {
        public ActionResult(string viewFullQualifiedName)
        {
            this.Action = (IRenderable)Activator.CreateInstance(MvcContext.Current.EntryAssembly.GetType(viewFullQualifiedName));
        }

        public IRenderable Action { get; set; }

        public string Invoke()
        {
            return this.Action.Render();
        }
    }
}
