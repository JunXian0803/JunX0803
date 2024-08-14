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
    public class AccountProfileBase
    {
        private readonly string _connectionString;

        public AccountProfileBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected const string TABLE_COLUMN = "[ID], [IdentifierID], [AccountID], [ProfilePicture], [FirstName], [LastName], [NRIC], [Email], [CountryCode], [PhoneNumber], [Address], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]";

        public long Insert(Model.AccountProfile accountProfile)
        {
            string query = "DECLARE @TEMPID TABLE(ID BIGINT); INSERT INTO [DBO].[ACCOUNTPROFILE] ([ACCOUNTID], [PROFILEPICTURE], [FIRSTNAME], [LASTNAME], [NRIC], [EMAIL], [COUNTRYCODE], [PHONENUMBER], [ADDRESS], [CREATEDBY], [CREATEDDATE], [MODIFIEDBY], [MODIFIEDDATE]) OUTPUT INSERTED.ID INTO @TEMPID VALUES (@ACCOUNTID, @PROFILEPICTURE, @FIRSTNAME, @LASTNAME, @NRIC, @EMAIL, @COUNTRYCODE, @PHONENUMBER, @ADDRESS, @CREATEDBY, GETUTCDATE(), @MODIFIEDBY, GETUTCDATE()); SELECT * FROM @TEMPID";
            long id = -1;

            using (var db = new SqlManager(_connectionString))
            {
                try
                {
                    db.BeginTransaction();
                    db.Parameters.Clear();
                    db.Parameters.Add(new SqlParameter("@AccountID", accountProfile.AccountID));
                    db.Parameters.Add(new SqlParameter("@ProfilePicture", MyFunction.CheckNull(accountProfile.ProfilePicture)));
                    db.Parameters.Add(new SqlParameter("@FirstName", MyFunction.CheckNull(accountProfile.FirstName)));
                    db.Parameters.Add(new SqlParameter("@LastName", MyFunction.CheckNull(accountProfile.LastName)));
                    db.Parameters.Add(new SqlParameter("@NRIC", MyFunction.CheckNull(accountProfile.NRIC)));
                    db.Parameters.Add(new SqlParameter("@Email", MyFunction.CheckNull(accountProfile.Email)));
                    db.Parameters.Add(new SqlParameter("@CountryCode", MyFunction.CheckNull(accountProfile.CountryCode)));
                    db.Parameters.Add(new SqlParameter("@PhoneNumber", MyFunction.CheckNull(accountProfile.PhoneNumber)));
                    db.Parameters.Add(new SqlParameter("@Address", MyFunction.CheckNull(accountProfile.Address)));
                    db.Parameters.Add(new SqlParameter("@CreatedBy", MyFunction.CheckNull(accountProfile.CreatedBy)));
                    db.Parameters.Add(new SqlParameter("@ModifiedBy", MyFunction.CheckNull(accountProfile.ModifiedBy)));

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

        public int Update(Model.AccountProfile accountProfile)
        {
            string query = "UPDATE [DBO].[ACCOUNTPROFILE] SET [ACCOUNTID] = @ACCOUNTID, [PROFILEPICTURE] = @PROFILEPICTURE, [FIRSTNAME] = @FIRSTNAME, [LASTNAME] = @LASTNAME, [NRIC] = @NRIC, [EMAIL] = @EMAIL, [COUNTRYCODE] = @COUNTRYCODE, [PHONENUMBER] = @PHONENUMBER, [ADDRESS] = @ADDRESS, [MODIFIEDBY] = @MODIFIEDBY, [MODIFIEDDATE] = GETUTCDATE() WHERE ID = @ID";
            int affected = -1;

            using (var db = new SqlManager(_connectionString))
            {
                try
                {
                    db.BeginTransaction();

                    db.Parameters.Clear();
                    db.Parameters.Add(new SqlParameter("@ID", accountProfile.ID.ToString()));
                    db.Parameters.Add(new SqlParameter("@AccountID", accountProfile.AccountID));
                    db.Parameters.Add(new SqlParameter("@ProfilePicture", MyFunction.CheckNull(accountProfile.ProfilePicture)));
                    db.Parameters.Add(new SqlParameter("@FirstName", MyFunction.CheckNull(accountProfile.FirstName)));
                    db.Parameters.Add(new SqlParameter("@LastName", MyFunction.CheckNull(accountProfile.LastName)));
                    db.Parameters.Add(new SqlParameter("@NRIC", MyFunction.CheckNull(accountProfile.NRIC)));
                    db.Parameters.Add(new SqlParameter("@Email", MyFunction.CheckNull(accountProfile.Email)));
                    db.Parameters.Add(new SqlParameter("@CountryCode", MyFunction.CheckNull(accountProfile.CountryCode)));
                    db.Parameters.Add(new SqlParameter("@PhoneNumber", MyFunction.CheckNull(accountProfile.PhoneNumber)));
                    db.Parameters.Add(new SqlParameter("@Address", MyFunction.CheckNull(accountProfile.Address)));
                    db.Parameters.Add(new SqlParameter("@ModifiedBy", MyFunction.CheckNull(accountProfile.ModifiedBy)));

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
            string query = "DELETE [DBO].[ACCOUNTPROFILE] WHERE ID = @ID;";
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

        public Model.AccountProfile BindData(IDataRecord data)
        {
            var item = new Model.AccountProfile();

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

            if (!data["PROFILEPICTURE"].Equals(DBNull.Value))
            {
                item.ProfilePicture = (string)data["PROFILEPICTURE"];
            }

            if (!data["FIRSTNAME"].Equals(DBNull.Value))
            {
                item.FirstName = (string)data["FIRSTNAME"];
            }

            if (!data["LASTNAME"].Equals(DBNull.Value))
            {
                item.LastName = (string)data["LASTNAME"];
            }

            if (!data["NRIC"].Equals(DBNull.Value))
            {
                item.NRIC = (string)data["NRIC"];
            }

            if (!data["EMAIL"].Equals(DBNull.Value))
            {
                item.Email = (string)data["EMAIL"];
            }

            if (!data["COUNTRYCODE"].Equals(DBNull.Value))
            {
                item.CountryCode = (string)data["COUNTRYCODE"];
            }

            if (!data["PHONENUMBER"].Equals(DBNull.Value))
            {
                item.PhoneNumber = (string)data["PHONENUMBER"];
            }

            if (!data["ADDRESS"].Equals(DBNull.Value))
            {
                item.Address = (string)data["ADDRESS"];
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
                item.ModifiedBy =(long)data["MODIFIEDBY"];
            }

            if (!data["MODIFIEDDATE"].Equals(DBNull.Value))
            {
                item.ModifiedDate = (DateTime?)data["MODIFIEDDATE"];
            }

            return item;

        }

        public List<Model.AccountProfile> Select(string condition, string orderBy, int limit = 0, int offset = 0)
        {
            var list = new List<Model.AccountProfile>();

            var query = "SELECT " + TABLE_COLUMN + " FROM [DBO].[ACCOUNTPROFILE]";

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

            var query = "SELECT " + (columns == string.Empty ? TABLE_COLUMN : columns) + " FROM [DBO].[ACCOUNTPROFILE]";

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
            var query = "SELECT COUNT(ID) FROM [DBO].[ACCOUNTPROFILE]";

            if (condition != string.Empty) query += " WHERE " + condition;

            using (var db = new SqlManager(_connectionString))
            {
                return (int)db.ExecuteScalar(query);
            }
        }

        public object GetSumTotal(string column, string condition)
        {
            var query = "SELECT ISNULL(SUM(" + column + "),0) FROM [DBO].[ACCOUNTPROFILE]";

            if (condition != string.Empty) query += " WHERE " + condition;

            using (var db = new SqlManager(_connectionString))
            {
                return db.ExecuteScalar(query);
            }
        }


    }

}

