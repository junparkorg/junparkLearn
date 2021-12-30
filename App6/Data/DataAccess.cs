using Microsoft.Data.SqlClient;

namespace App6.Data
{
    public class DataAccess
    {
        // test for code Security.
        private const string DataAccessKey = "server=1.1.1.1;uid=sa;pwd=NotAPassword;database=nodb";

        
        public async Task<SqlConnection> CreateConnection()
        {
            SqlConnection conn = new SqlConnection(DataAccessKey);
            await conn.OpenAsync();
            return conn;
        }
    }
}
