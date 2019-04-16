using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace iSpendDAL.ContextInterfaces
{
    public interface IInvitationContext
    {
        void CreateInvite();
        void DeleteInvite();
        void AcceptInvite();
    }
}
