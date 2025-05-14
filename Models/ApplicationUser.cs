using Microsoft.AspNetCore.Identity;

namespace ST10122162_Agri_Energy_Connect_Platform.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? FarmerId { get; set; }
        public virtual Farmer? Farmer { get; set; }
    }
}
