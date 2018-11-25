using eLTMS.BusinessLogic.Services;
using eLTMS.Models;
using eLTMS.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult CheckLogin(FormCollection fc)
        {
            var phoneNumber = fc["phoneNumber"];
            var password = fc["password"];
            var returnUrl = fc["returnUrl"];

            var account = this._accountService.checkLogin(phoneNumber, password);
            var errorMessage = "";
            if (account != null)
            {
                Session[ConstantManager.SESSION_ACCOUNT] = account.RoleId;
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                } else
                {
                    // Redirect Default Page of each Role
                    switch(account.RoleId)
                    {
                        case (int)RoleEnum.Receptionist:
                            return Redirect("/Receptionist");
                        case (int)RoleEnum.WarehouseKeeper:
                            return Redirect("/Warehouse/Supplies");
                        case (int)RoleEnum.Doctor:
                            return Redirect("/LabTest/LabTestingResult");
                        case (int)RoleEnum.Manager:
                            return Redirect("/Receptionist");
                        case (int)RoleEnum.Nurse:
                        case (int)RoleEnum.Patient:
                            errorMessage = "Tài khoản của bạn không được phép vào trang này";
                            break;
                    }
                } // end if has returnUrl
            } // end if has account
            else
            {
                errorMessage = "Sai tên hoặc mật khẩu";
            }

            // Require Login again
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ErrorMessage = errorMessage;
            return View("Login");
        }
    }
}