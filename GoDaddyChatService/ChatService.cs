using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GoDaddyChatService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class ChatService : IChatInterface
    {

        ServiceModel model = new ServiceModel();

        public string Register(User user)
        {
            model.registerUser(user.name);
            return user.name + " registered";
        }

        public string Login(User user)
        {
            model.loginUser(user.name);
            return user.name + " logged in";
        }
        
       
    }
}
