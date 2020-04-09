using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam_6.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int Count { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public OrderItem(int orderId, int productId, int count)
        {
            OrderId = orderId;
            ProductId = productId;
            Count = count;
        }

        public OrderItem(int productId, int count)
        {
            ProductId = productId;
            Count = count;
        }

        public OrderItem()
        {
                
        }
    }
}
