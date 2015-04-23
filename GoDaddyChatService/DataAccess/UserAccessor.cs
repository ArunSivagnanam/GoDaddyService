﻿using GoDaddyChatService.DomainObjects;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using GoDaddyChatService;
using System.Diagnostics;

namespace Service.DataBaseAccess
{
    public class UserAccessor
    {

        String connectionString;

        public UserAccessor()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        }

        public List<User> getAllUsers()
        {
            string quary = "SELECT * FROM `comida-db`.user_domain;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(quary, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                return passToUserList(dataReader);
            }

        }


        public User getUserByUsernameAndPassword(string username, string password)
        {
            string query = "SELECT * FROM `comida-db`.user_domain where userName = @USERNAME and userPassword = @PASSWORD;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Prepare();
                cmd.Parameters.AddWithValue("@USERNAME", username);
                cmd.Parameters.AddWithValue("@PASSWORD", password);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                return passToUserList(dataReader).First();
            }

        }

        public long addUser(User u)
        {
            string quary = "insert into `comida-db`.`user_domain` (userName,userPassword,firstName,lastName,userStatus)"+
                    "values (@USERNAME,@PASSWORD, @FIRSTNAME,@LASTNAME ,@USERSTATUS)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(quary, conn);

                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@USERNAME", u.firstName);
                    cmd.Parameters.AddWithValue("@PASSWORD", u.password);
                    cmd.Parameters.AddWithValue("@FIRSTNAME", u.firstName);
                    cmd.Parameters.AddWithValue("@LASTNAME", u.lastName);
                    cmd.Parameters.AddWithValue("@USERSTATUS", u.status);
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

        private List<User> passToUserList(MySqlDataReader dataReader)
        {
            List<User> userList = new List<User>();

            //Read the data and store them in the list
            while (dataReader.Read())
            {
                User u = new User()
                {
                    ID = dataReader.GetInt32(0),
                    userName = dataReader.GetString(1),
                    password = dataReader.GetString(2),
                    firstName = dataReader.GetString(3),
                    lastName = dataReader.GetString(4),
                    status = dataReader.GetInt32(5)
                };
                userList.Add(u);
            }
            return userList;
        }


        public User passToUserDomain(User user)
        {

            return new User()
            {
                ID = user.ID,
                userName = user.userName,
                password = user.password,
                firstName = user.firstName,
                lastName = user.lastName,
                status = user.status
            };
        }

    }
}