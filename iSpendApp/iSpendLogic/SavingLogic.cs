using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Savings;
using iSpendInterfaces;

namespace iSpendLogic
{
    public class SavingLogic
    {
        private SavingsRepository Repository { get; }

        public SavingLogic(ISavingsContext context)
        {
            Repository = new SavingsRepository(context);
        }

        public void CreateSaving(ISaving saving)
        {
            Repository.CreateSaving(saving);
        }

        public void UpdateSaving(ISaving saving)
        {
            throw new NotImplementedException();
        }

        public void DeleteSaving(int id)
        {
            throw new NotImplementedException();
        }

        public ISaving GetSavingById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISaving> GetUserSavings(int id)
        {
            return Repository.GetUserSavings(id);
        }


    }
}
