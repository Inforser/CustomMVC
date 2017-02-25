namespace CustomMVC.Core.ViewEngine
{
    using System;
    using CustomMVC.Core.Interfaces;

    public class ActionResult : IActionResult
    {
        public ActionResult(string viewFullQualifiedName)
        {
            this.Action = (IRenderable)Activator.CreateInstance(Type.GetType(viewFullQualifiedName));
        }

        public IRenderable Action { get; set; }

        public string Invoke()
        {
            return this.Action.Render();
        }
    }
}
