using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Web.Models;
using eLTMS.Web.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eLTMS.Web.Api
{
    public class AccountApiController : ApiController
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
            var phoneNumber = loginModel.PhoneNumber; // sure that not empty from client
            var password = loginModel.Password; // sure that not empty from client
            var account = this._accountService.checkLogin(phoneNumber, password);
            var loginResult = new LoginResultModel();
            if (account!=null)
            {
                loginResult.LoginSuccess = true;
                var patient = account.Patients.FirstOrDefault();
                loginResult.FullName = patient.FullName;
            } else
            {
                loginResult.LoginSuccess = false;
                loginResult.PhoneNumber = phoneNumber;
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, loginResult);
            return response;
        }

        //EditProfile
        
    }
}
