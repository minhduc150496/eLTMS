﻿using AutoMapper;
using eLTMS.Models.Models.dto;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eLTMS.Web.Controllers
{
    public class LabTestController : Controller
    {
        //private readonly IExportPaperService _exportPaperService;
        private readonly ISampleService _sampleService;
        private readonly ISampleGroupService _sampleGroupService;
        private readonly ILabTestService _labTestService;

        //private readonly IImportPaperService _importPaperService;
        public LabTestController(ILabTestService labTestService, ISampleService sampleService, ISampleGroupService sampleGroupService)
        {
            this._labTestService = labTestService;
            this._sampleService = sampleService;
            this._sampleGroupService = sampleGroupService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LabTests()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAllSamples( int page = 1, int pageSize = 20)
        {
            var queryResult = _sampleService.GetAll();
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Sample>, IEnumerable<SampleDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllSampleGroups(int page = 1, int pageSize = 20)
        {
            var queryResult = _sampleGroupService.GetAll();
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<SampleGroup>, IEnumerable<SampleGroupDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllLabTests(string code = "", int page = 1, int pageSize = 20)
        {
            var queryResult = _labTestService.GetAllLabTests(code);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<LabTest>, IEnumerable<LabTestDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateLabTest(LabTest labTest)
        {
            var result = _labTestService.Update(labTest);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpPost]
        public JsonResult AddLabTest(LabTest labTest)
        {
            var result = _labTestService.AddLabTest(labTest);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpPost]
        public JsonResult AddSample(Sample sample)
        {
            var result = _sampleService.AddSample(sample);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpPost]
        public JsonResult AddSampleGroup(SampleGroup sample)
        {
            var result = _sampleGroupService.AddSampleGroup(sample);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpGet]
        public JsonResult LabTestDetail(int id)
        {
            var result = _labTestService.GetLabTestById(id);
            var labTest = Mapper.Map<LabTest, LabTestDto>(result);
            return Json(new
            {
                sucess = true,
                data = labTest
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteLabTest(int id)
        {
            var result = _labTestService.Delete(id);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult DeleteSample(int id)
        {
            var result = _sampleService.Delete(id);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult DeleteSampleGroup(int id)
        {
            var result = _sampleGroupService.Delete(id);
            return Json(new
            {
                success = result
            });
        }
    }
}