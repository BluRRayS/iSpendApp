using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.ContextInterfaces
{
    public interface IInvitationContext
    {
        void CreateInvite(IInvitation invitation);
        void DeleteInvite(int id);
        void AcceptInvite(int id);
        IEnumerable<IInvitation> GetUserInvitations(int userId);
        IInvitation GetInviteById(int id);
    }
}
