using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoDaddyChatService.DomainObjects
{
    class FriendDomain
    {
        public int requesterID { get; set; }

        public int friendID { get; set; }

        public bool requestStatus { get; set; }
    }
}
