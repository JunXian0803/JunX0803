using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Logic
{
    public class AccountBase
    {
        private readonly string _connectionString;

        public AccountBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool Validation(Model.Account account)
        {
            bool valid = true;

            if (account.Username == null || account.Username == string.Empty)
            {
                valid = false;
                throw new ApplicationException("Required Value - Username.");
            }

            if (account.Username != null)
            {
                if (account.Username.Length > 50)
                {
                    valid = false;
                    throw new ApplicationException("Username exists maximum length of 50.");
                }
            }

            if (account.Password == null || account.Password == string.Empty)
            {
                valid = false;
                throw new ApplicationException("Required Value - Password.");
            }

            if (account.Password != null)
            {
                if (account.Password.Length > 255)
                {
                    valid = false;
                    throw new ApplicationException("Password exists maximum length of 255.");
                }
            }

            if (account.CreatedBy != null)
            {
                if (account.CreatedBy >= 50)
                {
                    valid = false;
                    throw new ApplicationException("Created By exists maximum length of 50.");
                }
            }

            if (account.ModifiedBy != null)
            {
                if (account.ModifiedBy >= 50)
                {
                    valid = false;
                    throw new ApplicationException("Modified By exists maximum length of 50.");
                }
            }

            return valid;
        }

        public bool Add(Model.Account account)
        {
            if (Validation(account))
            {
                account.ID = new Data.Account(_connectionString).Insert(account);
            }
            return (account.ID > 0);
        }

        public bool Edit(Model.Account account)
        {
            if (Validation(account))
            {
                return (new Data.Account(_connectionString).Update(account) > 0);
            }
            return false;
        }

        public bool Delete(long id)
        {
            return (new Data.Account(_connectionString).Delete(id) > 0);
        }

        public virtual Model.Account GetAccount(long id)
        {
            //var condition = "ID = " + MyFunction.CSQL(accountId.ToString());

            var list = new Data.Account(_connectionString).Select("ID = " + MyFunction.CSQL(id.ToString()), string.Empty);

            if (list.Count > 0) return list[0];
            return null;
        }


        public DataTable GetDataTable()
        {
            return GetDataTable(string.Empty, string.Empty, string.Empty);
        }

        public DataTable GetDataTable(string columns, string condition, string orderBy, int limit = 0, int offset = 0)
        {
            return new Data.Account(_connectionString).SelectDataTable(columns, condition, orderBy, limit, offset);
        }
    }
}

