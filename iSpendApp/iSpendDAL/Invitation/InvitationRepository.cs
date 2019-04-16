using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;

namespace iSpendDAL.Invitation
{
    public class InvitationRepository
    {
        private readonly IInvitationContext _context;

        public InvitationRepository(IInvitationContext context)
        {
            _context = context;
        }
    }
}
