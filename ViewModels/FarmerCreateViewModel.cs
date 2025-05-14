using System.ComponentModel.DataAnnotations;

namespace ST10122162_Agri_Energy_Connect_Platform.ViewModels
{
    public class FarmerCreateViewModel
    {
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

        [Display(Name = "Create User Account")]
        public bool CreateAccount { get; set; } = true;

        // User account fields (only used if CreateAccount is true)
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
