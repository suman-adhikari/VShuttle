using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Serialization;
using VShuttle.Models;

namespace VShuttle.Repository
{
    public class UserInfoRepository : Repo
    {
        public AjaxGridResult FindAll(int offset, int rowNumber, string sortExpression, string sortOrder, int pageNumber)
        {
            try
            {
                using (var con = new SqlConnection(connectionString)) {

                    var cmd = new SqlCommand(string.Format(@"
                         @Declare @page Int, @pagesize Int
                         select @page={0}, @pagesize={1};
                         with rownumber as(
                            select * from(
                              select RowNumber() over(order by {2} {3} ) as number,
                              name,location,sublocation,date from UserInfo
                           )tbl
                         )
                         select * from rownumber where rownumber.number between ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)",
                         pageNumber, rowNumber, sortExpression, sortOrder
                       ));
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter();
                    DataTable dataTable = new DataTable();
                    da.SelectCommand = cmd;
                    da.Fill(dataTable);

                    var jSerializer = new JavaScriptSerializer();

                    var myData = dataTable.AsEnumerable().Select(x => new
                    {
                        ID = x.Field<int>("ID"),
                        Name = x.Field<string>("name"),
                        Location = x.Field<int>("location"),
                        SubLocation = x.Field<string>("sublocation"),
                        Date = x.Field<DateTime?>("Date") != null ? string.Format("{0:yyyy/M/d HH:mm:ss}", x.Field<DateTime>("Date")) : ""
                    }).ToList();

                    var jsonData = jSerializer.Serialize(myData);

                    cmd = new SqlCommand("SELECT Count(*) FROM UserInfo", con);
                    var rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                    return new AjaxGridResult
                    {
                        Data = jsonData,
                        pageNumber = pageNumber,
                        RowCount = rowCount
                    };
                }

            } catch (Exception ex)
            {
                return new AjaxGridResult();
            }
        }

        public bool Save(UserInfo userInfo) {

            try {
                var query = "insert into UserInfo (name,location,sublocation,date) values(@name,@location,@sublocation,@sublocation)";
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@name", userInfo.Name);
                        cmd.Parameters.AddWithValue("@location", userInfo.Location);
                        cmd.Parameters.AddWithValue("@sublocation", userInfo.SubLocation);
                        cmd.Parameters.AddWithValue("@sublocation", DateTime.Now);

                        while (cmd.ExecuteReader().Read())
                            return true;
                    }
                
            }catch(Exception ex)
            {
                return false;
            }
            return false;

        }

        public bool Update(UserInfo userInfo)
        {
            try {
                string query = "update UserInfo set name=@name, location=@location, sublocation=@sublocation, date=@date where id=@id";
                using (var con = new SqlConnection(connectionString))               
                using (var cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@name", userInfo.Name);
                        cmd.Parameters.AddWithValue("@location", userInfo.Location);
                        cmd.Parameters.AddWithValue("@sublocation", userInfo.SubLocation);
                        cmd.Parameters.AddWithValue("@date", userInfo.Date);
                        cmd.Parameters.AddWithValue("@id", userInfo.Id);

                        while (cmd.ExecuteReader().Read())
                            return true;
                    }
                
            }catch(Exception ex)
            {
                return false;
            }
            return false;
        }


    }
}
