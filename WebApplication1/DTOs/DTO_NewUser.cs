using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    // class that contains the data from user in side 
    public class DTO_NewUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
