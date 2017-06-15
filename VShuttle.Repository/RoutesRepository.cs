using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShuttle.Model;

namespace VShuttle.Repository
{
    public class RoutesRepository : Repo
    {
        public bool UpdateRoutes(int id, string routelocation)
        {
            try
            {
                string query = "Update Routes set RouteLocations=@routelocation where id=@id";
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(query,con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@routelocation",routelocation);
                    cmd.Parameters.AddWithValue("@id",id);
                    while (cmd.ExecuteNonQuery() != -1)
                    {
                        return true;
                    }
                    return false;
                }

            }catch(Exception ex)
            {
                return false;
            }          
        } 

        public List<Routes> FindAll()
        {
            try
            {
                string query = "select * from Routes";
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(query,con))
                {
                    List<Routes> routes= new List<Routes>();
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var data = new Routes
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            RouteLocations = Convert.ToString(dr["RouteLocations"])
                        };
                        routes.Add(data);
                    }
                    return routes;
                }  

            }catch(Exception ex)
            {
                return null;
            }
           
        }

    }
}
