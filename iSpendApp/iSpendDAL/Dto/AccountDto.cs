using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.Dto
{
    internal class AccountDto:IAccount
    {
        public AccountDto(int billId, string billName, double balance,DateTimeOffset time,int iconId)
        {
            AccountId = billId;
            AccountName = billName;
            AccountBalance = balance;
            DateOfCreation = time;
            IconId = iconId;
        }

        public AccountDto()
        {
                
        }

        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public double AccountBalance { get; set; }
        public IEnumerable<ITransaction> Transactions { get; set; }
        public int IconId { get; set; }
        public IEnumerable<int> UserIds { get; set; }
        public IEnumerable<IReservation> Reservations { get; set; }
        public DateTimeOffset DateOfCreation { get; set; }

    }
}
