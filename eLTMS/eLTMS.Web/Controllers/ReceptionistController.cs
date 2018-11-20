using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class ReceptionistController : Controller
    {
        private readonly IReceptionistService _receptionistService;

        public ReceptionistController(IReceptionistService receptionistService)
        {
            this._receptionistService = receptionistService;
            //this._importPaperService = importPaperService;
        }
        public ActionResult Index()
        {
            return View();
        }

        // DucBM
        public ActionResult SampleGettings()
        {
            return View("SampleGettings");
        }

        [HttpGet]
        public JsonResult GetAllSampleGettingsBySampleGroupId(int sampleGroupId, int page = 1, int pageSize = 20)
        {
            var queryResult = _receptionistService.GetSampleGettingsBySampleGroupId(sampleGroupId);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<SampleGetting>, IEnumerable<SampleGettingForReceptionistDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllAppointment(int page = 1, int pageSize = 20 )
        {
            var queryResult = _receptionistService.GetAllAppointment();
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetAllDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllAppointment2()
        {
            return null;
        }

        [HttpGet]
        public JsonResult GetAppBySample(DateTime date, int sampleId, int page=1, int pageSize=20)
        {
            //var queryResult = _receptionistService.GetAppBySample(sampleId);
            var result = _receptionistService.GetAllBySample(date, sampleId);
            var totalRows = result.Count();
            //var totalRows = queryResult.Count();
            //var result = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetAllDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddApp(AppointmentAddDto data)
        {
            var result = _receptionistService.Add(data);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult CheckAndDelete(DateTime dateTime)
       {
            var result = _receptionistService.CheckAndDeleteMauAndNuocTieu(dateTime);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult IsPaid(int sampleGettingId)
        {
            var result = _receptionistService.ChangeIsPaid(sampleGettingId);
            return Json(new
            {
                success = result
            });
        }
        
        
    }
}