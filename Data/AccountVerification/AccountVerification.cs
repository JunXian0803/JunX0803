using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;

namespace Data
{
    public class AccountVerification : AccountVerificationBase
    {
        private readonly string _connectionString;

        public AccountVerification(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }
    }

}

