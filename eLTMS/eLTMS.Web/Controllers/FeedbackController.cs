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
        public ActionResult Index()
        {
            return View();
        }
    }
}