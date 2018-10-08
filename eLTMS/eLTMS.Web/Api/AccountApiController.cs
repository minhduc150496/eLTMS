using eLTMS.BusinessLogic.Services;
using eLTMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace eLTMS.Web.Api
{
    public class AccountApiController: ApiController
    {
        private readonly IAccountService _accountService;
        public AccountApiController(IAccountService accountService)
        {
            this._accountService = accountService;
        }
        [HttpPost]
        [Route("api/account/login")]
        public HttpResponseMessage Login(LoginModel loginModel)
        {
            var phoneNumber = loginModel.phoneNumber;
            var password = loginModel.password;
            var account = this._accountService.checkLogin(phoneNumber, password);
            var loginResult = new LoginResultModel();
            if (account != null)
            {
                loginResult.result = true;
                var patient = account.Patients.FirstOrDefault();
                loginResult.fullname = patient.FullName;
            }
            else
            {
                loginResult.result = false;
                loginResult.phoneNumber = phoneNumber;
            }
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, loginResult);
            return response;
        }
    }
}