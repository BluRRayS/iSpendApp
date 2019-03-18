using System;

namespace iSpendInterfaces
{
    public interface IAccount
    {
        int UserId { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Email { get; set; }
    }
}
