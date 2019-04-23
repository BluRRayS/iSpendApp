using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.Dto
{
    internal class InvitationDto: IInvitation
    {
        public InvitationDto()
        {
            
        }
        public InvitationDto(int userIdSender, int userIdReceiver, int accountId, DateTime date, int invitationId)
        {
            InvitationId = invitationId;
            UserIdSender = userIdSender;
            UserIdReceiver = userIdReceiver;
            AccountId = accountId;
            Date = date;
        }

        public int InvitationId { get; set; }
        public int UserIdSender { get; set; }
        public int UserIdReceiver { get; set; }
        public int AccountId { get; set; }
        public DateTime Date { get; set; }
    }
}
