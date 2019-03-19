using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Dto;
using iSpendInterfaces;

namespace iSpendDAL
{
    public class BillContext:IBillContext
    {
        private List<IBill> userBills = new List<IBill>();
        private readonly DatabaseConnection _connection;

        public BillContext(DatabaseConnection connection)
        {
            _connection = connection;
        }

        public void AddBill(IBill newBill)//Todo: Add transaction instead of adding balance directly
        //ToDo: test SubQuery second command.
        {
            _connection.SqlConnection.Open();
            var command = new SqlCommand("INSERT INTO [Account] Name,Balance,DateOfCreation VALUES(@Name,@Balance,@DateOfCreation,@UserId)",_connection.SqlConnection);
            command.Parameters.AddWithValue("@Name",newBill.BillName);
            command.Parameters.AddWithValue("@Balance", newBill.BillBalance);
            command.Parameters.AddWithValue("@DateOfCreation",DateTimeOffset.Now);
            command.ExecuteNonQuery();
            command.Parameters.Clear();
            command = new SqlCommand("INSERT INTO [User_Account] UserId,AccountId VALUES(@UserId,(SELECT SCOPE_IDENTITY() FROM [Account])) ", _connection.SqlConnection);
            command.Parameters.AddWithValue("@UserId",newBill.AccountIds.First());
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public void RemoveBill(IBill billToRemove)
        {
            throw new NotImplementedException();
        }

        public void UpdateBill(IBill billToUpdate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBill> GetBillsByUsername(string username)
        {
            userBills.Clear();
            _connection.SqlConnection.Open();
            var command = new SqlCommand("SELECT Id,Name,Balance,DateOfCreation FROM [Account] WHERE Id = (SELECT AccountId FROM [User_Account] WHERE UserId = (SELECT Id FROM [User] where UserName = @Username))",_connection.SqlConnection);
            command.Parameters.AddWithValue("@Username", username);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                userBills = new List<IBill>();
                while (reader.Read())
                {
                     userBills.Add( new BillDto(reader.GetInt32(0),reader.GetString(1),reader.GetDecimal(3),reader.GetDateTimeOffset(4)));
                }
            }
            _connection.SqlConnection.Close();
            return userBills;
        }

        public IBill GetBillById(int billId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITransaction> GetBillTransactions(int billId)
        {
            throw new NotImplementedException();
        }

    }

}
