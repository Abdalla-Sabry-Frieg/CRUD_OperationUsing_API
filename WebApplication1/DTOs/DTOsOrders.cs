namespace WebApplication1.DTOs
{
    public class DTOsOrders
    {
        public int? orderId { get; set; }
        public DateTime CreatedAt { get; set; }
      
        public ICollection<dtoOrderItems>? dtoOrderItems { get; set; } = new List<dtoOrderItems>();
    }

    public class dtoOrderItems
    {
        public int? itemId { get; set; }
        public decimal? Price { get; set;}
        public string? itemName { get; set; }
        public string? CategoryName { get; set; }

    }
}
