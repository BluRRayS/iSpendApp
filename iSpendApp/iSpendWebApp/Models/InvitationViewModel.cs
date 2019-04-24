using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using iSpendInterfaces;

namespace iSpendWebApp.Models
{
    public class InvitationViewModel : IInvitation
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
        [Display(Name = "Sender")]

        public int UserIdSender { get; set; }
        [Display(Name = "Receiver")]
        [Required]
        public int UserIdReceiver { get; set; }
        [Display(Name = "Account")]
        [Required]
        public int AccountId { get; set; }
        public DateTime Date { get; set; }
    }
}
