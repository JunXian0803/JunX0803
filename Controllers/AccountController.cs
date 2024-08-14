using Logic;
using Microsoft.AspNetCore.Mvc;
using Model;
using Newtonsoft.Json;
using System.Data;
using Helper;
using Microsoft.AspNetCore.Mvc;


namespace Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly Logic.Account _account;
        private readonly Logic.AccountProfile _accountProfile;
        private readonly Logic.AccountVerification _accountVerification;
        


        public AccountController(Logic.Account account, Logic.AccountProfile accountProfile, Logic.AccountVerification accountVerification)
        {
            _account = account;
            _accountProfile = accountProfile;
            _accountVerification = accountVerification;

        }

        [HttpPost("login")]
        public ActionResult ValidationPhoneNumer([FromBody] string profile)
        {
            bool isExistingUser = _account.Validation_PhoneNumber(profile);
            if (isExistingUser)
            {
                return Ok("Login successfull");
            }
            else
            {
                return Ok("Phone number is added");
            }

        }
        

      
        


    }
}



        


     



 