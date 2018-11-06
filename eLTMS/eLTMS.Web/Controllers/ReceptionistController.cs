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
        public JsonResult GetAppBySample(int sampleId, int page=1, int pageSize=20)
        {
            var queryResult = _receptionistService.GetAppBySample(sampleId);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetAllDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
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
                sucess = result
            });
        }

    }
}