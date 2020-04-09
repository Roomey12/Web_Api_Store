using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epam_6.Models;

namespace Epam_6
{
    public class DbOperations
    {
        public static void FillDatabase()
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                Product p1 = new Product("Milk", 100);
                Product p2 = new Product("Bread", 150);
                Product p3 = new Product("Tomato", 50);
                db.Products.AddRange(p1,p2,p3);
                Order o1 = new Order(new DateTime(2020, 3, 18), "Mike Miller");
                Order o2 = new Order(new DateTime(2020, 3, 20), "Ben Brown");
                db.Orders.AddRange(o1, o2);
                db.SaveChanges();
                OrderItem oi1 = new OrderItem(1, 1, 3);
                OrderItem oi2 = new OrderItem(1, 3, 1);
                OrderItem oi3 = new OrderItem(2, 1, 7);
                db.OrderItems.AddRange(oi1, oi2, oi3);
                db.SaveChanges();
            }
        }
    }
}
