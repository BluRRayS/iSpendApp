using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.Dto
{
    internal class ReservationDto:IReservation
    {
        public ReservationDto(int reservationId, int accountId, int savingsId, decimal amount, DateTime date)
        {
            ReservationId = reservationId;
            AccountId = accountId;
            SavingsId = savingsId;
            Amount = amount;
            Date = date;
        }

        public int ReservationId { get; set; }
        public int AccountId { get; set; }
        public int SavingsId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
