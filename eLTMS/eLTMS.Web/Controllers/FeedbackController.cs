﻿using AutoMapper;
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
    public class FeedbackController : Controller
    {
        //Nguyen Huu Lam
        // GET: Feedback
        //Khai báo IFeedbackService
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            this._feedbackService = feedbackService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Feedbacks()
        {
            return View();
        }
        //Tạo page cho View Feedback-lay tat ca feedback show tren bang
        [HttpGet]
        public JsonResult GetAllFeed(String dateTime = "", int page = 1, int pageSize = 20)
        {
            var queryResult = _feedbackService.GetAllFeed(dateTime);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

        // Xem thông tin detail của feedback
        [HttpGet]
        public JsonResult FeedbackDetail(int id)
        {
            var result = _feedbackService.getFeedbackId(id);
            var supply = Mapper.Map<Feedback, FeedbackDto>(result);
            return Json(new
            {
                sucess = true,
                data = supply
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFeedback(int feedId)
        {
            var result = _feedbackService.DeleteFeedback(feedId);
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult UpdateFeedback(Feedback feedback)
        {
            var result = _feedbackService.Update(feedback);
            return Json(new
            {
                sucess = result
            });
        }
    }
}