using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoDaddyChatService.DomainObjects
{
    class MessageDomain
    {
        public int messageID { get; set; }

        public int senderID { get; set; }

        public int receiverID { get; set; }

        public Nullable<System.DateTime> sendMessageTime { get; set; }

        public string message { get; set; }

        public bool received { get; set; }
    }
}
