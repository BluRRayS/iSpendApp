using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Savings;
using iSpendInterfaces;
using iSpendInterfaces.Helpers;
using iSpendLogic.Models;

namespace iSpendLogic
{
    public class SavingLogic
    {
        private SavingsRepository Repository { get; }

        public SavingLogic(ISavingsContext context)
        {
            Repository = new SavingsRepository(context);
        }

        public void CreateSaving(ISaving saving,int accountId)
        {
            Repository.CreateSaving(saving);
            Repository.AddReservation(new Reservation(0,accountId,Repository.GetNewSavingId(),saving.SavingCurrentAmount,DateTime.Now) );
        }

        public void UpdateSaving(ISaving saving)
        {
            if (saving.SavingCurrentAmount > saving.SavingsGoalAmount) throw new ArgumentOutOfRangeException();
            Repository.UpdateSaving(saving);
        }

        public void DeleteSaving(int id)
        {
            Repository.DeleteSaving(id);
        }

        public ISaving GetSavingById(int id)
        {
            return Repository.GetSavingById(id);
        }

        public IEnumerable<ISaving> GetOngoingUserSavings(int id)
        {
            return Repository.GetUserSavings(id).Where(saving => saving.State != SavingState.Paid);
        }

        public IEnumerable<ISaving> GetAllUserSavings(int id)
        {
            return Repository.GetUserSavings(id);
        }

        public void AddReservation(IReservation reservation)
        {
            reservation.Date = DateTime.Now;
            var saving = Repository.GetSavingById(reservation.SavingsId);
            if (reservation.Amount + saving.SavingCurrentAmount > saving.SavingsGoalAmount) throw  new Exception("Reservation precedes goal");
            else if (reservation.Amount + saving.SavingCurrentAmount == saving.SavingsGoalAmount)
            {
                saving.State = SavingState.Completed;
                saving.SavingCurrentAmount += reservation.Amount;
                Repository.UpdateSaving(saving);
            }
            Repository.AddReservation(reservation);
        
        }

        public void RefreshSavingsAmount(int id)
        {
            Repository.RefreshSavingsAmount(id);
        }

        public void CompleteSaving(int id)
        {
            var saving = Repository.GetSavingById(id);
            saving.State = SavingState.Paid;
            Repository.CompleteSaving(saving);
            Repository.UpdateSaving(saving);            
        }
    }
}
