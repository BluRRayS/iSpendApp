using System;
using iSpendInterfaces;

namespace iSpendUnitTests
{
    internal class AccountDto:IUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
