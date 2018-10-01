using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Areas.AdminWeb.Controllers
{
    public class PatientController : Controller
    {
        // GET: AdminWeb/Patient
        public ActionResult Index()
        {
            return View();
        }
    }
}