using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class AuthorizableController : Controller
    {
        // Author: DucBM
        // GET: Feedback
        private readonly IAccountService _accountService;
        public AuthorizableController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        protected bool CheckSession()
        {
            var accountId = Session["AccountId"];
            if (accountId != null)
            {
                var accId = (int)accountId;
                Account account = _accountService.GetById(accId);
                if (account != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}