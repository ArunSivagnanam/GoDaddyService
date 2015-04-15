using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoDaddyChatService
{
    class ServiceModel
    {

        private static List<string> registeredUsers = new List<string>();

        private static List<string> loggedInUsers = new List<string>();
        

        public void registerUser(string userName)
        {
            registeredUsers.Add(userName);
        }

        public void loginUser(string userName)
        {
            loggedInUsers.Add(userName);
        }

        public List<string> GetUserList()
        {
            return registeredUsers;
        }

    }
}
