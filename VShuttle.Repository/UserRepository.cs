using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using VShuttle.Models;

namespace VShuttle.Repository
{
    public class UserRepository : Repo
    {

        public Users CheckUser(string username, string password) {

            return new Users
            {
                Id = 2,
                UserId="i10244",
                UserName="Suman",
                UserRole=2
            };
            using (var con = new SqlConnection(connectionString)) {
                con.Open();
                var query = "select * from Users where username=@username and password=@password";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Users user = new Users()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            UserName = Convert.ToString(reader["UserName"])
                        };
                        return user;
                    }
                }
                          
            }
                return null;
        }

        public bool RegisterUser(Users users)
        {
            try
            {
                string query = "insert into users(userid,username,password) values(@userid,@username,@password)";
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(query,con))
                {
                    cmd.Parameters.AddWithValue("@userid", users.UserId);
                    cmd.Parameters.AddWithValue("@username", users.UserName);
                    cmd.Parameters.AddWithValue("@password", users.Password);

                    if (cmd.ExecuteNonQuery() != -1)                   
                        return true;                   
                    return false;
                }

            }catch(Exception ex)
            {
                return false;
            }           
        }



    }
}
