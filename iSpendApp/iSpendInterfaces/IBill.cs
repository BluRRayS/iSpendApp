﻿using System;
using System.Collections.Generic;
using System.Text;

namespace iSpendInterfaces
{
    interface IBill
    {
        string BillName { get; set; }
        decimal BillBalance { get; set; }
        IEnumerable<ITransaction> Transactions { get; set; }
        int IconId { get; set; }
        IEnumerable<int> AccountIds { get; set; }
    }
}
