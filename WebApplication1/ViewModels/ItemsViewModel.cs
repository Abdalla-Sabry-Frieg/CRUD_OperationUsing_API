namespace WebApplication1.ViewModels
{
    public class ItemsViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; } 
        public int CategoryId { get; set; }
    }
}
