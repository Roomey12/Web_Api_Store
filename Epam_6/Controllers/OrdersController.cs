using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Epam_6;
using Epam_6.Models;

namespace Epam_6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public OrdersController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        //GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        //получить товары из заказа
        [HttpGet("{id}/products")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderProducts(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return await _context.OrderItems.Where(p => p.OrderId == order.OrderId).ToListAsync();
        }

        //получить только названия товаров в заказе
        [HttpGet("{id}/products/string")]
        public async Task<ActionResult<IEnumerable<string>>> GetOrderProductsString(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return await _context.OrderItems.Where(p => p.OrderId == order.OrderId).Select(p=>p.Product.Name).ToListAsync();
        }

        //добавить продукт к заказу
        [HttpPut("{id}/products")]
        public async Task<IActionResult> PutProductInOrder(int id, [FromBody]OrderItem orderItem)
        {
            var order = _context.Orders.SingleOrDefault(p => p.OrderId == id);

            OrderItem oi = new OrderItem(id, orderItem.ProductId, orderItem.Count);
            _context.OrderItems.Add(oi);
            await _context.SaveChangesAsync();

            order.Total = _context.OrderItems
                .Where(p => p.OrderId == id)
                .Include(p => p.Product)
                .Sum(p => p.Count * p.Product.Price);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(orderItem.OrderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //создать заказ
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder([FromBody]Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var newOrder = _context.Orders.SingleOrDefault(p => p.OrderId ==
                                                           _context.Orders.Max(p => p.OrderId));

            foreach (var e in order.OrderProductsIdsCount)
            {
                OrderItem oi = new OrderItem(newOrder.OrderId, Convert.ToInt32(e.Key), e.Value);
                _context.OrderItems.Add(oi);
            }
            await _context.SaveChangesAsync();

            newOrder.Total = _context.OrderItems
                .Where(p => p.OrderId == newOrder.OrderId)
                .Include(p => p.Product)
                .Sum(p => p.Count * p.Product.Price);

            await _context.SaveChangesAsync();
            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
