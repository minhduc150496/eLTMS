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
        //Tạo page cho View Employee-lay tat ca employee shoe tren bang
        [HttpGet]
        public JsonResult GetAllEmployees (String fullName="",int page=1,int pageSize = 20)
        {
            var queryResult = _employeeService.GetAllEmployees(fullName);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);          
        }

        //Update-Edit Employee
        [HttpPost]
        public JsonResult UpdateEmployee(Employee employee)
        {
            var result = _employeeService.Update(employee.EmployeeId, employee.Status, employee.FullName, employee.Gender, employee.DateOfBirth, employee.PhoneNumber, employee.HomeAddress, employee.StartDate, employee.Account.Role);
            return Json(new
            {
                sucess = result
            });
        }
    }
}