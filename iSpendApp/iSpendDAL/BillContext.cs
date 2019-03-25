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

        public void AddBill(IBill newBill,int userId)//Todo: Add transaction instead of adding balance directly
        //ToDo: test SubQuery second command.
        {
            _connection.SqlConnection.Open();
            var command = new SqlCommand("INSERT INTO dbo.Account (Name,Balance,DateOfCreation) VALUES(@Name,@Balance,@DateOfCreation)",_connection.SqlConnection);
            command.Parameters.AddWithValue("@Name",newBill.BillName);
            command.Parameters.AddWithValue("@Balance", newBill.BillBalance);
            command.Parameters.AddWithValue("@DateOfCreation",DateTime.Now);
            command.ExecuteNonQuery();
            command.Parameters.Clear();
            command = new SqlCommand("INSERT INTO dbo.User_Account (UserId,AccountId) VALUES(@UserId,(SELECT Max(Id) From Account)) ", _connection.SqlConnection);
            command.Parameters.AddWithValue("@UserId",userId);
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
            var command = new SqlCommand("SELECT * FROM [Account] INNER JOIN dbo.User_Account ON dbo.Account.Id = dbo.User_Account.AccountId  AND dbo.User_Account.UserId = (SELECT Id FROM dbo.[User] WHERE UserName = @Username)", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Username", username);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                userBills = new List<IBill>();
                while (reader.Read())
                {
                    userBills.Add(new BillDto(reader.GetInt32(0), reader.GetString(1),Convert.ToDouble(reader.GetDecimal(2)),reader.GetDateTime(3)));
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
