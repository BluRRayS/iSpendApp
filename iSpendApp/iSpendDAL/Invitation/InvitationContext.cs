using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;

namespace iSpendDAL.Invitation
{
    internal class InvitationContext: IInvitationContext
    {
        private readonly DatabaseConnection _connection;

        public InvitationContext(DatabaseConnection connection)
        {
            _connection = connection;
        }

        public void CreateInvite()
        {
            throw new NotImplementedException();
        }

        public void DeleteInvite()
        {
            throw new NotImplementedException();
        }

        public void AcceptInvite()
        {
            throw new NotImplementedException();
        }
    }
}
