using System;
using Microsoft.Owin.Hosting;

namespace ProjectManager
{
    public class Program
    {
        static int Main()
        {
            var webApp = WebApp.Start<Startup>("http://localhost:8000");
            Console.ReadKey();
            return 0;
        }
    }
}