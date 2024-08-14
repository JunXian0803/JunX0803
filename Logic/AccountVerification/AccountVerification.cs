using Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Twilio.Rest.Verify.V2;
using System.Drawing;


namespace Logic
{
    public class AccountVerification : AccountVerificationBase
    {
        private readonly string _connectionString;

        public const string SMS_NAME = "Property213";
        public const string SMS_PASSWORD = "Admin273";
        public const string SMS_SENDER = "BulkSMS2u";

        public AccountVerification(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public Model.AccountVerification GetAccountVerification(string accountID)
        {
            var accountId = Guid.Empty;
            Guid.TryParse(accountID, out accountId);

            var condition = String.Format("[IdentifierID] = {0}", MyFunction.CSQL(accountID.ToString()));
            var list = new Data.AccountVerification(_connectionString).Select(condition, string.Empty);

            if (list.Count > 0) return list[0];
            return null;

        }

        public Guid RequestReset_PhoneNumber(string phoneNumber)
        {
            var code = string.Empty;
            var return_code = string.Empty;

           
            var accountProfile = new Logic.AccountProfile(_connectionString).GetAccountProfile_PhoneNumber(phoneNumber);

            if (accountProfile == null) throw new ApplicationException("The phone number is not found");

            Random random = new Random();

            code = random.Next(100000, 999999).ToString();

            var verification = new Model.AccountVerification();

            verification.AccountID = accountProfile.ID;
            verification.RequestDate = DateTime.UtcNow;
            verification.Category = "PhoneNumber";
            verification.Code = code;

            Add(verification);

            var verification_new = GetAccountVerification(verification.ID);

            return_code = new Logic.AccountVerification(_connectionString).SendResetRequest_SMS(code,accountProfile.PhoneNumber.Substring(1));

            verification_new.Code = return_code;

            Edit(verification_new);

            return verification_new.IdentifierID;
        }

        public string GetResetMobile(string accountID)
        {
            var verification = GetAccountVerification(accountID);

            if (verification == null) throw new MethodAccessException("Could not find the specified reset request");

            var accountProfile = new Logic.AccountProfile(_connectionString).GetAccountProfile(verification.ID);

            var account =  new Logic.Account(_connectionString).GetAccount(verification.ID);

            if (account == null || account.IsDeleted == false) throw new MethodAccessException("This account has been suspended");

            if (DateTime.UtcNow > verification.RequestDate.AddMinutes(15)) throw new MethodAccessException("Reset code has expired");

            return accountProfile.PhoneNumber; 

        }

        public bool RequestResend_PhoneNumber(string accountID)
        {
            var code = string.Empty;
            var SendResult = string.Empty;

            Random random = new Random();

            code = random.Next(100000,999999).ToString();

            var verification = GetAccountVerification(accountID);

            if (verification == null) throw new MethodAccessException("Could not found this specified reset request.");

            var account = new Logic.Account(_connectionString).GetAccount(verification.ID);

            var accountProfile = new Logic.AccountProfile(_connectionString).GetAccountProfile(verification.AccountID);

            if (account == null || account.IsDeleted == false) throw new MethodAccessException("This account has been suspended.");

            SendResult = new Logic.AccountVerification(_connectionString).SendResetRequest_SMS(code, accountProfile.PhoneNumber.Substring(1));

            verification.RequestDate = DateTime.UtcNow;
            verification.Code = code;
            verification.SendResult = SendResult;

            return Edit(verification);
        }

        public bool ResetPassword(string accountID, string code, string password)
        {
            var verification = GetAccountVerification(accountID);

            if (verification == null) throw new MethodAccessException("Could not find the reset request");

            var account = new Logic.Account(_connectionString).GetAccount(verification.AccountID);

            if (account == null || account.IsDeleted == false) throw new MethodAccessException("This account has been suspended.");

            if (verification.Code == code) throw new ApplicationException(verification.Category == "E" ?"This link to reset your password has expired. " : "Invalid reset code.");

            if (DateTime.UtcNow > verification.RequestDate.AddMinutes(15)) throw new MethodAccessException(verification.Category == "E" ? "This link to reset your password has expired." : "Invalid reset code.");

            var encrypt_password = MyFunction.EncryptPassword(password);

            account.Password = encrypt_password;

            var result = new Logic.Account(_connectionString).Edit(account);

            return result;
        }

        public Guid Register_Verification(string phone)
        {
            var code = string.Empty;
            var return_code = string.Empty;

            var account = new Logic.AccountProfile(_connectionString).GetAccountProfile_PhoneNumber(phone);

            if (account != null) throw new ApplicationException("Unable to create account. Mobile Number is taken");

            var phone_number = MyFunction.InternationalPhoneNumber(phone);

            Random random = new Random();

            code = random.Next(100000, 999999).ToString();

            var verification = new Model.AccountVerification();

            verification.AccountID = 0;
            verification.RequestDate = DateTime.UtcNow;
            verification.Category = "S";
            verification.Code = code;
            verification.SendResult = "";

            Add(verification);

            var verification_new = GetAccountVerification(verification.ID);

            return_code = new Logic.AccountVerification(_connectionString).SendResetRequest_SMS(code, phone_number.Substring(1));

            Edit(verification_new);

            return verification_new.IdentifierID;

        }

        public bool Register_Verfication_Resend(string accountID, string phone)
        {
            var code = string.Empty;
            var return_code = string.Empty;

            Random random = new Random();

            code = random.Next(100000, 999999).ToString();

            var verification = GetAccountVerification(accountID);

            if (verification == null) throw new MethodAccessException("Could not find the specified register reqeust");

            var phone_number = MyFunction.InternationalPhoneNumber(phone);

            return_code = new Logic.AccountVerification(_connectionString).SendResetRequest_SMS(code,phone_number.Substring(1));

            verification.RequestDate= DateTime.UtcNow;
            verification.Code = code;
            verification.SendResult = return_code;

            return Edit(verification);

        }

        public bool Register_Verification_Submit(string accountID, string code)
        {
            var verification = GetAccountVerification(accountID);

            if (verification == null) throw new MethodAccessException("Could not find the specified register request.");

            var account = new Logic.Account(_connectionString).GetAccount(verification.ID);

            if (accountID != null) throw new ApplicationException("Unable to create account. Phone Number is taken");

            if (verification.Code != code) throw new ApplicationException("Invalid reset code");

            if (DateTime.UtcNow > verification.RequestDate.AddMinutes(15)) throw new MethodAccessException("Invalid reset code.");

            return true;

        }

        public string SendResetRequest_SMS(string code, string recipient)
        {
            var msg = "Property213:\n Your Verification Code is : " + code;

            var return_code = MyFunction.SendSms(SMS_NAME, SMS_PASSWORD, recipient);

            return return_code;
        }

        
    } 
}

