using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Enums;
using eLTMS.Models.Models.dto;
using eLTMS.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class CashierController : BaseController
    {
        private readonly ICashiertService _cashierService;

        public CashierController(ICashiertService cashierService)
        {
            this._cashierService = cashierService;
            //this._importPaperService = importPaperService;
        }
        public ActionResult Index()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.Cashier))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
        
        [HttpGet]
        public JsonResult GetAllSampleGettingsBySampleGroupId(int sampleGroupId, int page = 1, int pageSize = 20)
        {
            var queryResult = _cashierService.GetSampleGettingsBySampleGroupId(sampleGroupId);
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
        public JsonResult GetAppBySample(string search, DateTime date, int sampleId, int page=1, int pageSize=20)
        {
            //var queryResult = _receptionistService.GetAppBySample(sampleId);
            var result = _cashierService.GetAllBySample(search, date, sampleId);
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
            var result = _cashierService.Add(data);
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult CheckAndDeleteBlood(DateTime dateTime)
       {
            var result = _cashierService.CheckAndDeleteBlood(dateTime);
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult CheckNDeleteUrine(DateTime dateTime)
        {
            var result = _cashierService.CheckAndDeleteUrine(dateTime);
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult CheckNDeleteCell(DateTime dateTime)
        {
            var result = _cashierService.CheckAndDeleteCell(dateTime);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult CheckNDeleteMucus(DateTime dateTime)
        {
            var result = _cashierService.CheckAndDeleteMucus(dateTime);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult CheckNDeletePhan(DateTime dateTime)
        {
            var result = _cashierService.CheckAndDeletePhan(dateTime);
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult IsPaid(int sampleGettingId)
        {
            var result = _cashierService.ChangeIsPaid(sampleGettingId);
            if (result == true)
            {
                var tokens = _cashierService.GetAllTokens();// lấy tất cả device token
                foreach (var token in tokens)
                {
                    var data = new
                    {
                        to = token.TokenString,
                        data = new
                        {
                            message = "Đã thanh toán. ",
                        }
                    };
                    try
                    {
                        SendNotificationUtils.SendNotification(data); // dòng lệnh gửi data từ server => Firebase, Firebase => Device có device token trong list
                    }
                    catch (Exception ex)
                    {
                        //
                    }
                }
            }
            return Json(new
            {
                success = result
            });
        }
        
        
    }
}