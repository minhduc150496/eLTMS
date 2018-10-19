using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Areas.UserWeb.Controllers
{
    public class AppointmentController : Controller
    {
        // GET: UserWeb/Appointment
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserWeb/Appointment/Create
        public ActionResult Create()
        {
            return View("Create", "_Layout");
        }

        // GET: UserWeb/Appointment/NewAppointments
        public ActionResult NewAppointments()
        {
            return View("NewAppointments", "_Layout");
        }

        // GET: UserWeb/Appointment/History
        public ActionResult History() // Old Appointment
        {
            return View("History", "_Layout");
        }

        // GET: UserWeb/Appointment/Edit
        public ActionResult Edit()
        {
            return View("Edit", "_Layout");
        }

        // GET: UserWeb/Appointment/Results
        public ActionResult Results()
        {
            return View("Results", "_Layout");
        }

    }
}