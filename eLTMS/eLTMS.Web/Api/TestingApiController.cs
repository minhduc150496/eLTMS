using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Web.Models.dto;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eLTMS.Web.Api
{
    public class TestingApiController : ApiController
    {

        private readonly ITestingService _testingService;
        public TestingApiController(ITestingService testingService)
        {
            this._testingService = testingService;
        }

        [HttpGet]
        [Route("api/testing/get-all")]
        public HttpResponseMessage TestingGetAll()
        {
            var results = _testingService.GetAll();
            var result = Mapper.Map<IEnumerable<Testing>, IEnumerable<TestingDto>>(results);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        [Route("api/testing/create")]
        public HttpResponseMessage Create(Testing newTesting)
        {
            var success = _testingService.Create(newTesting);
            var response = Request.CreateResponse(HttpStatusCode.OK, success);
            return response;
        }

        [HttpPost]
        [Route("api/testing/create-many")]
        public HttpResponseMessage CreateMany(List<Testing> newTestings)
        {
            var success = _testingService.CreateMany(newTestings);
            var response = Request.CreateResponse(HttpStatusCode.OK, success);
            return response;
        }

        [HttpPut]
        [Route("api/testing/update")]
        public HttpResponseMessage Update(Testing testing)
        {
            var success = _testingService.Update(testing);
            var response = Request.CreateResponse(HttpStatusCode.OK, success);
            return response;
        }

        [HttpDelete]
        [Route("api/testing/delete")]
        public HttpResponseMessage Delete(int id)
        {
            var success = _testingService.Delete(id);
            var response = Request.CreateResponse(HttpStatusCode.OK, success);
            return response;
        }

    }
}