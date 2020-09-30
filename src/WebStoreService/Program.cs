using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebStoreService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // using (var db = new WebStoreContext())
            // {
            //     // Create
            //     db.Add(new Product 
            //     { 
            //         Id = 0, 
            //         Name = "Banana",
            //         Value = 2
            //     });
            //     db.SaveChanges();

            //     // Read
            //     Console.WriteLine("Querying for a blog");
            //     Console.WriteLine(db.Products
            //         .OrderBy(p => p.Id)
            //         .FirstOrDefault());
            // }
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
