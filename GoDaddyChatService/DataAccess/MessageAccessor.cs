using GoDaddyChatService.DomainObjects;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoDaddyChatService.DataAccess
{
    class MessageAccessor
    {

        String connectionString;

        public MessageAccessor()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        }


        public List<MessageDomain> getMessageHistory(int senderID, int receiverID){

            string query = "SELECT * FROM `comida-db`.message_domain where (senderID = @SENDERID and receiverID = @RECEIVERID) or (senderID = @RECEIVERID and receiverID = @SENDERID) ORDER BY sendMessageTime ASC";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Prepare();
                cmd.Parameters.AddWithValue("@SENDERID", senderID);
                cmd.Parameters.AddWithValue("@RECEIVERID", receiverID);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                return parseToMessageList(dataReader);
            }

        }

        public long addMessage(MessageDomain m)
        {
            string quary = "INSERT INTO `comida-db`.`message_domain` (`senderID`, `receiverID`, `sendMessageTime`, `message`, `received`)" +
                "VALUES (@SENDERID, @RECEIVERID, @SENDMESSAGETIME, @MESSAGE, @RECEIVED);";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(quary, conn);

                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@SENDERID", m.senderID);
                    cmd.Parameters.AddWithValue("@RECEIVERID", m.receiverID);
                    cmd.Parameters.AddWithValue("@SENDMESSAGETIME", m.sendMessageTime);
                    cmd.Parameters.AddWithValue("@MESSAGE", m.message);
                    cmd.Parameters.AddWithValue("@RECEIVED", m.received);
                    cmd.ExecuteReader();

                    return cmd.LastInsertedId;
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.StackTrace);
                return -1;
            }
        }



        private List<MessageDomain> parseToMessageList(MySqlDataReader dataReader)
        {
            List<MessageDomain> userList = new List<MessageDomain>();

            //Read the data and store them in the list
            while (dataReader.Read())
            {
                MessageDomain m = new MessageDomain()
                {
                    messageID = dataReader.GetInt32(0),
                    senderID = dataReader.GetInt32(1),
                    receiverID = dataReader.GetInt32(2),
                    sendMessageTime = dataReader.GetDateTime(3),
                    message = dataReader.GetString(4),
                    received = dataReader.GetBoolean(5)
                };
                userList.Add(m);
            }
            return userList;
        }

    }
}
