using System;
using System.Diagnostics;
using Microsoft.Owin.Hosting;

namespace ProjectManager
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main()
        {
            var hostingUrl = "http://localhost:8500";
            using (var webApp = WebApp.Start<Startup>(hostingUrl))
            {
                Process.Start(hostingUrl);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return 0;
            }
        }
    }
}
