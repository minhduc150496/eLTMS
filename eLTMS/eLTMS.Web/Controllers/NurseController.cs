using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Enums;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class NurseController : BaseController
    {
        private readonly INurseService _nurseService;

        public NurseController(INurseService nurseService)
        {
            this._nurseService = nurseService;
        }

        public ActionResult Index()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.Nurse)) // chi co Manager vs Nurse dc vo coi
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
        
        //[HttpGet]
        //public JsonResult GetAllSampleGettingsBySampleGroupId(int sampleGroupId, int page = 1, int pageSize = 20)
        //{
        //    var queryResult = _nurseService.GetSampleGettingsBySampleGroupId(sampleGroupId);
        //    var totalRows = queryResult.Count();
        //    var result = Mapper.Map<IEnumerable<SampleGetting>, IEnumerable<SampleGettingForReceptionistDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
        //    return Json(new
        //    {
        //        success = true,
        //        data = result,
        //        total = totalRows
        //    }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult GetAllAppointment(int page = 1, int pageSize = 20 )
        //{
        //    var queryResult = _nurseService.GetAllBySample();
        //    var totalRows = queryResult.Count();
        //    var result = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetAllDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
        //    return Json(new
        //    {
        //        success = true,
        //        data = result,
        //        total = totalRows
        //    }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetAllAppointment2()
        {
            return null;
        }

        [HttpGet]
        public JsonResult GetAppBySample(DateTime date, int sampleId, int page=1, int pageSize=20)
        {
            //var queryResult = _receptionistService.GetAppBySample(sampleId);
            var result = _nurseService.GetAllBySample(date, sampleId);
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
        public JsonResult IsGot(int sampleGettingId)
        {
            var result = _nurseService.ChangeIsGot(sampleGettingId);
            return Json(new
            {
                success = result
            });
        }
        
        
    }
}