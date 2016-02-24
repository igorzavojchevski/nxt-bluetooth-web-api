using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using NxtApi2.Controllers;

[assembly: OwinStartup(typeof(NxtApi2.Startup))]

namespace NxtApi2
{
    public partial class Startup
    {
        bool isConnected;
        public void Configuration(IAppBuilder app)
        { 
            ConfigureAuth(app);
            app.MapSignalR();

            if (!isConnected)
            {
                RobotController r = new RobotController();
                r.Connect("COM4");
                isConnected = true;
            }

            else
            {
                Console.WriteLine("Веќе сте конектирани на NXT!");
            }
        }
    }
}
