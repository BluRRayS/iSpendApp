using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using iSpendInterfaces;

namespace iSpendWebApp.Models
{
    public class ReservationViewModel:IReservation
    {
        public ReservationViewModel(IReservation reserve)
        {
            ReservationId = reserve.ReservationId;
            AccountId = reserve.AccountId;
            SavingsId = reserve.SavingsId;
            Amount = reserve.Amount;
            Date = reserve.Date;
        }

        public ReservationViewModel()
        {
            
        }

        public int ReservationId { get; set; }
        public int AccountId { get; set; }
        public int SavingsId { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<ISaving> UserSavings{ get; set; }
    }
}
