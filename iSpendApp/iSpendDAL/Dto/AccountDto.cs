using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.Dto
{
    internal class AccountDto:IUser
    {
        public AccountDto()
        {
            
        }

        public AccountDto(int userId, string username)
        {
            UserId = userId;
            Username = username;
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
