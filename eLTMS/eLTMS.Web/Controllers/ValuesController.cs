using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eLTMS.Web.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        public ValuesController(IEmployeeService employeeService)
        {
            this._employeeService = employeeService;
        }

        [HttpGet]
        [Route("employee/all-employee")]
        public HttpResponseMessage GetAll()
        {
            var results = _employeeService.GetAll();
            var response = Request.CreateResponse(HttpStatusCode.OK, results);
            return response;
        }

        //[HttpGet]
        //[Route("employee/get-name")]
        //public HttpResponseMessage GetName(string name)
        //{
        //    var results = _employeeService.GetByName(name);
        //    var response = Request.CreateResponse(HttpStatusCode.OK, results);
        //    return response;
        //}

        //[HttpPost]
        //[Route("employee/insert")]
        //public HttpResponseMessage Insert(int id,string name,int age)
        //{
        //    var results = _employeeService.Insert(id,name,age);
        //    var response = Request.CreateResponse(HttpStatusCode.OK, results);
        //    return response;
        //}

        //[HttpDelete]
        //[Route("employee/delete")]
        //public HttpResponseMessage Delete(int id)
        //{
        //    var results = _employeeService.Delete(id);
        //    var response = Request.CreateResponse(HttpStatusCode.OK, results);
        //    return response;
        //}

        //[HttpPut]
        //[Route("employee/update")]
        //public HttpResponseMessage Update(int id, string name, int age)
        //{
        //    var results = _employeeService.Update(id, name, age);
        //    var response = Request.CreateResponse(HttpStatusCode.OK, results);
        //    return response;
        //}

    }
}
