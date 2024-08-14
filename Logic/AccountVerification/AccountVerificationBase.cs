using Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Text;
using System.Net;
using System.Net.Mail;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;
using Twilio.Types;



namespace Logic
{
    public class AccountVerificationBase
    {
        private readonly string _connectionString;

        private readonly Logic.AccountProfile _accountProfile;

        private static ConcurrentDictionary<long, string> verificationCodes = new ConcurrentDictionary<long, string>();

        public AccountVerificationBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool Validation(Model.AccountVerification accountVerification)
        {
            bool valid = true;

            if (accountVerification.AccountID == -1)
            {
                valid = false;
                throw new ApplicationException("Required Value - Account.");
            }

            if (accountVerification.Category != null)
            {
                if (accountVerification.Category.Length > 50)
                {
                    valid = false;
                    throw new ApplicationException("Category exists maximum length of 50.");
                }
            }

            if (accountVerification.Code != null)
            {
                if (accountVerification.Code.Length > 50)
                {
                    valid = false;
                    throw new ApplicationException("Code exists maximum length of 50.");
                }
            }

            if (accountVerification.SendResult != null)
            {
                if (accountVerification.SendResult.Length > 500)
                {
                    valid = false;
                    throw new ApplicationException("Send Result exists maximum length of 500.");
                }
            }

            return valid;
        }

        public bool Add(Model.AccountVerification accountVerification)
        {
            if (Validation(accountVerification))
            {
                accountVerification.ID = new Data.AccountVerification(_connectionString).Insert(accountVerification);
            }
            return (accountVerification.ID > 0);
        }

        public bool Edit(Model.AccountVerification accountVerification)
        {
            if (Validation(accountVerification))
            {
                return (new Data.AccountVerification(_connectionString).Update(accountVerification) > 0);
            }
            return false;
        }

        public bool Delete(long id)
        {
            return (new Data.AccountVerification(_connectionString).Delete(id) > 0);
        }

        public Model.AccountVerification GetAccountVerification(long id)
        {
            var list = new Data.AccountVerification(_connectionString).Select("ID = " + MyFunction.CSQL(id.ToString()), string.Empty);
            if (list.Count > 0) return list[0];
            return null;
        }


        public DataTable GetDataTable()
        {
            return GetDataTable(string.Empty, string.Empty, string.Empty);
        }

        public DataTable GetDataTable(string columns, string condition, string orderBy, int limit = 0, int offset = 0)
        {
            return new Data.AccountVerification(_connectionString).SelectDataTable(columns, condition, orderBy, limit, offset);
        }

       
        

    }
}

