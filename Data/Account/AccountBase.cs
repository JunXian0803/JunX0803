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
    public class AccountBase
    {
        private readonly string _connectionString;

        public AccountBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected const string TABLE_COLUMN = "[ID], [IdentifierID], [Username], [Password], [IsActive], [IsDeleted], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]";

        public long Insert(Model.Account account)
        {
            string query = "DECLARE @TEMPID TABLE(ID BIGINT); INSERT INTO [DBO].[ACCOUNT] ([USERNAME], [PASSWORD], [ISACTIVE], [ISDELETED], [CREATEDBY], [CREATEDDATE], [MODIFIEDBY], [MODIFIEDDATE]) OUTPUT INSERTED.ID INTO @TEMPID VALUES (@USERNAME, @PASSWORD, @ISACTIVE, @ISDELETED, @CREATEDBY, GETUTCDATE(), @MODIFIEDBY, GETUTCDATE()); SELECT * FROM @TEMPID";
            long id = -1;

            using (var db = new SqlManager(_connectionString))
            {
                try
                {
                    db.BeginTransaction();

                    db.Parameters.Clear();
                    db.Parameters.Add(new SqlParameter("@Username", account.Username));
                    db.Parameters.Add(new SqlParameter("@Password", account.Password));
                    db.Parameters.Add(new SqlParameter("@IsActive", MyFunction.CheckNull(account.IsActive)));
                    db.Parameters.Add(new SqlParameter("@IsDeleted", MyFunction.CheckNull(account.IsDeleted)));
                    db.Parameters.Add(new SqlParameter("@CreatedBy", MyFunction.CheckNull(account.CreatedBy)));
                    db.Parameters.Add(new SqlParameter("@ModifiedBy", MyFunction.CheckNull(account.ModifiedBy)));

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

        public int Update(Model.Account account)
        {
            string query = "UPDATE [DBO].[ACCOUNT] SET [USERNAME] = @USERNAME, [PASSWORD] = @PASSWORD, [ISACTIVE] = @ISACTIVE, [ISDELETED] = @ISDELETED, [MODIFIEDBY] = @MODIFIEDBY, [MODIFIEDDATE] = GETUTCDATE() WHERE ID = @ID";
            int affected = -1;

            using (var db = new SqlManager(_connectionString))
            {
                try
                {
                    db.BeginTransaction();

                    db.Parameters.Clear();
                    db.Parameters.Add(new SqlParameter("@ID", account.ID.ToString()));
                    db.Parameters.Add(new SqlParameter("@Username", account.Username));
                    db.Parameters.Add(new SqlParameter("@Password", account.Password));
                    db.Parameters.Add(new SqlParameter("@IsActive", MyFunction.CheckNull(account.IsActive)));
                    db.Parameters.Add(new SqlParameter("@IsDeleted", MyFunction.CheckNull(account.IsDeleted)));
                    db.Parameters.Add(new SqlParameter("@ModifiedBy", MyFunction.CheckNull(account.ModifiedBy)));

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
            string query = "DELETE [DBO].[ACCOUNT] WHERE ID = @ID;";
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

        public Model.Account BindData(IDataRecord data)
        {
            var item = new Model.Account();

            if (!data["ID"].Equals(DBNull.Value))
            {
                item.ID = (long)data["ID"];
            }

            if (!data["IDENTIFIERID"].Equals(DBNull.Value))
            {
                item.IdentifierID = (Guid)data["IDENTIFIERID"];
            }

            if (!data["USERNAME"].Equals(DBNull.Value))
            {
                item.Username = (string)data["USERNAME"];
            }

            if (!data["PASSWORD"].Equals(DBNull.Value))
            {
                item.Password = (string)data["PASSWORD"];
            }

            if (!data["ISACTIVE"].Equals(DBNull.Value))
            {
                item.IsActive = (bool?)data["ISACTIVE"];
            }

            if (!data["ISDELETED"].Equals(DBNull.Value))
            {
                item.IsDeleted = (bool?)data["ISDELETED"];
            }

            if (!data["CREATEDBY"].Equals(DBNull.Value))
            {
                item.CreatedBy = (long)data["CREATEDBY"];
            }

            if (!data["CREATEDDATE"].Equals(DBNull.Value))
            {
                item.CreatedDate = (DateTime?)data["CREATEDDATE"];
            }

            if (!data["MODIFIEDBY"].Equals(DBNull.Value))
            {
                item.ModifiedBy = (long)data["MODIFIEDBY"];
            }

            if (!data["MODIFIEDDATE"].Equals(DBNull.Value))
            {
                item.ModifiedDate = (DateTime?)data["MODIFIEDDATE"];
            }

            return item;

        }

        public List<Model.Account> Select(string condition, string orderBy, int limit = 0, int offset = 0)
        {
            var list = new List<Model.Account>();

            var query = "SELECT " + TABLE_COLUMN + " FROM [DBO].[ACCOUNT]";

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

            var query = "SELECT " + (columns == string.Empty ? TABLE_COLUMN : columns) + " FROM [DBO].[ACCOUNT]";

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
            var query = "SELECT COUNT(ID) FROM [DBO].[ACCOUNT]";

            if (condition != string.Empty) query += " WHERE " + condition;

            using (var db = new SqlManager(_connectionString))
            {
                return (int)db.ExecuteScalar(query);
            }
        }

        public object GetSumTotal(string column, string condition)
        {
            var query = "SELECT ISNULL(SUM(" + column + "),0) FROM [DBO].[ACCOUNT]";

            if (condition != string.Empty) query += " WHERE " + condition;

            using (var db = new SqlManager(_connectionString))
            {
                return db.ExecuteScalar(query);
            }
        }


    }

}

