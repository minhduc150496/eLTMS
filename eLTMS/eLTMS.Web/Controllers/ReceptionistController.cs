﻿using AutoMapper;
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
    public class ReceptionistController : BaseController
    {
        private readonly IReceptionistService _receptionistService;

        public ReceptionistController(IReceptionistService receptionistService)
        {
            this._receptionistService = receptionistService;
            //this._importPaperService = importPaperService;
        }
        public ActionResult Index()
        {
            if (base.ValidRole((int)RoleEnum.Manager, (int)RoleEnum.Receptionist))
            {
                return View();
            }
            var returnUrl = Request.Url.AbsoluteUri;
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
        
       
        [HttpGet]
        public JsonResult GetPatientByDate(string search, DateTime date, int page=1, int pageSize=20)
        {
            var result = _receptionistService.GetAllPatientByDateTesting(search, date);
            var totalRows = result.Count();
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAppByPatientId(int patientId, DateTime date, int page = 1, int pageSize = 20)
        {
            var result = _receptionistService.GetAppByPatient(patientId, date);
            var totalRows = result.Count();
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsPaid(int patientId, DateTime date)
        {
            var result = _receptionistService.ChangeIsPaid(patientId,date);
            if (result == true)
            {
                var tokens = _receptionistService.GetAllTokens();// lấy tất cả device token
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

        [HttpGet]
        public JsonResult GetPriceByPatient(int patientId, DateTime date, int page = 1, int pageSize = 20)
        {
            var result = _receptionistService.GetPrice(patientId, date);
            var totalRows = result.TotalPrice;
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
    }
}