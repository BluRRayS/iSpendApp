using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.Dto
{
    internal class UserDto:IUser
    {
        public UserDto()
        {
            
        }

        public UserDto(int userId, string username)
        {
            UserId = userId;
            Username = username;
        }

        public UserDto(int userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
