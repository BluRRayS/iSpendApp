using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Dto;
using iSpendInterfaces;

namespace iSpendDAL.Invitation
{
    public class InvitationContext: IInvitationContext
    {
        private readonly DatabaseConnection _connection;

        public InvitationContext(DatabaseConnection connection)
        {
            _connection = connection;
        }

        public void CreateInvite(IInvitation invitation)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("INSERT INTO dbo.Invitations (UserIdSender,UserIdReceiver,[Date],AccountId) VALUES (@Sender,@Receiver,@Date,@AccountId)", connection);
                command.Parameters.AddWithValue("@AccountId", invitation.AccountId);
                command.Parameters.AddWithValue("@Receiver", invitation.UserIdReceiver);
                command.Parameters.AddWithValue("@Sender", invitation.UserIdSender);
                command.Parameters.AddWithValue("@Date", invitation.Date);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void DeleteInvite(int id)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("DELETE FROM dbo.Invitations WHERE InvitationId = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void AcceptInvite(int id)
        {
            var invite = GetInviteById(id);
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("INSERT INTO User_Account (UserId,AccountId) VALUES (@User,@Account)", connection);
                command.Parameters.AddWithValue("@User", invite.UserIdReceiver);
                command.Parameters.AddWithValue("@Account", invite.AccountId);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            DeleteInvite(id);      
        }

        public IEnumerable<IInvitation> GetUserInvitations(int userId)
        {
            var invites = new List<InvitationDto>();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT InvitationId,AccountId,UserIdSender,UserIdReceiver,[Date] FROM dbo.Invitations WHERE UserIdReceiver = @Id ", connection);
                command.Parameters.AddWithValue("@Id", userId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        invites.Add(new InvitationDto(reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(1), reader.GetDateTime(4), reader.GetInt32(0)));
                    }
                }
                connection.Close();
            }
            return invites;
        }

        public IInvitation GetInviteById(int id)
        {
            var invite = new InvitationDto();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT InvitationId,AccountId,UserIdSender,UserIdReceiver,[Date] FROM dbo.Invitations WHERE InvitationId = @Id ", connection);
                command.Parameters.AddWithValue("Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        invite.InvitationId = reader.GetInt32(0);
                        invite.AccountId = reader.GetInt32(1);
                        invite.UserIdSender = reader.GetInt32(2);
                        invite.UserIdReceiver = reader.GetInt32(3);
                        invite.Date = reader.GetDateTime(4);
                    }
                }
                connection.Close();
            }
            return invite;
        }
    }
}
