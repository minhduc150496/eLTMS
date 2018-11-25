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
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        protected bool ValidRole(params int[] args)
        {
            if (args == null)
            {
                return false;
            }
            var roleId = Session[ConstantManager.SESSION_ACCOUNT];
            if (roleId != null)
            {
                if (args.Contains((int)roleId))
                {
                    return true;
                }
            }
            return false;
        }
    }
}