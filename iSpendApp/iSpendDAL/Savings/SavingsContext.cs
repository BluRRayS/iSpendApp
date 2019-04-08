using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Dto;
using iSpendInterfaces;
using iSpendInterfaces.Helpers;

namespace iSpendDAL.Savings
{
    public class SavingsContext:ISavingsContext
    {
        private readonly DatabaseConnection _connection;
        public SavingsContext(DatabaseConnection connection)
        {
            _connection = connection;
        }

        public void CreateSaving(ISaving saving)
        {
            var command = new SqlCommand("INSERT INTO dbo.Savings (UserId,SavingsName,CurrentAmount,IconId,GoalAmount,[State],DateOfCreation,GoalDate) VALUES(@UserId,@Name,@Current,@IconId,@Goal,@State,@DateOfC, @GoalDate)", _connection.SqlConnection);
            _connection.SqlConnection.Open();
            command.Parameters.AddWithValue("@Name",saving.SavingName);
            command.Parameters.AddWithValue("@Current", saving.SavingCurrentAmount);
            command.Parameters.AddWithValue("@Goal", saving.SavingsGoalAmount);
            command.Parameters.AddWithValue("@IconId", saving.IconId);
            command.Parameters.AddWithValue("@State", (int)saving.State);
            command.Parameters.AddWithValue("@GoalDate", saving.GoalDate);
            command.Parameters.AddWithValue("@UserId", saving.UserId);
            command.Parameters.AddWithValue("@DateOfC", DateTime.Now);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
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
            var savings = new List<SavingsDto>();
            var command = new SqlCommand("SELECT SavingsId,UserId,SavingsName,CurrentAmount,GoalAmount,IconId,[State],GoalDate FROM dbo.Savings WHERE UserId = @Id",_connection.SqlConnection);
            _connection.SqlConnection.Open();
            command.Parameters.AddWithValue("@Id",id);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    savings.Add(new SavingsDto(reader.GetInt32(1),reader.GetInt32(0),reader.GetString(2),reader.GetDecimal(3),reader.GetDecimal(4),(SavingState)reader.GetInt32(6),reader.GetInt32(5),reader.GetDateTime(7)));
                }
            }
            _connection.SqlConnection.Close();
            return savings;
        }
    }
}
