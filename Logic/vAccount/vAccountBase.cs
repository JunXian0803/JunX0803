using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Logic
{
    public class vAccountBase
    {
        private readonly string _connectionString;

        public vAccountBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Model.vAccount GetvAccount(long id)
        {
            var list = new Data.vAccount(_connectionString).Select("ID = " + MyFunction.CSQL(id.ToString()), string.Empty);
            if (list.Count > 0) return list[0];
            return null;
        }


        public DataTable GetDataTable()
        {
            return GetDataTable(string.Empty, string.Empty, string.Empty);
        }

        public DataTable GetDataTable(string columns, string condition, string orderBy, int limit = 0, int offset = 0)
        {
            return new Data.vAccount(_connectionString).SelectDataTable(columns, condition, orderBy, limit, offset);
        }
    }
}

