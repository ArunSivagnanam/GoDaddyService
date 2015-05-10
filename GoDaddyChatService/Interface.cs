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
        List<User> ReceiveFriendList(string username);
        [OperationContract]
        List<User> ReceiveFriendsToAccept(string username);

        [OperationContract]
        string LogOut(string username);

        [OperationContract]
        string SendMessage(Message m);

        [OperationContract]
        String AddFriend(string userName, string friendName);

        [OperationContract]
        String AcceptFriend(string requesterName, string userName);
        
        [OperationContract]
        string RemoveFriend(string user, string friend);

        [OperationContract]
        List<Message> GetMessageHistory(string user, string friend);

    }
    
    public interface InterfaceChatCallBack
    {
        [OperationContract]
        void RecievMessage(Message message);

        [OperationContract]
        void UpdateFriendList(User user);

        [OperationContract] 
        void UpdateFriendListRemove(User user);

        [OperationContract]
        void UpdateFriendsToAcceptList(User user);

        [OperationContract]
        void removeFromPendingList(User user);
           
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "GoDaddyChatService.ContractType".

     [DataContract(Name = "Availability")]
    public enum Availability {

         [EnumMember]
         Online,
         [EnumMember]
         Offline,
         [EnumMember]
         FriendRequest };

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
        public Availability Status { get; set; }

        public InterfaceChatCallBack channel { get; set; }

        [OperationContract]
        
        public override string ToString()
        {
            return userName;
        }
    }

    public class Message // Domæne 
    {
        

        [DataMember]
        public string senderUserName { get; set; }

        [DataMember]
        public string receiverUserName { get; set; }
        [DataMember]
        public Nullable<System.DateTime> sendMessageTime { get; set; }
        
        [DataMember]
        public string message { get; set; }
    }


}
