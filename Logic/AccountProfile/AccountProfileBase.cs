using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Logic
{
    public class AccountProfileBase
    {
        private readonly string _connectionString;

        public AccountProfileBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool Validation(Model.AccountProfile accountProfile)
        {
            bool valid = true;

            if (accountProfile.AccountID == -1)
            {
                valid = false;
                throw new ApplicationException("Required Value - Account.");
            }

            if (accountProfile.ProfilePicture != null)
            {
                if (accountProfile.ProfilePicture.Length > 255)
                {
                    valid = false;
                    throw new ApplicationException("Profile Picture exists maximum length of 255.");
                }
            }

            if (accountProfile.FirstName != null)
            {
                if (accountProfile.FirstName.Length > 50)
                {
                    valid = false;
                    throw new ApplicationException("First Name exists maximum length of 50.");
                }
            }

            if (accountProfile.LastName != null)
            {
                if (accountProfile.LastName.Length > 50)
                {
                    valid = false;
                    throw new ApplicationException("Last Name exists maximum length of 50.");
                }
            }

            if (accountProfile.NRIC != null)
            {
                if (accountProfile.NRIC.Length > 20)
                {
                    valid = false;
                    throw new ApplicationException("N R I C exists maximum length of 20.");
                }
            }

            if (accountProfile.Email != null)
            {
                if (accountProfile.Email.Length > 100)
                {
                    valid = false;
                    throw new ApplicationException("Email exists maximum length of 100.");
                }
            }

            if (accountProfile.CountryCode != null)
            {
                if (accountProfile.CountryCode.Length > 5)
                {
                    valid = false;
                    throw new ApplicationException("Country Code exists maximum length of 5.");
                }
            }

            if (accountProfile.PhoneNumber != null)
            {
                if (accountProfile.PhoneNumber.Length > 20)
                {
                    valid = false;
                    throw new ApplicationException("Phone Number exists maximum length of 20.");
                }
            }

            if (accountProfile.Address != null)
            {
                if (accountProfile.Address.Length > 255)
                {
                    valid = false;
                    throw new ApplicationException("Address exists maximum length of 255.");
                }
            }

            if (accountProfile.CreatedBy != null)
            {
                if (accountProfile.CreatedBy >= 50)
                {
                    valid = false;
                    throw new ApplicationException("Created By exists maximum length of 50.");
                }
            }

            if (accountProfile.ModifiedBy != null)
            {
                if (accountProfile.ModifiedBy >= 50)
                {
                    valid = false;
                    throw new ApplicationException("Modified By exists maximum length of 50.");
                }
            }

            return valid;
        }

        public bool Add(Model.AccountProfile accountProfile)
        {
            if (Validation(accountProfile))
            {
                accountProfile.ID = new Data.AccountProfile(_connectionString).Insert(accountProfile);
            }
            return (accountProfile.ID > 0);
        }

        public bool Edit(Model.AccountProfile accountProfile)
        {
            if (Validation(accountProfile))
            {
                return (new Data.AccountProfile(_connectionString).Update(accountProfile) > 0);
            }
            return false;
        }

        public bool Delete(long id)
        {
            return (new Data.AccountProfile(_connectionString).Delete(id) > 0);
        }

        public Model.AccountProfile GetAccountProfile(long id)
        {
            var list = new Data.AccountProfile(_connectionString).Select("ID = " + MyFunction.CSQL(id.ToString()), string.Empty);
            if (list.Count > 0) return list[0];
            return null;
        }


        public DataTable GetDataTable()
        {
            return GetDataTable(string.Empty, string.Empty, string.Empty);
        }

        public DataTable GetDataTable(string columns, string condition, string orderBy, int limit = 0, int offset = 0)
        {
            return new Data.AccountProfile(_connectionString).SelectDataTable(columns, condition, orderBy, limit, offset);
        }
    }
}

