using System.Collections.Generic;
using iSpendInterfaces;

namespace iSpendDAL.ContextInterfaces
{
    public interface ISavingsContext
    {
        void CreateSaving(ISaving saving);
        void UpdateSaving(ISaving saving);
        void DeleteSaving(int id);
        ISaving GetSavingById(int id);
        IEnumerable<ISaving> GetUserSavings(int id);
        void AddReservation(IReservation reservation);
        void RefreshSavingBalance(int id);
        IEnumerable<IReservation> GetReservations(int id);
        int GetNewSavingId();
    }
}