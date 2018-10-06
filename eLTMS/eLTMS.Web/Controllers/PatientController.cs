using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Web.Models.dto;
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



        [HttpPost]
        public JsonResult AddPatient(Patient patient)
        {
            var result =  _patientService.AddPatient(patient);
            return Json(new
            {
                sucess = result
            });
        }
     
    }
}