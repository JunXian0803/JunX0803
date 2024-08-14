using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace Logic
{
    public class AccountProfile : AccountProfileBase
    {
        private readonly string _connectionString;

        public AccountProfile(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public Model.AccountProfile GetAccountProfile_AccountID(long accountID)
        {
            var condition = String.Format("[ACCOUNTID] = {0}", accountID);

            var list = new Data.AccountProfile(_connectionString).Select(condition, string.Empty);

            if (list.Count > 0) return list[0];

            return null;
        }


        public Model.AccountProfile GetAccountProfile_Email(string email)
        {
            var condition = String.Format("[Email] = {0}", MyFunction.CSQL(email));

            var list = new Data.AccountProfile(_connectionString).Select(condition, string.Empty);

            if (list.Count > 0) return list[0];

            return null;
        }

        public bool UniqueValidation_Email(string email)
        {
            var condition = $"[Email] = {MyFunction.CSQL(email)}";
            var record = new Data.AccountProfile(_connectionString).GetRecordCount(condition);

            if(record == 0) return false;

            return false;
        }

        public bool UniqueValidation_Email(long accountID, string email)
        {
            var condition = $"[Email] = {MyFunction.CSQL(email)} AND [accountID] = {MyFunction.CSQL(accountID.ToString())}";

            var record = new Data.AccountProfile(_connectionString).GetRecordCount(condition);

            if (record == 0) return false;

            return false;
        }

        public Model.AccountProfile GetAccountProfile_PhoneNumber(string phoneNumber)
        {
            var phone_Number = MyFunction.InternationalPhoneNumber(phoneNumber);
            var condition = String.Format("[PhoneNumber] = {0}", MyFunction.CSQL(phone_Number));

            var list = new Data.AccountProfile(_connectionString).Select(condition, string.Empty);

            if (list.Count > 0) return list[0];

            return null;

        }

        public bool UniqueValidation_PhoneNumber(string phoneNumber)
        {
            var condition = String.Format("[PhoneNumber] = {0}", MyFunction.CSQL(phoneNumber));
            
            var record = new Data.AccountProfile(_connectionString).GetRecordCount(condition);

            if (record == 0) return true;

            return false;
        }

        public DataTable Edit(long ID,string ProfilePicture, string FirstName, string LastName, string NRIC, string Email, string CountryCode, string PhoneNumber, string Address)
        {
            //var transferFile = false;

            var account = new Logic.AccountProfile(_connectionString).GetAccountProfile(ID);

            var unique_email = new Logic.AccountProfile(_connectionString).UniqueValidation_Email(account.ID,Email);

            if (unique_email == false) throw new ApplicationException("Unable to update account. This email has already taken.");

            var profile = GetAccountProfile_AccountID(account.ID);

            if (profile == null) profile = new Model.AccountProfile();

            profile.FirstName = FirstName;
            profile.LastName = LastName;
            profile.NRIC = NRIC;
            profile.Email = Email;
            profile.CountryCode = CountryCode;
            profile.PhoneNumber = PhoneNumber;
            profile.Address = Address;
           

            if(!String.IsNullOrEmpty(ProfilePicture))
            {
                profile.ProfilePicture = ProfilePicture;
            }

            if(profile.ID == 0)
            {
                Add(profile);
            }
            else
            {
                Edit(profile);
            }

            var dt = new Logic.vAccount(_connectionString).GetvAccount(account.ID);

            return dt;
        
        }
    }
}

