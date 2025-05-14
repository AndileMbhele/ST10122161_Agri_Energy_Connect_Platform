using System.ComponentModel.DataAnnotations;

namespace ST10122162_Agri_Energy_Connect_Platform.Models
{
    public class Farmer
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Farm name is required")]
        [Display(Name = "Farm Name")]
        [StringLength(100, ErrorMessage = "Farm name cannot be longer than 100 characters")]
        public string FarmName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required")]
        [Display(Name = "Contact Number")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string ContactNumber { get; set; } = string.Empty;

        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Navigation property
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        
        // Navigation property to ApplicationUser
        public virtual ApplicationUser? User { get; set; }
    }
}
