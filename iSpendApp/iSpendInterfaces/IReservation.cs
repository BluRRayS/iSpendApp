using System;
using System.Collections.Generic;
using System.Text;

namespace iSpendInterfaces
{
    public interface IReservation
    {
        int ReservationId { get; set; }
        int AccountId { get; set; }
        int SavingsId { get; set; }
        decimal Amount { get; set; }
        DateTime Date { get; set; }
    }
}
