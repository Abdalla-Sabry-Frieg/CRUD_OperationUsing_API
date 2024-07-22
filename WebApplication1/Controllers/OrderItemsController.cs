using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
    public class OrderItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var OrderItems = await _context.OrderItems.ToListAsync();
            return Ok(OrderItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll(int id)
        {
            var OrderItems = await _context.OrderItems.FirstOrDefaultAsync(x => x.Id == id);
            if (OrderItems == null)
            {
                return NotFound($"The Order Items with id : {id} is not found");
            }
            return Ok(OrderItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem(OrderItem model)
        {
          

            var orderItem = new OrderItem
            {
               ItemId=model.ItemId,
               OrderId=model.OrderId,
               Price=model.Price,
            };

            await _context.OrderItems.AddAsync(orderItem);
             _context.SaveChangesAsync();
            return Ok(orderItem);
        }

    }
}
