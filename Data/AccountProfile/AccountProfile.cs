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
    public class AccountProfile : AccountProfileBase
    {
        private readonly string _connectionString;

        public AccountProfile(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public long InsertPhoneNumber(Model.AccountProfile accountProfile)
        {
            string query = "INSERT INTO [DBO].[ACCOUNTPROFILE]([PHONENUMBER]) VALUES (@PHONENUMBER)";

            long id = -1;

            using (var db = new SqlManager(_connectionString))
            {
                try
                {
                    db.BeginTransaction();
                    db.Parameters.Clear();
                    db.Parameters.Add(new SqlParameter("@PhoneNumber", MyFunction.CheckNull(accountProfile.PhoneNumber)));

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


    }

}

