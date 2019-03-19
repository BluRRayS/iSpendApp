using System.ComponentModel.DataAnnotations;
using iSpendInterfaces;

namespace iSpendWebApp.Models.User
{
    public class UserViewModel:IAccount
    {
        public int UserId { get; set; }


        [Display(Name = "Username")]
        [Required(ErrorMessage = "A username is required.")]
        [MinLength(4)]
        public string Username { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter a valid Email address.")]
        [Required(ErrorMessage = "An Email is required.")]
        public string Email { get; set; }

        [Display(Name = "Confirm Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid Email address.")]
        [Compare("EmailAddress", ErrorMessage = "The Emails don't match.")]
        [Required]
        public string ConfirmEmail { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "A password is required.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Password is too short.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Repeating your password is required")]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
