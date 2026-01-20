using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
;
using Microsoft.Extensions.Hosting.WindowsServices;

var builder = Host.CreateApplicationBuilder(args);

// This tells the app to behave like a service when installed, 
// but it stays a console app when you press F5 in Visual Studio.
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Satark";
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

namespace WindowsService1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
