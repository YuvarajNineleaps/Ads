using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Hosting;
using System.Threading;

namespace Ads
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:8080"))
            {
                Console.WriteLine("Web Server is running.");
                Thread.Sleep(Timeout.Infinite);
                Console.WriteLine("Press any key to quit.");
                Console.ReadLine();
            }
        }
    }
}