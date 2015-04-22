﻿using Service.DataBaseAccess;
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


        Dictionary<String, InterfaceChatCallBack> loggedInUserChannels =
                        new Dictionary<String, InterfaceChatCallBack>(); // key username

        Dictionary<String, User> loggedInUsers =
                        new Dictionary<String, User>(); // key username (maaske skal users callback channel gemmes i user objected)

        UserAccessor userAccesor = new UserAccessor();


        public string Register(User user)
        {
            // 1) Opret ham i databasen
            long id = userAccesor.addUser(userAccesor.passToUserDomain(user));

            if (id != -1)
            {
                return "SUCCESS";
            }else{
                return "FAIL";
            }
           
        }

        public User Login(string username, string password)
        {
            // 1) Hent bruger fra databasen ud fra username og password hvis han eksistere
            
            // 2) Smid usernam + callbackchennel i dictionary loggedin user channels

            // 3) Skaf usernames for brugerens venner fra databasen
            
            // 4) Kald RecieveFriendList(List<User>) metoden på brugeren som vil logge ind og giv ham listen af venner i form af User objecter fra dict

            // 5) Alle Users i dictionry loggedInUsers som brugeren er venner med, skal have kaldt metoden UpdateFriendLits(User user)

            return null;
        }

        public string LogOut(string username)
        {
            // 1) fjern ham fra dictionary
            return "somthing cool";
        }

        public string SendMessage(string sender ,string reciever, string message)
        {
            // 1) Check om reciever er obline (er han i dictionary)

            // 2) Skaf callback channel for reciver og sender
            
            // 3) Kald RecieveMessage metoden på reciever

            // 4) Hvis beskeden modtages korrekt, send samme besked tilbage til sender
                    // ellers send fejl besked

            // 5) Opdatere message history i db for sender og reciever

            return "";
        }

        public String AddFriend(string user, string friend)
        {
            // 1) Check om friend eksistere i db og om de allerede er venner
            
            // 2) opdatere db venneliste for user og friend med flag accepted false

            // 3) Kald UpdateFriendLits(User friend) for user, men sæt friend status til "not accepted"

            // 4) Hvis friend er online kald UpdateFriendLits(User user) på friend, friend skal derefter acceptere user
            
            // 5) Hvis friend ikke er online, skal han acceptere user neste gang han logger ind. 

            return "";
        }


        public string RemoveFriend(string user, string friend)
        {
            // 1) Check om friend eksistere i db og om de allerede er venner

            // 2) opdatere db venneliste for user og friend ved at fjerne rækkerne  eller set flag til blocked

            // 3) Kald metoden RecieveFriendList(List<User>)

            // 4) Hvis friend er online gør det samme for friend

            return "";
        }

        public string GetMessageHistory(string user, string friend)
        {

            // 1) Skaf callback channel for user

            // 2) Skaf message history for reciever = user og sender = friend og send 

            // 3) retunere svaret som formateret string eller en liste af stringe eller noget

            return "";
        }

    }
}

// skaf current call back
// //OperationContext.Current.GetCallbackChannel<InterfaceChatCallBack>().RecievMessage("stat: " + i);