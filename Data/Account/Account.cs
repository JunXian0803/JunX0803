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
    public class Account : AccountBase
    {
        private readonly string _connectionString;

        public Account(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public long Insert(Model.Account account, Model.AccountProfile profile)
        {
            string query = "DECLARE @TEMPID TABLE(ID BIGINT); INSERT INTO[DBO].[ACCOUNT] ([USERNAME], [PASSWORD], [ISACTIVE], [ISDELETED], [CREATEDBY], [CREATEDDATE], [MODIFIEDBY], [MODIFIEDDATE]) OUTPUT INSERTED.ID INTO @TEMPID VALUES(@USERNAME, @PASSWORD, @ISACTIVE, @ISDELETED, @CREATEDBY, GETUTCDATE(), @MODIFIEDBY, GETUTCDATE()); SELECT* FROM @TEMPID";


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

                    if (profile != null)
                    {
                        query = "INSERT INTO[DBO].[ACCOUNTPROFILE]([ACCOUNTID], [PROFILEPICTURE], [FIRSTNAME], [LASTNAME], [NRIC], [EMAIL], [COUNTRYCODE], [PHONENUMBER], [ADDRESS], [CREATEDBY], [CREATEDDATE], [MODIFIEDBY], [MODIFIEDDATE]) OUTPUT INSERTED.ID INTO @TEMPID VALUES (@ACCOUNTID, @PROFILEPICTURE, @FIRSTNAME, @LASTNAME, @NRIC, @EMAIL, @COUNTRYCODE, @PHONENUMBER, @ADDRESS, @CREATEDBY, GETUTCDATE(), @MODIFIEDBY, GETUTCDATE());";

                        db.Parameters.Clear();
                        db.Parameters.Add(new SqlParameter("@AccountID", profile.AccountID));
                        db.Parameters.Add(new SqlParameter("@ProfilePicture", MyFunction.CheckNull(profile.ProfilePicture)));
                        db.Parameters.Add(new SqlParameter("@FirstName", MyFunction.CheckNull(profile.FirstName)));
                        db.Parameters.Add(new SqlParameter("@LastName", MyFunction.CheckNull(profile.LastName)));
                        db.Parameters.Add(new SqlParameter("@NRIC", MyFunction.CheckNull(profile.NRIC)));
                        db.Parameters.Add(new SqlParameter("@Email", MyFunction.CheckNull(profile.Email)));
                        db.Parameters.Add(new SqlParameter("@CountryCode", MyFunction.CheckNull(profile.CountryCode)));
                        db.Parameters.Add(new SqlParameter("@PhoneNumber", MyFunction.CheckNull(profile.PhoneNumber)));
                        db.Parameters.Add(new SqlParameter("@Address", MyFunction.CheckNull(profile.Address)));
                        db.Parameters.Add(new SqlParameter("@CreatedBy", MyFunction.CheckNull(profile.CreatedBy)));
                        db.Parameters.Add(new SqlParameter("@ModifiedBy", MyFunction.CheckNull(profile.ModifiedBy)));


                        db.ExecuteNonQuery(query);
                    }
                    db.Commit();

                }
                catch(SqlException)
                {
                        db.Rollback(); 
                        throw;
                }
            
            }

            return id;
        }
    }



    }




