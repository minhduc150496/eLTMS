using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Areas.UserWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/Index
        public ActionResult Index()
        {
            return View("Index", "_Layout");
        }

        // GET: /Home/About
        public ActionResult About()
        {
            return View("About", "_Layout");
        }

        // GET: /Home/Contact
        public ActionResult Contact()
        {
            return View("Contact", "_Layout");
        }

        // GET: /Home/Gallery
        public ActionResult Gallery()
        {
            return View("Gallery", "_Layout");
        }

        // GET: /Home/FAQ
        public ActionResult FAQ()
        {
            return View("FAQ", "_Layout");
        }

        // GET: /Home/Service
        public ActionResult Service()
        {
            return View("Service", "_Layout");
        }

        // GET: /Home/ServiceDetails
        public ActionResult ServiceDetails()
        {
            return View("ServiceDetails", "_Layout");
        }

        // GET: /Home/Appointment
        public ActionResult Appointment()
        {
            return View("Appointment", "_Layout");
        }
    }
}