using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
            var command = new SqlCommand("UPDATE dbo.Savings set SavingsName=@Name,CurrentAmount=@Current,GoalAmount=@Goal,IconId=@IconId,[State]=@State,GoalDate=@GoalDate WHERE SavingsId=@Id", _connection.SqlConnection);
            _connection.SqlConnection.Open();
            command.Parameters.AddWithValue("@Id", saving.SavingId);
            command.Parameters.AddWithValue("@Name", saving.SavingName);
            command.Parameters.AddWithValue("@Current", saving.SavingCurrentAmount);
            command.Parameters.AddWithValue("@Goal", saving.SavingsGoalAmount);
            command.Parameters.AddWithValue("@IconId", saving.IconId);
            command.Parameters.AddWithValue("@State", (int)saving.State);
            command.Parameters.AddWithValue("@GoalDate", saving.GoalDate);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public void DeleteSaving(int id)
        {
            var command1 = new SqlCommand("DELETE FROM dbo.Savings WHERE SavingsId = @Id", _connection.SqlConnection);
            command1.Parameters.AddWithValue("@Id", id);
            var command2 = new SqlCommand("DELETE FROM dbo.Reservations WHERE SavingsId = @Id", _connection.SqlConnection);
            command2.Parameters.AddWithValue("@Id", id);
            _connection.SqlConnection.Open();
            command2.ExecuteNonQuery();
            command1.ExecuteNonQuery();          
            _connection.SqlConnection.Close();
        }

        public ISaving GetSavingById(int id)
        {
            var saving = new SavingsDto {SavingId = id};
            var command = new SqlCommand("SELECT SavingsId,UserId,SavingsName,CurrentAmount,GoalAmount,IconId,[State],GoalDate FROM dbo.Savings WHERE SavingsId = @Id",_connection.SqlConnection);
           command.Parameters.AddWithValue("@Id", id);
           _connection.SqlConnection.Open();
           command.ExecuteNonQuery();
           using (var reader = command.ExecuteReader())
           {
               if(reader.Read())
               {
                   saving.UserId = reader.GetInt32(1);
                   saving.SavingName = reader.GetString(2);
                   saving.SavingCurrentAmount = reader.GetDecimal(3);
                   saving.SavingsGoalAmount = reader.GetDecimal(4);
                   saving.IconId = reader.GetInt32(5);
                   saving.State = (SavingState) reader.GetInt32(6);
                   saving.GoalDate = reader.GetDateTime(7);
               }
           }
           _connection.SqlConnection.Close();
           return saving;
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

        public void AddReservation(IReservation reservation)
        {
            var command = new SqlCommand("insert into dbo.Reservations  (AccountId,SavingsId,Amount,[Date]) VALUES (@AccountId,@SavingsId,@Amount,@Date)", _connection.SqlConnection);
            _connection.SqlConnection.Open();
            command.Parameters.AddWithValue("@AccountId", reservation.AccountId);
            command.Parameters.AddWithValue("@SavingsId",reservation.SavingsId);
            command.Parameters.AddWithValue("@Amount", reservation.Amount);
            command.Parameters.AddWithValue("@Date", reservation.Date);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public void RefreshSavingBalance(int id)
        {
            var command = new SqlCommand("UPDATE dbo.Savings SET CurrentAmount = (SELECT SUM(Amount) FROM dbo.Reservations WHERE SavingsId = @Id)", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public IEnumerable<IReservation> GetReservations(int id)
        {
            var reservations = new List<IReservation>();
            var command = new SqlCommand("SELECT ReservationId,AccountId,SavingsId,Amount,[Date] FROM dbo.Reservations WHERE SavingsId = @Id", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    reservations.Add(new ReservationDto(reader.GetInt32(0),reader.GetInt32(1),reader.GetInt32(2),reader.GetDecimal(3),reader.GetDateTime(4)));
                }
            }
            _connection.SqlConnection.Close();
            return reservations;
        }

        public int GetNewSavingId()
        {
           var command = new SqlCommand("SELECT MAX(SavingsId) FROM dbo.Savings",_connection.SqlConnection);
           var savingId = 0;
           _connection.SqlConnection.Open();
           command.ExecuteNonQuery();
           using (var reader = command.ExecuteReader())
           {
               if (reader.Read())
               {
                   savingId = reader.GetInt32(0);
               }
            }         
           _connection.SqlConnection.Close();
           return savingId;
        }

        public void CompleteSaving(ISaving saving)
        {
            var reservations = GetReservations(saving.SavingId).ToList();
            var commands = reservations.Select(reservation => new SqlCommand("INSERT INTO dbo.[Transaction] ([Name],Amount,TimeOfTransaction,AccountId,Category,IconId) VALUES('Saving Reservation',@Amount,@Time,@AccountId,'SavingPlan',@IconId)", _connection.SqlConnection)).ToList();
            var i = 0;
            foreach (var command in commands)
            {
                command.Parameters.AddWithValue("@Amount", -reservations[i].Amount);
                command.Parameters.AddWithValue("@Time", DateTime.Now);
                command.Parameters.AddWithValue("@AccountId", reservations[i].AccountId);
                command.Parameters.AddWithValue("IconId", 0);
                i++;
            }
            _connection.SqlConnection.Open();
            foreach (var command in commands)
            {
                command.ExecuteNonQuery();
            }
            _connection.SqlConnection.Close();
            RemoveReservations(saving.SavingId);
        }

        public void RemoveReservations(int id)
        {
            var command = new SqlCommand("DELETE FROM  dbo.Reservations WHERE SavingsId = @Id",_connection.SqlConnection);
            command.Parameters.AddWithValue("@Id",id);
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

    }
}
