using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;

namespace Data
{
    public class AccountVerificationBase
    {
        private readonly string _connectionString;

        public AccountVerificationBase(string connectionString)
        {
            _connectionString = connectionString;
        }


        protected const string TABLE_COLUMN = "[ID], [IdentifierID], [AccountID], [Category], [Code], [RequestDate], [ExpiryDate], [SendResult]";

        public long Insert(Model.AccountVerification accountVerification)
        {
            string query = "DECLARE @TEMPID TABLE(ID BIGINT); INSERT INTO [DBO].[ACCOUNTVERIFICATION] ([ACCOUNTID], [CATEGORY], [CODE], [REQUESTDATE], [EXPIRYDATE], [SENDRESULT]) OUTPUT INSERTED.ID INTO @TEMPID VALUES (@ACCOUNTID, @CATEGORY, @CODE, @REQUESTDATE, @EXPIRYDATE, @SENDRESULT); SELECT * FROM @TEMPID";
            long id = -1;

            using (var db = new SqlManager(_connectionString))
            {
                try
                {
                    db.BeginTransaction();

                    db.Parameters.Clear();
                    db.Parameters.Add(new SqlParameter("@AccountID", accountVerification.AccountID));
                    db.Parameters.Add(new SqlParameter("@Category", MyFunction.CheckNull(accountVerification.Category)));
                    db.Parameters.Add(new SqlParameter("@Code", MyFunction.CheckNull(accountVerification.Code)));
                    db.Parameters.Add(new SqlParameter("@RequestDate", MyFunction.CheckNull(accountVerification.RequestDate)));
                    db.Parameters.Add(new SqlParameter("@ExpiryDate", MyFunction.CheckNull(accountVerification.ExpiryDate)));
                    db.Parameters.Add(new SqlParameter("@SendResult", MyFunction.CheckNull(accountVerification.SendResult)));

                    id = (long)db.ExecuteScalar(query);

                    db.Commit();
                }
                catch (SqlException)
                {
                    db.Rollback();
                    throw;
                }
            }
            return id;
        }

        public int Update(Model.AccountVerification accountVerification)
        {
            string query = "UPDATE [DBO].[ACCOUNTVERIFICATION] SET [ACCOUNTID] = @ACCOUNTID, [CATEGORY] = @CATEGORY, [CODE] = @CODE, [REQUESTDATE] = @REQUESTDATE, [EXPIRYDATE] = @EXPIRYDATE, [SENDRESULT] = @SENDRESULT WHERE ID = @ID";
            int affected = -1;

            using (var db = new SqlManager(_connectionString))
            {
                try
                {
                    db.BeginTransaction();

                    db.Parameters.Clear();
                    db.Parameters.Add(new SqlParameter("@ID", accountVerification.ID.ToString()));
                    db.Parameters.Add(new SqlParameter("@AccountID", accountVerification.AccountID));
                    db.Parameters.Add(new SqlParameter("@Category", MyFunction.CheckNull(accountVerification.Category)));
                    db.Parameters.Add(new SqlParameter("@Code", MyFunction.CheckNull(accountVerification.Code)));
                    db.Parameters.Add(new SqlParameter("@RequestDate", MyFunction.CheckNull(accountVerification.RequestDate)));
                    db.Parameters.Add(new SqlParameter("@ExpiryDate", MyFunction.CheckNull(accountVerification.ExpiryDate)));
                    db.Parameters.Add(new SqlParameter("@SendResult", MyFunction.CheckNull(accountVerification.SendResult)));

                    affected = db.ExecuteNonQuery(query);

                    db.Commit();
                }
                catch (SqlException)
                {
                    db.Rollback();
                    throw;
                }
            }
            return affected;
        }

        public int Delete(long id)
        {
            string query = "DELETE [DBO].[ACCOUNTVERIFICATION] WHERE ID = @ID;";
            int affected = -1;

            using (var db = new SqlManager(_connectionString))
            {
                try
                {
                    db.BeginTransaction();
                    db.Parameters.Add(new SqlParameter("@ID", id.ToString()));
                    affected = db.ExecuteNonQuery(query);
                    db.Commit();
                }
                catch (SqlException)
                {
                    db.Rollback();
                    throw;
                }
            }
            return affected;
        }

        public Model.AccountVerification BindData(IDataRecord data)
        {
            var item = new Model.AccountVerification();

            if (!data["ID"].Equals(DBNull.Value))
            {
                item.ID = (long)data["ID"];
            }

            if (!data["IDENTIFIERID"].Equals(DBNull.Value))
            {
                item.IdentifierID = (Guid)data["IDENTIFIERID"];
            }

            if (!data["ACCOUNTID"].Equals(DBNull.Value))
            {
                item.AccountID = (long)data["ACCOUNTID"];
            }

            if (!data["CATEGORY"].Equals(DBNull.Value))
            {
                item.Category = (string)data["CATEGORY"];
            }

            if (!data["CODE"].Equals(DBNull.Value))
            {
                item.Code = (string)data["CODE"];
            }

            if (!data["REQUESTDATE"].Equals(DBNull.Value))
            {
                item.RequestDate = (DateTime)data["REQUESTDATE"];
            }

            if (!data["EXPIRYDATE"].Equals(DBNull.Value))
            {
                item.ExpiryDate = (DateTime)data["EXPIRYDATE"];
            }

            if (!data["SENDRESULT"].Equals(DBNull.Value))
            {
                item.SendResult = (string)data["SENDRESULT"];
            }

            return item;

        }

        public List<Model.AccountVerification> Select(string condition, string orderBy, int limit = 0, int offset = 0)
        {
            var list = new List<Model.AccountVerification>();

            var query = "SELECT " + TABLE_COLUMN + " FROM [DBO].[ACCOUNTVERIFICATION]";

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

            var query = "SELECT " + (columns == string.Empty ? TABLE_COLUMN : columns) + " FROM [DBO].[ACCOUNTVERIFICATION]";

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
            var query = "SELECT COUNT(ID) FROM [DBO].[ACCOUNTVERIFICATION]";

            if (condition != string.Empty) query += " WHERE " + condition;

            using (var db = new SqlManager(_connectionString))
            {
                return (int)db.ExecuteScalar(query);
            }
        }

        public object GetSumTotal(string column, string condition)
        {
            var query = "SELECT ISNULL(SUM(" + column + "),0) FROM [DBO].[ACCOUNTVERIFICATION]";

            if (condition != string.Empty) query += " WHERE " + condition;

            using (var db = new SqlManager(_connectionString))
            {
                return db.ExecuteScalar(query);
            }
        }

    }

}

