namespace CustomMVC.Core
{
    public class MvcContext
    {
        public static readonly MvcContext Current = new MvcContext();

        public string AssemblyName { get; set; }

        public string ControllersFolder { get; set; }

        public string ContollersSuffix { get; set; }

        public string ViewsFolder { get; set; }

        public string MoldelsFolder { get; set; }
    }
}
