using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendDAL.Savings
{
    public class SavingsRepository
    {
        private readonly ISavingsContext _context;
        public SavingsRepository(ISavingsContext context)
        {
            _context = context;
        }

        public void CreateSaving(ISaving saving)
        {
           _context.CreateSaving(saving);
        }

        public void UpdateSaving(ISaving saving)
        {
            _context.UpdateSaving(saving);
        }

        public void DeleteSaving(int id)
        {
           _context.DeleteSaving(id);
        }

        public ISaving GetSavingById(int id)
        {
            return _context.GetSavingById(id);
        }

        public IEnumerable<ISaving> GetUserSavings(int userId)
        {
            return _context.GetUserSavings(userId);
        }

        public void AddReservation(IReservation reservation)
        {
            _context.AddReservation(reservation);
        }

        public IEnumerable<IReservation> GetSavingsReservations(int id)
        {
            return _context.GetReservations(id);
        }

        public void RefreshSavingsAmount(int id)
        {
            _context.RefreshSavingBalance(id);
        }

        public int GetNewSavingId()
        {
            return _context.GetNewSavingId();
        }

        public void CompleteSaving(ISaving saving)
        {
            _context.CompleteSaving(saving);
        }
    }
}
