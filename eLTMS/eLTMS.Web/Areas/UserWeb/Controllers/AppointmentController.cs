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
        
        // GET: UserWeb/Appointment/ViewAppointments
        public ActionResult ViewAppointments() 
        {
            return View("ViewAppointments", "_Layout");
        }

        // GET: UserWeb/Appointment/Edit/{apId}
        public ActionResult Edit(int appointmentId)
        {
            Appointment app = _appointmentService.GetSingleById(appointmentId);
            AppointmentDto appDto = Mapper.Map<Appointment, AppointmentDto>(app);
            ViewBag.appDto = appDto;
            return View("Edit", "_Layout");
        }

        // GET: UserWeb/Appointment/Result/{apId}
        public ActionResult Result(int appointmentId)
        {
            Appointment app = _appointmentService.GetResultDoneByAppointmentId(appointmentId);
            ResultOfAppointmentDto dto = Mapper.Map<Appointment, ResultOfAppointmentDto>(app);
            ViewBag.ResultDto = dto;
            return View("Result", "_Layout");
        }

    }
}