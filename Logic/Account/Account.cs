using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Helper;
using Logic;
using Data;


namespace Logic
{
    public class Account : AccountBase
    {
        private readonly string _connectionString;
      

        public Account(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        
        public bool Validation_PhoneNumber(string phoneNumber)
        {
            bool exists = phoneNumberValid (phoneNumber);
            if(!exists)
            {
                AddPhoneNumber(phoneNumber);
            }

            return exists;
        }

        public bool phoneNumberValid(string phoneNumber)
        {
            var condition = string.Format("[PhoneNumber] = {0}", MyFunction.CSQL(phoneNumber));
            var list = new Data.AccountProfile(_connectionString).GetRecordCount(condition);

            return true;

        }      
        
        public void  AddPhoneNumber(string phoneNumber)
        {
            var AccountProfile = new Model.AccountProfile();
            {
                AccountProfile.PhoneNumber = phoneNumber;
            };
            
            new Data.AccountProfile(_connectionString).Insert(AccountProfile);

        }

        
        

         


        
    }
}


