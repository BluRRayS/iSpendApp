﻿using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendLogic.Models
{
    internal class Account : IAccount
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
