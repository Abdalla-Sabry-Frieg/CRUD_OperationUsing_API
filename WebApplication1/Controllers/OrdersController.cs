using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var Orders =  _context.Orders.ToArray();

            return Ok(Orders);
        }

        // Can ser Overrideing for the same methoud but must set a new uniqe rout 
        [HttpGet("GetOrdersById/{id}")]
        public async Task<IActionResult> GetOrders(int id)
        {
            var Orders = await _context.Orders.Where(x=>x.id == id).FirstOrDefaultAsync();
            if (Orders != null)
            {
               DTOsOrders dtoOrder= new()
                {
                    CreatedAt = Orders.CreatedDate,
                    orderId= Orders.id,
                    
                };
                if(Orders.ordersItems.Any())  // if exists
                {
                    foreach (var item in Orders.ordersItems)
                    {
                        dtoOrderItems dtoOrderItemNew = new dtoOrderItems()
                        {
                            itemId = item.items.Id,
                            itemName=item.items.Name,
                            Price=item.items.Price,
                            CategoryName=item.items.category.Name,

                        };
                        dtoOrder.dtoOrderItems.Add(dtoOrderItemNew);
                    }
                }
                return Ok(dtoOrder);
            }
            return NotFound($"The order id {id} not found");


        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] DTOsOrders model)
        {
           
            if(ModelState.IsValid)
            {
                var order = new Order()
                {
                    CreatedDate = model.CreatedAt,
                    ordersItems = new List<OrderItem>(),
                };
                foreach (var item in model.dtoOrderItems)
                {
                    OrderItem orderItem = new()
                    {
                        ItemId =(int) item.itemId,
                        Price = item.Price,

                    };
                    order.ordersItems.Add(orderItem);

                }
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            return BadRequest();
        }
    }
}
