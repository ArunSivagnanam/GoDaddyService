using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace GoDaddyChatService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ChatService : InterfaceServerChatService
    {

        ServiceModel model = new ServiceModel();

        public string Register(User user)
        {
            model.registerUser(user.name);

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(50);
                OperationContext.Current.GetCallbackChannel<InterfaceChatCallBack>().RecievMessage("stat: " + i);
            }

                return user.name + " registered";
        }

        public string Login(User user)
        {
            model.loginUser(user.name);
            return user.name + " logged in";
        }
        
       
    }
}
