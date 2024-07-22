using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
           _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var category = await _context.Categories.ToListAsync();
          
            return Ok(category);
        }

        // Can ser Overrideing for the same methoud but must set a new uniqe rout 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategories(int id)
        {
            var category = await _context.Categories.Include(x=>x.Items).FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound($"Category Id {category.Id} not found");
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(string name , string description)
        {
            var category = new Category() { Name = name, Description = description };
           

            await _context.Categories.AddAsync(category);
            _context.SaveChanges();
            return Ok(category);

        }

        [HttpPut] // Update full object 
        public async Task<IActionResult> UpdateCategory(Category c) //[FromBody]
        {

            var category =await _context.Categories.SingleOrDefaultAsync(x => x.Id == c.Id);

            if (category == null) 
            {
                return NotFound($"Category Id {c.Id} not found");
            }
            category.Name = c.Name;
            category.Description = c.Description;
           
            await _context.SaveChangesAsync();
            return Ok(category);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCAtegory(int id)
        {
            var categoryId = await _context.Categories.SingleOrDefaultAsync(x=>x.Id ==  id);
            if (categoryId == null)
            {
                return NotFound($"Category Id {id} not found");
            }

             _context.Categories.Remove(categoryId);
             _context.SaveChangesAsync();
             return Ok(categoryId);

        }

        [HttpPatch("{id}")] // Update in one element inside the Object from class

        //[FromRoute] is meaning will take this parameter from HttpPatch
        public async Task<IActionResult> UpdatePatch([FromBody] JsonPatchDocument<Category> category , [FromRoute] int id)
        {
            var c = await _context.Categories.SingleOrDefaultAsync(x => x.Id == id);

            if (c == null)
            {
                return NotFound($"Category Id {id} not found");
            }

            category.ApplyTo(c);
            await _context.SaveChangesAsync();
            return Ok(category);
        }
    }
}
