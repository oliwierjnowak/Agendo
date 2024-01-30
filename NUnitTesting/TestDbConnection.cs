using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTesting
{
    // Singleton 
    internal class TestDbConnection 
    {
        private static TestDbConnection _instance;
        private TestDbConnection()
        {
            //A@123!23sda
            _connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");
        }
        
        private SqlConnection _connection;
        public  SqlConnection Connection {  get { return _connection; } }

        public static TestDbConnection GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TestDbConnection();
            }
            return _instance;
        }
    }
}
