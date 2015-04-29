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
    class FriendAccessor
    {

        String connectionString;

        public FriendAccessor()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        }

        // hej // jej
        public List<FriendDomain> getFriends(int userID)
        {
            string query = "SELECT * FROM `comida-db`.friend_domain where userID = @USERID";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Prepare();
                cmd.Parameters.AddWithValue("@USERID", userID);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                return parseToFriendList(dataReader);
            }
        }

        public long addFriend(FriendDomain f)
        {
            string quary1 = "INSERT INTO `comida-db`.`friend_domain` (`userID`, `friendID`, `friendshipStatus`) VALUES (@USERID, @FRIENDID, @STATUS);";
           

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(quary1, conn);

                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@USERID", f.userID);
                    cmd.Parameters.AddWithValue("@FRIENDID", f.friendID);
                    cmd.Parameters.AddWithValue("@STATUS", f.status);
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

        public void acceptFriend(int requesterID, int userID) {

            string query = "UPDATE `comida-db`.`friend_domain` SET `friendshipStatus`= 1 WHERE requesterID = @REQUESTERID and friendID = @USERID";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@REQUESTERID", requesterID);
                    cmd.Parameters.AddWithValue("@USERID", userID);
                    cmd.ExecuteReader();
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.StackTrace);
            }
        }
        
        
        private List<FriendDomain> parseToFriendList(MySqlDataReader dataReader)
        {
            List<FriendDomain> friendList = new List<FriendDomain>();

            //Read the data and store them in the list
            while (dataReader.Read())
            {
                FriendDomain f = new FriendDomain()
                {
                    id = dataReader.GetInt32(0),
                    userID = dataReader.GetInt32(1),
                    friendID = dataReader.GetInt32(2),
                    status = dataReader.GetInt32(3)
                };
                friendList.Add(f);
            }
            return friendList;
        }


      

    }
}
