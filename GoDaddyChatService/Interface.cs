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
        string Register(User username);

        [OperationContract]
        string Login(User username);

    }

    public interface InterfaceChatCallBack
    {
        [OperationContract]
        void RecievMessage(String message);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "GoDaddyChatService.ContractType".
    [DataContract]
    public class User
    {
        private string userName;
       
        [DataMember]
        public string name
        {
            get { return this.userName; }
            set { this.userName = value; }
        }
    }
}
