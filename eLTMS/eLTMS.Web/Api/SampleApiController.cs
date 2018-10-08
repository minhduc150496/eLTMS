using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Web.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace eLTMS.Web.Api
{
    public class SampleApiController: ApiController
    {
        private readonly ISampleService _sampleService;
        public SampleApiController(ISampleService sampleService)
        {
            this._sampleService = sampleService;
        }

        [HttpGet]
        [Route("api/sample/get-all")]
        public HttpResponseMessage GetAllLabTest()
        {
            var sample = _sampleService.GetAll();
            var sampleDtos = Mapper.Map<IEnumerable<Sample>, IEnumerable<SampleDto>>(sample);
            var response = Request.CreateResponse(HttpStatusCode.OK, sampleDtos);
            return response;
        }
    }
}