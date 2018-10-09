using AutoMapper;
using eLTMS.AdminWeb.Models.dto;
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
    public class EmployeeController : Controller
    {
        //Nguyen Huu Lam
        // GET: Employee
        //Khai báo IEmployeeService
        private readonly IEmployeeService _employeeService;

        public EmployeeController (IEmployeeService employeeService)
        {
            this._employeeService = employeeService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Employees()
        {
            return View();
        }
        //Tạo page cho View Employee
        [HttpGet]
        public JsonResult GetAllEmployees (int page=1,int pageSize = 20)
        {
            var queryResult = _employeeService.GetAll();
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);         
        }
    }
}