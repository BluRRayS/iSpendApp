using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using iSpendInterfaces;

namespace iSpendWebApp.Models.User
{
    public class EditUserViewModel : IAccount
    {
        public int UserId { get; set; }

        [Required]
        [MinLength(4)]
        public string Username { get; set; }

        [Required]
        [MinLength(4)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
