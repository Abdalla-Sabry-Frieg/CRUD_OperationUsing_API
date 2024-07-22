using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;
using System.IO;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.Items.ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll(int id)
        {
            var items = await _context.Items.FirstOrDefaultAsync(x=>x.Id == id);
            if(items == null)
            {
                return NotFound($"The item with id : {id} is not found");
            }
            return Ok(items);
        }

        [HttpGet("GetAllWithCategory/{CategoryId}")] // To avoid the method override
        public async Task<IActionResult> GetAllWithCategoryId(int CategoryId)
        {
            var items =await _context.Items.Where(x=>x.CategoryId ==  CategoryId).ToListAsync();
            if (items == null)
            {
                return NotFound($"The item with id : {CategoryId} is not found");
            }
            return Ok(items);
        }


        [HttpPost]
        public async Task<IActionResult> AddItem ([FromForm]ItemsViewModel model)
        {
            using var stream = new MemoryStream();
            await model.Image.CopyToAsync(stream);

            var item = new Item
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Image = stream.ToArray(),
            };

            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();  
            return Ok(item);
        }

        [HttpPut("{Itemsid}")]
        public async Task<IActionResult> UpdateItem(int id ,[FromForm] ItemsViewModel model)
        {

            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound($"The item with id : {id} is not found");
            }
            var category = await _context.Categories.AnyAsync(x=>x.Id == model.CategoryId);
            if(!category)
            {
                return NotFound($"The Category with id : {id} is not found");
            }
            if (model.Image != null)
            {
                using var stream = new MemoryStream();
                await model.Image.CopyToAsync(stream); 
                item.Image = stream.ToArray();
            }
            item.Name = model.Name;
            item.Description = model.Description;
            item.CategoryId = model.CategoryId;
            
            await _context.SaveChangesAsync();
            return Ok(item);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.SingleOrDefaultAsync(x=>x.Id == id);

            if (item == null)
            {
                return NotFound($"The item with id : {id} is not found");
            }

             _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(item);
        }
    }
}
