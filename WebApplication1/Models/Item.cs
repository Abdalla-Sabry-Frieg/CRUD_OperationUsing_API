using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class Item
    {
        [Key]
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public byte[]? Image { get; set; }

        [ForeignKey(nameof(category))]
        public int CategoryId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Category? category { get; set;}
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<OrderItem>? OrderItems { get; set;}

    }
}
