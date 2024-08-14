
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;

namespace Data
{
    public class vAccount : vAccountBase
    {
        private readonly string _connectionString;

        public vAccount(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }
    }

}

