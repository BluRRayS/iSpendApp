using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Savings;
using iSpendInterfaces;
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
            throw new NotImplementedException();
        }

        public void DeleteSaving(int id)
        {
            Repository.DeleteSaving(id);
        }

        public ISaving GetSavingById(int id)
        {
            return Repository.GetSavingById(id);
        }

        public IEnumerable<ISaving> GetUserSavings(int id)
        {
            return Repository.GetUserSavings(id);
        }

        public void AddReservation(IReservation reservation)
        {
            Repository.AddReservation(reservation);
        }

        public void RefreshSavingsAmount(int id)
        {
            Repository.RefreshSavingsAmount(id);
        }

    }
}
