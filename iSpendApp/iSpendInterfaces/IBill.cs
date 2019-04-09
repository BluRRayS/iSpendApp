using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace iSpendInterfaces
{
    public interface IBill
    {
        int BillId { get; set; }
        string BillName { get; set; }
        double BillBalance { get; set; }
        IEnumerable<ITransaction> Transactions { get; set; }
        int IconId { get; set; }
        IEnumerable<int> AccountIds { get; set; }
        IEnumerable<IReservation> Reservations { get; set; }
    }
}
