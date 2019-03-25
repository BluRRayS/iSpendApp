using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.Dto
{
    internal class BillDto:IBill
    {
        public BillDto(int billId, string billName, double balance,DateTimeOffset time)
        {
            BillId = billId;
            BillName = billName;
            BillBalance = balance;
            DateOfCreation = time;
        }

        public int BillId { get; set; }
        public string BillName { get; set; }
        public double BillBalance { get; set; }
        public IEnumerable<ITransaction> Transactions { get; set; }
        public int IconId { get; set; }
        public IEnumerable<int> AccountIds { get; set; }
        public DateTimeOffset DateOfCreation { get; set; }

    }
}
