using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Invitation;

namespace iSpendLogic
{
    public class InvitationLogic
    {
        private InvitationRepository Repository { get; }

        public InvitationLogic(IInvitationContext context)
        {
            Repository = new InvitationRepository(context);
        }

        public void CreateInvite()
        {
            Repository.CreateInvite();
        }

        public void DeleteInvite()
        {
            Repository.DeleteInvite();
        }

        public void AcceptInvite()
        {
            Repository.AcceptInvite();
        }
    }
}
