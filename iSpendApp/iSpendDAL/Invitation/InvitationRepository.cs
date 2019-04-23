using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendDAL.Invitation
{
    public class InvitationRepository
    {
        private readonly IInvitationContext _context;

        public InvitationRepository(IInvitationContext context)
        {
            _context = context;
        }

        public void CreateInvite(IInvitation invitation)
        {
           _context.CreateInvite(invitation);
        }

        public void DeleteInvite(int id)
        {
            _context.DeleteInvite(id);
        }

        public void AcceptInvite(int id)
        {
            _context.AcceptInvite(id);
        }
        public IEnumerable<IInvitation> GetUserInvitations(int userId)
        {
            return _context.GetUserInvitations(userId);
        }
    }
}
