using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Helper;

namespace Logic
{
    public class vAccount : vAccountBase
    {
        private readonly string _connectionString;

        public vAccount(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetvAccount(long IdentifierID)
        {
            var condition = String.Format("[IdentifierID] = {0}", MyFunction.CSQL(IdentifierID.ToString()));

            return new Data.vAccount(_connectionString).SelectDataTable(string.Empty, condition, string.Empty);
        }

    }
}

