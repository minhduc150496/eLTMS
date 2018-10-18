using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models;
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
            object loginResult = null;
            if (account != null)
            {
                var patient = account.Patients.FirstOrDefault();
                loginResult = new
                {
                    Success = true,
                    PatientId = patient.PatientId,
                    PhoneNumber = phoneNumber,
                    FullName = patient.FullName
                };
            }
            else
            {
                loginResult = new
                {
                    Success = account != null,
                    PatientId = -1,
                    PhoneNumber = phoneNumber,
                    FullName = ""
                };
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, loginResult);
            return response;
        }

        [HttpGet]
        [Route("api/account/logout")]
        public HttpResponseMessage Logout()
        {
            return null;
        }

        [HttpGet]
        [Route("api/account/register")]
        public HttpResponseMessage Register()
        {
            return null;
        }

    }
}
