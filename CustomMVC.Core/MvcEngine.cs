namespace CustomMVC.Core
{
    using System;
    using System.Reflection;
    using CustomHttpServer;

    public static class MvcEngine
    {
        public static void Run(HttpServer server)
        {
            RegisterAssembly();
            RegisterAssemblyName();
            RegisterControllers();
            RegisteViews();
            RegisterModels();
           
            try
            {
                server.Listen();
            }
            catch (Exception ex)
            {
                // TODO: Log erros in a file
                Console.WriteLine(ex.Message);
            }
        }

        private static void RegisterAssembly()
        {
            MvcContext.Current.EntryAssembly = Assembly.GetEntryAssembly();
        }

        private static void RegisterAssemblyName()
        {
            MvcContext.Current.AssemblyName = Assembly.GetEntryAssembly().GetName().Name;
        }

        private static void RegisterControllers()
        {
            MvcContext.Current.ControllersFolder = "Controllers";
            MvcContext.Current.ContollersSuffix = "Controller";
        }

        private static void RegisteViews()
        {
            MvcContext.Current.ViewsFolder = "Views";
        }

        private static void RegisterModels()
        {
            MvcContext.Current.MoldelsFolder = "Models";
        }
    }
}
