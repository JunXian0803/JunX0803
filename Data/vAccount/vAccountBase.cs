using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;

namespace Data
{
    public class vAccountBase
    {
        private readonly string _connectionString;

        public vAccountBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected const string TABLE_COLUMN = "[ID], [AccountUID], [FirstName], [LastName], [ProfilePicture], [NRIC], [Email], [PhoneNumber], [Address], [IsActive], [IsDeleted]";

        public Model.vAccount BindData(IDataRecord data)
        {
            var item = new Model.vAccount();

            if (!data["ID"].Equals(DBNull.Value))
            {
                item.ID = (long)data["ID"];
            }

            if (!data["ACCOUNTUID"].Equals(DBNull.Value))
            {
                item.AccountUID = (Guid)data["ACCOUNTUID"];
            }

            if (!data["FIRSTNAME"].Equals(DBNull.Value))
            {
                item.FirstName = (string)data["FIRSTNAME"];
            }

            if (!data["LASTNAME"].Equals(DBNull.Value))
            {
                item.LastName = (string)data["LASTNAME"];
            }

            if (!data["PROFILEPICTURE"].Equals(DBNull.Value))
            {
                item.ProfilePicture = (string)data["PROFILEPICTURE"];
            }

            if (!data["NRIC"].Equals(DBNull.Value))
            {
                item.NRIC = (string)data["NRIC"];
            }

            if (!data["EMAIL"].Equals(DBNull.Value))
            {
                item.Email = (string)data["EMAIL"];
            }

            if (!data["PHONENUMBER"].Equals(DBNull.Value))
            {
                item.PhoneNumber = (string)data["PHONENUMBER"];
            }

            if (!data["ADDRESS"].Equals(DBNull.Value))
            {
                item.Address = (string)data["ADDRESS"];
            }

            if (!data["ISACTIVE"].Equals(DBNull.Value))
            {
                item.IsActive = (bool?)data["ISACTIVE"];
            }

            if (!data["ISDELETED"].Equals(DBNull.Value))
            {
                item.IsDeleted = (bool?)data["ISDELETED"];
            }

            return item;

        }

        public List<Model.vAccount> Select(string condition, string orderBy, int limit = 0, int offset = 0)
        {
            var list = new List<Model.vAccount>();

            var query = "SELECT " + TABLE_COLUMN + " FROM [DBO].[VACCOUNT]";

            if (condition != string.Empty) query += " WHERE " + condition;
            if (orderBy != string.Empty || limit > 0) query += " ORDER BY " + (orderBy == string.Empty ? "ID DESC" : orderBy);
            if (limit > 0) query += " OFFSET " + offset + " ROWS FETCH NEXT " + limit + " ROWS ONLY";

            using (var db = new SqlManager(_connectionString))
            {
                
                using (var reader = db.ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        var item = BindData(reader);
                        list.Add(item);
                    }
                }
            }

            return list;

        }

        public DataTable SelectDataTable(string columns, string condition, string orderBy, int limit = 0, int offset = 0)
        {

            var query = "SELECT " + (columns == string.Empty ? TABLE_COLUMN : columns) + " FROM [DBO].[VACCOUNT]";

            if (condition != string.Empty) query += " WHERE " + condition;
            if (orderBy != string.Empty || limit > 0) query += " ORDER BY " + (orderBy == string.Empty ? "ID DESC" : orderBy);
            if (limit > 0) query += " OFFSET " + offset + " ROWS FETCH NEXT " + limit + " ROWS ONLY";

            using (var db = new SqlManager(_connectionString))
            {
                return db.ExecuteDataTable(query);
            }
        }

        public int GetRecordCount(string condition)
        {
            var query = "SELECT COUNT(*) FROM [DBO].[VACCOUNT]";

            if (condition != string.Empty) query += " WHERE " + condition;

            using (var db = new SqlManager(_connectionString))
            {
                return (int)db.ExecuteScalar(query);
            }
        }

        public object GetSumTotal(string column, string condition)
        {
            var query = "SELECT ISNULL(SUM(" + column + "),0) FROM [DBO].[VACCOUNT]";

            if (condition != string.Empty) query += " WHERE " + condition;

            using (var db = new SqlManager(_connectionString))
            {
                return db.ExecuteScalar(query);
            }
        }


    }

}

