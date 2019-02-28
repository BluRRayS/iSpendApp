using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iSpendWebApp.Models
{
    public class UserViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "A username is required.")]
        public string UserName { get; set; }

        [Display(Name = "Firstname")]
        [Required(ErrorMessage = "Your first name is required")]
        public string FirstName { get; set; }
        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter a valid Email address.")]
        [Required(ErrorMessage = "An Email is required.")]
        public string EmailAddress { get; set; }

        [Display(Name = "Confirm Email")]
        [Required(ErrorMessage = "Confirming your Email is required.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid Email address.")]
        [Compare("EmailAddress", ErrorMessage = "The Emails don't match.")]
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
