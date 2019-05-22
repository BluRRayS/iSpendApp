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
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("INSERT INTO dbo.Savings (UserId,SavingsName,CurrentAmount,IconId,GoalAmount,[State],DateOfCreation,GoalDate) VALUES(@UserId,@Name,@Current,@IconId,@Goal,@State,@DateOfC, @GoalDate)", connection);
                command.Parameters.AddWithValue("@Name", saving.SavingName);
                command.Parameters.AddWithValue("@Current", saving.SavingCurrentAmount);
                command.Parameters.AddWithValue("@Goal", saving.SavingsGoalAmount);
                command.Parameters.AddWithValue("@IconId", saving.IconId);
                command.Parameters.AddWithValue("@State", (int)saving.State);
                command.Parameters.AddWithValue("@GoalDate", saving.GoalDate);
                command.Parameters.AddWithValue("@UserId", saving.UserId);
                command.Parameters.AddWithValue("@DateOfC", DateTime.Now);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void UpdateSaving(ISaving saving)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("UPDATE dbo.Savings set SavingsName=@Name,CurrentAmount=@Current,GoalAmount=@Goal,IconId=@IconId,[State]=@State,GoalDate=@GoalDate WHERE SavingsId=@Id", connection);
                command.Parameters.AddWithValue("@Id", saving.SavingId);
                command.Parameters.AddWithValue("@Name", saving.SavingName);
                command.Parameters.AddWithValue("@Current", saving.SavingCurrentAmount);
                command.Parameters.AddWithValue("@Goal", saving.SavingsGoalAmount);
                command.Parameters.AddWithValue("@IconId", saving.IconId);
                command.Parameters.AddWithValue("@State", (int)saving.State);
                command.Parameters.AddWithValue("@GoalDate", saving.GoalDate);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void DeleteSaving(int id)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command1 = new SqlCommand("DELETE FROM dbo.Savings WHERE SavingsId = @Id", connection);
                command1.Parameters.AddWithValue("@Id", id);
                var command2 = new SqlCommand("DELETE FROM dbo.Reservations WHERE SavingsId = @Id", connection);
                command2.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command2.ExecuteNonQuery();
                command1.ExecuteNonQuery();
                connection.Close();
            }
        }

        public ISaving GetSavingById(int id)
        {
            var saving = new SavingsDto { SavingId = id };
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT SavingsId,UserId,SavingsName,CurrentAmount,GoalAmount,IconId,[State],GoalDate FROM dbo.Savings WHERE SavingsId = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        saving.UserId = reader.GetInt32(1);
                        saving.SavingName = reader.GetString(2);
                        saving.SavingCurrentAmount = reader.GetDecimal(3);
                        saving.SavingsGoalAmount = reader.GetDecimal(4);
                        saving.IconId = reader.GetInt32(5);
                        saving.State = (SavingState)reader.GetInt32(6);
                        saving.GoalDate = reader.GetDateTime(7);
                    }
                }
                connection.Close();
            }
           return saving;
        }

        public IEnumerable<ISaving> GetUserSavings(int id)
        {
            var savings = new List<SavingsDto>();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT SavingsId,UserId,SavingsName,CurrentAmount,GoalAmount,IconId,[State],GoalDate FROM dbo.Savings WHERE UserId = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        savings.Add(new SavingsDto(reader.GetInt32(1), reader.GetInt32(0), reader.GetString(2), reader.GetDecimal(3), reader.GetDecimal(4), (SavingState)reader.GetInt32(6), reader.GetInt32(5), reader.GetDateTime(7)));
                    }
                }
                connection.Close();
            }
            return savings;
        }

        public void AddReservation(IReservation reservation)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("insert into dbo.Reservations  (AccountId,SavingsId,Amount,[Date]) VALUES (@AccountId,@SavingsId,@Amount,@Date)", connection);
                command.Parameters.AddWithValue("@AccountId", reservation.AccountId);
                command.Parameters.AddWithValue("@SavingsId", reservation.SavingsId);
                command.Parameters.AddWithValue("@Amount", reservation.Amount);
                command.Parameters.AddWithValue("@Date", reservation.Date);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void RefreshSavingBalance(int id)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("UPDATE dbo.Savings SET CurrentAmount = (SELECT SUM(Amount) FROM dbo.Reservations WHERE SavingsId = @Id)", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public IEnumerable<IReservation> GetReservations(int id)
        {
            var reservations = new List<IReservation>();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT ReservationId,AccountId,SavingsId,Amount,[Date] FROM dbo.Reservations WHERE SavingsId = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add(new ReservationDto(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetDecimal(3), reader.GetDateTime(4)));
                    }
                }
                connection.Close();
            }
            return reservations;
        }

        public int GetNewSavingId()
        {
            var savingId = 0;
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT MAX(SavingsId) FROM dbo.Savings", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        savingId = reader.GetInt32(0);
                    }
                }
                connection.Close();
            }
           return savingId;
        }

        public void CompleteSaving(ISaving saving)
        {
            var reservations = GetReservations(saving.SavingId).ToList();
            var i = 0;
            using (var connection = _connection.SqlConnection)
            {
                var commands = reservations.Select(reservation => new SqlCommand("INSERT INTO dbo.[Transaction] ([Name],Amount,TimeOfTransaction,AccountId,Category,IconId) VALUES('Saving Reservation',@Amount,@Time,@AccountId,'SavingPlan',@IconId)", connection)).ToList();
                foreach (var command in commands)
                {
                    command.Parameters.AddWithValue("@Amount", -reservations[i].Amount);
                    command.Parameters.AddWithValue("@Time", DateTime.Now);
                    command.Parameters.AddWithValue("@AccountId", reservations[i].AccountId);
                    command.Parameters.AddWithValue("IconId", 0);
                    i++;
                }
                connection.Open();
                foreach (var command in commands)
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            RemoveReservations(saving.SavingId);
        }

        public void RemoveReservations(int id)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("DELETE FROM  dbo.Reservations WHERE SavingsId = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public IReservation GetReservationById(int id)
        {
            var reservation = new ReservationDto();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT ReservationId,SavingsId,AccountId,Amount,Date FROM dbo.Reservations WHERE ReservationId = @Id",connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reservation.ReservationId = reader.GetInt32(0);
                        reservation.SavingsId = reader.GetInt32(1);
                        reservation.AccountId = reader.GetInt32(2);
                        reservation.Amount = reader.GetDecimal(3);
                        reservation.Date = reader.GetDateTime(4);
                    }
                }
                connection.Close();
            }
            return reservation;
        }

        public void DeleteReservation(int id)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("DELETE FROM dbo.Reservations WHERE ReservationId = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
