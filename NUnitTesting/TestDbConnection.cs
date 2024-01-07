using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTesting
{
    internal class TestDbConnection 
    {
        public TestDbConnection()
        {
            //A@123!23sda
            Connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");
        }

        public SqlConnection Connection { get; set; }
    }
}
