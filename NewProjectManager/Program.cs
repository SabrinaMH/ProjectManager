using System;
using Microsoft.Owin.Hosting;
using ProjectManager;

namespace NewProjectManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main()
        {
            using (var webApp = WebApp.Start<Startup>("http://localhost:8500"))
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return 0;
            }
        }
    }
}
