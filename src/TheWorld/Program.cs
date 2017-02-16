using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace TheWorld
{
    public class Program
    {
        public static void Main(string[] args)      //starting point for all of the code
        {
            var host = new WebHostBuilder()     //listens to requests (web host)
                .UseKestrel()                   //web server
                .UseContentRoot(Directory.GetCurrentDirectory())        //where the content is (dir)
                .UseIISIntegration()                
                .UseStartup<Startup>()          //use class Startup to setup my web server
                .Build();

            host.Run();
        }
    }
}
