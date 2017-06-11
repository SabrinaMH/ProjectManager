using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using Microsoft.Owin.Hosting;
using ProjectManager.Features.SendEmail;

namespace ProjectManager
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main()
        {
            var emailService = new EmailService();
            int emailCheckEveryXMinutes = int.Parse(ConfigurationManager.AppSettings["email.check.inMinutes"]);
            var timer = new Timer(async state => await emailService.SendEmailsAsync(), null, TimeSpan.FromMinutes(emailCheckEveryXMinutes), TimeSpan.FromMinutes(emailCheckEveryXMinutes));

            var hostingUrl = "http://localhost:8500";
            using (var webApp = WebApp.Start<Startup>(hostingUrl))
            {
                Process.Start(hostingUrl);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                timer.Dispose();
                return 0;
            }
        }
    }
}
