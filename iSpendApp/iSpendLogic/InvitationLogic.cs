using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Invitation;
using iSpendInterfaces;

namespace iSpendLogic
{
    public class InvitationLogic
    {
        private InvitationRepository Repository { get; }

        public InvitationLogic(IInvitationContext context)
        {
            Repository = new InvitationRepository(context);
        }

        public void CreateInvite(IInvitation invitation)
        {
            Repository.CreateInvite(invitation);
        }

        public void DeleteInvite(int id)
        {
            Repository.DeleteInvite(id);
        }

        public void AcceptInvite(int id)
        {
            Repository.AcceptInvite(id);
        }

        public IEnumerable<IInvitation> GetUserInvitations(int userId)
        {
           return Repository.GetUserInvitations(userId);
        }
    }
}
