using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoDaddyChatService.DomainObjects
{
    class FriendDomain
    {

        public int id { get; set; }
        public int userID { get; set; }

        public int friendID { get; set; }

        public int status { get; set; }
    }
}
