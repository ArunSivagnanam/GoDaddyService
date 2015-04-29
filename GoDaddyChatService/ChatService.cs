﻿using GoDaddyChatService.DataAccess;
using GoDaddyChatService.DomainObjects;
using Service.DataBaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace GoDaddyChatService
{

    // USER STATUS
    // 1 = logged in
    // 0 = Ofline


    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ChatService : InterfaceServerChatService
    {

        public const int ONLINE = 1;
        public const int OFFLINE = 0;

        public const int FriendAcepted = 1;
        public const int FriendNotAcepted = 0;

        Dictionary<String, User> loggedInUsers =
                        new Dictionary<String, User>(); // key username (maaske skal users callback channel gemmes i user objected)

        UserAccessor userAccesor = new UserAccessor();
        FriendAccessor friendAccessor = new FriendAccessor();
        MessageAccessor messageAccessor = new MessageAccessor();

        [MethodImpl(MethodImplOptions.Synchronized)] // brug en ny tråd 
        
        public string Register(User user)
        {
            // 1) Opret ham i databasen
            long id = userAccesor.addUser(user);
            // TODO check om username er optaget

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
          

            User u = userAccesor.getUserByUsernameAndPassword(username, password);

            if (u == null || loggedInUsers.ContainsKey(u.userName))
            {
                return null;
            }
            else
            {
                // 2) Smid usernam + callbackchennel i dictionary loggedin user channels
                u.channel = GetCurrentCallBackChannel;
                loggedInUsers.Add(u.userName, u);
                u.status = ONLINE;
                Console.WriteLine("User logged in: " + u.firstName);

                // 3) Skaf usernames for brugerens venner fra databasen 
                List<FriendDomain> friendIDs = friendAccessor.getFriends(u.ID);
             
                foreach (FriendDomain f in friendIDs)
                {
                    if (f.status == FriendAcepted)
                    {
                        User friend = userAccesor.getUserByID(f.friendID);
 
                        if (loggedInUsers.ContainsKey(friend.userName))
                        {
                            // 4) Alle Users i dictionry loggedInUsers som brugeren er venner med, skal have kaldt metoden UpdateFriendLits(User user)
                            InterfaceChatCallBack friendChannel = loggedInUsers[friend.userName].channel;
                            friendChannel.UpdateFriendList(u);
                        }
                    }
               }
                   
                return u;
            }
        }

        // Denne metode skal kaldes efter login
        public List<User> ReceiveFriendList(string username)
        {
            List<FriendDomain> friendIDs = friendAccessor.getFriends(loggedInUsers[username].ID);
            List<User> friends = new List<User>(); // listen af venner som den loggede ind bruger skal have

            foreach (FriendDomain f in friendIDs)
            {
                User friend = userAccesor.getUserByID(f.friendID);
                friends.Add(friend);

                if (loggedInUsers.ContainsKey(friend.userName))
                {
                    friend.status = 1; // 1 = online
                }
                else
                {
                    friend.status = 0; // 0 = offline
                }
            }

            return friends;
        }

        // Denne metode skal kaldes efter login
        public List<User> ReceiveFriendsToAccept(string username)
        {
            User user = loggedInUsers[username];
            List<FriendDomain> friendIDs = friendAccessor.getFriendsToAccept(user.ID);
            List<User> friends = new List<User>();

            foreach (FriendDomain f in friendIDs)
            {
               User friend = userAccesor.getUserByID(f.userID);
               friends.Add(friend);
            }
            return friends;
        }

        public string LogOut(string username)
        {
            // 1) fjern ham fra dictionary
            loggedInUsers.Remove(username);
            return "SUCCESS";
        }

        public string SendMessage(Message m)
        {

            Console.WriteLine(m.sendMessageTime+" Forwarding a message to "+m.receiverUserName);
            // 1) Check om reciever er obline (er han i dictionary)

            if (loggedInUsers.ContainsKey(m.receiverUserName))
            {

                // 2) Skaf callback channel for reciver 
                InterfaceChatCallBack receiverChannel = loggedInUsers[m.receiverUserName].channel;

                try
                {
                    // 3) Kald RecieveMessage metoden på reciever
                    receiverChannel.RecievMessage(m.message);
                    // 5) Opdatere message history i db for sender og reciever
                    // message accessor.Add(besked med tid, og flag modtaget)
                    MessageDomain md = new MessageDomain()
                    {
                        receiverID = loggedInUsers[m.receiverUserName].ID, 
                        senderID = loggedInUsers[m.senderUserName].ID, 
                        message = m.message, 
                        sendMessageTime = m.sendMessageTime,
                        received = true
                    };

                    messageAccessor.addMessage(md);

                    return m.message;
                }
                catch (Exception e)
                {
                    // 4) Hvis beskeden modtages korrekt, send samme besked tilbage til sender
                    // ellers send fejl besked
                    return "ERROR";
                }
                
            }
            else
            {
                // læg besked op i pending messages i db
                // message accessor.Add(besked med tid, og flag ikke modtaget)
                User receiver = userAccesor.getUserByUserName(m.receiverUserName); // fra db da han ikke er online
                MessageDomain md = new MessageDomain()
                {
                    receiverID = receiver.ID, 
                    senderID = loggedInUsers[m.senderUserName].ID, 
                    message = m.message, 
                    sendMessageTime = m.sendMessageTime,
                    received = false
                };

                messageAccessor.addMessage(md);

                return "The user is not online but the mesage is saved in history";
            }
        }

        public String AddFriend(string userName, string friendUserName)
        {
            // 1) Check om friend eksistere i db og om de allerede er venner
            User friend = userAccesor.getUserByUserName(friendUserName);
            User user = loggedInUsers[userName];

            if (!(friendAccessor.checkFriend(friend.ID, user.ID)))
            { // de er ikke venner
                FriendDomain fd = new FriendDomain(user.ID, friend.ID, 0);
                friendAccessor.addFriend(fd);
                // opdatere pending friends på vennen hvis han er online
                if(loggedInUsers.ContainsKey(friend.userName)){
                    
                    InterfaceChatCallBack friendChannel = loggedInUsers[friend.userName].channel;
                    friendChannel.UpdatePendingFriendList(user);
                }
                return "Friend request sent";
            }
            else
            {
                return "Already friends";
            }

        }

        public String AcceptFriend(string friendToAcceptName, string username)
        {
            try
            {
                User friendToAccept = userAccesor.getUserByUserName(friendToAcceptName);
                User user = loggedInUsers[username];

                // update existerende row til status 1
                friendAccessor.setStatusForFriendRequest(user.ID, friendToAccept.ID);

                // add en ny row med status 1
                friendAccessor.finalizeFriendRequest(user.ID, friendToAccept.ID);

                // updatere userens friend list
                InterfaceChatCallBack userChannel = loggedInUsers[user.userName].channel;

                if (loggedInUsers.ContainsKey(friendToAccept.userName))
                {
                    friendToAccept.status = ONLINE;
                    InterfaceChatCallBack friendChannel = loggedInUsers[friendToAccept.userName].channel;
                    friendChannel.UpdateFriendList(user);
                }
                else
                {
                    friendToAccept.status = OFFLINE;
                }

                userChannel.UpdateFriendList(friendToAccept); // requester

                // fjern ham fra pending list
                userChannel.removeFromPendingList(friendToAccept);

                return "FRIEND ACCEPTED";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return "ERROR";
            }
           
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



        // PRIVATE METHODS NOT PART OF INTERFACE
        private InterfaceChatCallBack GetCurrentCallBackChannel
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<InterfaceChatCallBack>();
            }

        }

    }
}

// skaf current call back
// //OperationContext.Current.GetCallbackChannel<InterfaceChatCallBack>().RecievMessage("stat: " + i);