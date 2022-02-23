using Microsoft.Data.SqlClient;

namespace App6.Data
{
    // Code QL 테스트 용.
    public class DataAccess
    {
        // test for code Security.
        //private const string DataAccessKey = "server=211.11.12.13;uid=sa;pwd=NotAPassword;database=nodb";

        
        public async Task<SqlConnection> CreateConnection(string DataAccessKey)
        {
            SqlConnection conn = new SqlConnection(DataAccessKey);
            await conn.OpenAsync();
            return conn;
        }
    }
}
