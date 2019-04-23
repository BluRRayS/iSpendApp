using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSpendInterfaces;

namespace iSpendWebApp.Models
{
    public class InvitationViewModel: IInvitation
    {
        public InvitationViewModel()
        {
            
        }

        public InvitationViewModel(IInvitation invitation)
        {
            InvitationId = invitation.InvitationId;
            UserIdReceiver = invitation.UserIdReceiver;
            UserIdSender = invitation.UserIdSender;
            Date = invitation.Date;
            AccountId = invitation.AccountId;
        }

        public int InvitationId { get; set; }
        public int UserIdSender { get; set; }
        public int UserIdReceiver { get; set; }
        public int AccountId { get; set; }
        public DateTime Date { get; set; }
    }
}
