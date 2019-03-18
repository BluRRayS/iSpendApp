﻿using System;
using System.Collections.Generic;
using System.Text;

namespace iSpendInterfaces
{
    interface ITransaction
    {
        string TransactionName { get; set; }
        decimal TransactionAmount { get; set; }
        string Category { get; set; }
        int IconId { get; set; }
    }
}