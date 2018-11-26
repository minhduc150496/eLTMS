﻿using AutoMapper;
using eLTMS.Models.Models.dto;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using eLTMS.Models.Enums;

namespace eLTMS.Web.Controllers
{
    public class PatientController : BaseController
    {
        // GET: Patient
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;

        //private readonly IImportPaperService _importPaperService;
        public PatientController(IAppointmentService appointmentService, IPatientService patientService)
        {
            this._patientService = patientService;
            this._appointmentService = appointmentService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Patients()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.Receptionist))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
        public ActionResult Appointment()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAllResults(int id, int page = 1, int pageSize = 20)
        {
            var queryResult = _appointmentService.GetResultDone(id);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllPatients(string phoneNumber = "", int page = 1, int pageSize = 20)
        {
            var queryResult = _patientService.GetAllPatients(phoneNumber);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Patient>, IEnumerable<PatientDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdatePatient(Patient patient)
        {
            var result = _patientService.Update(patient);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpPost]
        public JsonResult AddPatient(Patient patient)
        {
            var result = _patientService.AddPatient(patient);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpGet]
        public JsonResult PatientDetail(int id)
        {
            var result = _patientService.GetPatientById(id);
            var patient = Mapper.Map<Patient, PatientDto>(result);
            return Json(new
            {
                sucess = true,
                data = patient
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateResult(string code, string con)
        {
            var result = _appointmentService.Update(code, con);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpGet]
        public JsonResult GetPatientByCode(string code)
        {
            var result = _appointmentService.GetSingleByCode(code);
            var patient = Mapper.Map<Appointment, AppointmentDto>(result);
            return Json(new
            {
                sucess = true,
                data = patient
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePatient(int patientId)
        {
            var result = _patientService.Delete(patientId);
            return Json(new
            {
                success = result
            });
        }
    }
}