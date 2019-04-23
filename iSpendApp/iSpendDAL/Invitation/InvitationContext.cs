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
            var command = new SqlCommand("INSERT INTO dbo.Invitations (UserIdSender,UserIdReceiver,[Date],AccountId) VALUES (@Sender,@Receiver,@Date,@AccountId)", _connection.SqlConnection);
            command.Parameters.AddWithValue("@AccountId", invitation.AccountId);
            command.Parameters.AddWithValue("@Receiver", invitation.UserIdReceiver);
            command.Parameters.AddWithValue("@Sender", invitation.UserIdSender);
            command.Parameters.AddWithValue("@Date", invitation.Date);
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public void DeleteInvite(int id)
        {
            var command = new SqlCommand("DELETE FROM dbo.Invitations WHERE InvitationId = @Id", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public void AcceptInvite(int id)
        {
            var invite = GetInviteById(id);
            var command = new SqlCommand("INSERT INTO User_Account (UserId,AccountId) VALUES (@User,@Account)", _connection.SqlConnection);
            command.Parameters.AddWithValue("@User",invite.UserIdReceiver);
            command.Parameters.AddWithValue("@Account", invite.AccountId);
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
            DeleteInvite(id);
        }

        public IEnumerable<IInvitation> GetUserInvitations(int userId)
        {
            var invites = new List<InvitationDto>();
            var command = new SqlCommand("SELECT InvitationId,AccountId,UserIdSender,UserIdReceiver,[Date] FROM dbo.Invitations WHERE UserIdReceiver = @Id ", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", userId);
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    invites.Add(new InvitationDto(reader.GetInt32(2),reader.GetInt32(3),reader.GetInt32(1),reader.GetDateTime(4),reader.GetInt32(0)));
                }
            }
            _connection.SqlConnection.Close();
            return invites;
        }

        public IInvitation GetInviteById(int id)
        {
            var invite = new InvitationDto();
            var command = new SqlCommand("SELECT InvitationId,AccountId,UserIdSender,UserIdReceiver,[Date] FROM dbo.Invitations WHERE InvitationId = @Id ", _connection.SqlConnection);
            command.Parameters.AddWithValue("Id", id);
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
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
            _connection.SqlConnection.Close();
            return invite;
        }
    }
}
