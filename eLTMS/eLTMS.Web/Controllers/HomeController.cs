using eLTMS.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IEmployeeService _employeeService;
        //public HomeController(IEmployeeService employeeService)
        //{
        //    this._employeeService = employeeService;
        //}
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            //var emps = _employeeService.GetAllEmployees();
            //ViewBag.Emps = emps;
            return View();
        }
    }
}
