using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;

namespace Epam_6.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Customer { get; set; }

        public int Total { get; set; }

        [NotMapped]
        public Dictionary<string, int> OrderProductsIdsCount { get; set; }
        public Order(DateTime orderDate, string customer)
        {
            OrderDate = orderDate;
            Customer = customer;
        }

        public Order()
        {
                
        }
    }
}
