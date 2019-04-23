using System;
using System.Collections.Generic;
using System.Text;

namespace iSpendInterfaces
{
    public interface IInvitation
    {
        int InvitationId { get; set; }
        int UserIdSender { get; set; }
        int UserIdReceiver { get; set; }
        int AccountId { get; set; }
        DateTime Date { get; set; }
    }
}
