using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace iSpendInterfaces
{
    public interface IAccount
    {
        int AccountId { get; set; }
        string AccountName { get; set; }
        double AccountBalance { get; set; }
        IEnumerable<ITransaction> Transactions { get; set; }
        int IconId { get; set; }
        IEnumerable<int> UserIds { get; set; }
        IEnumerable<IReservation> Reservations { get; set; }
    }
}
