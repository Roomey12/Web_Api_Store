using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Epam_6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (ApplicationContext _context = new ApplicationContext())
            {
                var order = _context.Orders.Where(p => p.OrderId == 1).FirstOrDefault();
                order.Total = _context.OrderItems
                    .Where(p => p.OrderId == order.OrderId)
                    .Include(p => p.Product)
                    .Sum(p => p.Count * p.Product.Price);
                _context.SaveChanges();
            }
            //DbOperations.FillDatabase();
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
