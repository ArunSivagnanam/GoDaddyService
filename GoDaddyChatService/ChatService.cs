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


        Dictionary<String, InterfaceChatCallBack> loggedInUser =
                 new Dictionary<String, InterfaceChatCallBack>();
         

        public string Register(User user)
        {
            // opret ham i db
            return "";
        }

        public string Login(string username, string password)
        {
            // smid usernam + callback i dictionary

            // den loggede ind bruger skal have kalt metoden -> receve friend list(usernames)

            // alle loggede ind bruger i dict -> update friend list (username) hvis de er venner


            return ";";
        }

        string SendMessage(string username, string message)
        {
            // skaf callback fro username fra dict

            // call -> recievMessage metoden

            // hvis han ikke kan kontaktest send feedback
            // ellers retunere samme besked

            return "";
        }

        String AddFriend(string friend)
        {
            // check om bruger eksistere og om de allerede er venner
            
            // update db venneliste for uswername (dict look up) med flag false
            // 
            // kald update friend list(username) og friend

            // for username skal den retunerede User's status være ikke accepteret 

            return "";
        }
    }
}

/*
 [OperationContract]
        string Register(User username);

        

        [OperationContract]
        String AddFriend(string username);
        
        [OperationContract]
        string RemoveFriend(string username);

        [OperationContract]
        string GetMessageHistory(string username);
*/

// skaf call back
// //OperationContext.Current.GetCallbackChannel<InterfaceChatCallBack>().RecievMessage("stat: " + i);