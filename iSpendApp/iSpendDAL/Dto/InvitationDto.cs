using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.Dto
{
    internal class InvitationDto: IInvitation
    {
        public InvitationDto(int userIdSender, int userIdReceiver, int accountId, DateTime date)
        {
            UserIdSender = userIdSender;
            UserIdReceiver = userIdReceiver;
            AccountId = accountId;
            Date = date;
        }

        public int UserIdSender { get; set; }
        public int UserIdReceiver { get; set; }
        public int AccountId { get; set; }
        public DateTime Date { get; set; }
    }
}
