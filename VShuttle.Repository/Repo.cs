using System.Configuration;

namespace VShuttle.Repository
{
    public class Repo
    {
        public string connectionString;

        public string connection() {
            connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
            return connectionString;
        }
    }
}
