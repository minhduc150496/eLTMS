using AutoMapper;
using eLTMS.Models.Models.dto;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        private readonly IPatientService _patientService;

        //private readonly IImportPaperService _importPaperService;
        public PatientController(IPatientService patientService)
        {
            this._patientService = patientService;
            //this._importPaperService = importPaperService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Patients()
        {
            return View();
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
            var result = _patientService.Update(patient.PatientId, patient.PatientCode, patient.FullName, patient.Gender, patient.PhoneNumber, patient.HomeAddress, patient.CompanyAddress);
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
            var supply = Mapper.Map<Patient, PatientDto>(result);
            return Json(new
            {
                sucess = true,
                data = supply
            }, JsonRequestBehavior.AllowGet);
        }
    }
}