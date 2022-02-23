using Microsoft.Data.SqlClient;

namespace App6.Data
{
    // Code QL 테스트 용.
    public class DataAccess
    {
        // test for code Security.
        private const string DataAccessKey = "server=1.1.1.1;uid=sa;pwd=NotAPassword;database=nodb";

        
        public async task<sqlconnection> createconnection()
        {
            sqlconnection conn = new sqlconnection(dataaccesskey);
            await conn.openasync();
            return conn;
        }
    }
}
