using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GoDaddyChatService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(InterfaceChatCallBack))] 
    public interface InterfaceServerChatService
    {
        [OperationContract]
        string Register(User user);

        [OperationContract]
        User Login(string username, string password);

        [OperationContract]
        string LogOut(string username);

        [OperationContract]
        string SendMessage(string sender,string reciever, string message);

        [OperationContract]
        String AddFriend(string user, string friend);
        
        [OperationContract]
        string RemoveFriend(string user, string friend);

        [OperationContract]
        string GetMessageHistory(string user, string friend);

    }

    public interface InterfaceChatCallBack
    {
        [OperationContract]
        void RecievMessage(String message);

        [OperationContract]
        void RecieveFriendList(List<User> friends);

        void UpdateFriendLits(User user);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "GoDaddyChatService.ContractType".
    [DataContract]
    public class User // Domæne 
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string userName { get; set;}
        [DataMember]
        public string password { get; set;}
        [DataMember]
        public string firstName { get; set;}
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public int status{ get; set; }

        public InterfaceChatCallBack channel { get; set; }

       
    }

    public class Message // Domæne 
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public User sender { get; set; }

    }


}
