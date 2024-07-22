using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class Order
    {
        public int? id { get; set; }
        public DateTime CreatedDate { set; get; } = DateTime.Now;
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<OrderItem>? ordersItems { get; set; }
    }
}
