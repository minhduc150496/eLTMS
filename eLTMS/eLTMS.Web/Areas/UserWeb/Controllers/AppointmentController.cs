using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Areas.UserWeb.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            this._appointmentService = appointmentService;
        }

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

        // GET: UserWeb/Appointment/Current
        public ActionResult Current()
        {
            var patientId = 1;
            var appointment = _appointmentService.GetNewApp(patientId).LastOrDefault();            
            ViewBag.Appointment = appointment;
            return View("Current", "_Layout");
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