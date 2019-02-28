using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.Operations;

namespace iSpendApp.Models
{
    public class NewUser
    {

        [Required(ErrorMessage = "Field is empty or username is taken")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string RepeatedPassword {  get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree with our terms!")]
        public bool Agreed { get; set; }
    }
}
