using AutoMapper;
using eLTMS.Models.Models.dto;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace eLTMS.Web.Controllers
{
    public class LabTestController : Controller
    {
        //private readonly IExportPaperService _exportPaperService;
        private readonly ISampleService _sampleService;
        private readonly ISampleGroupService _sampleGroupService;
        private readonly ILabTestService _labTestService;
        private readonly ILabTestingService _labTestingService;
        private readonly ILabTestingIndexService _labTestingIndexService;
        private readonly IAppointmentService _appointmentService;
        private readonly IHospitalSuggestionService _hospitalSuggestionService;
        public LabTestController(IHospitalSuggestionService hospitalSuggestionService, IAppointmentService appointmentService,ILabTestingIndexService labTestingIndexService, ILabTestingService labTestingService, ILabTestService labTestService, ISampleService sampleService, ISampleGroupService sampleGroupService)
        {
            this._labTestService = labTestService;
            this._appointmentService = appointmentService;
            this._labTestingService = labTestingService;
            this._labTestingIndexService = labTestingIndexService;
            this._sampleService = sampleService;
            this._sampleGroupService = sampleGroupService;
            this._hospitalSuggestionService = hospitalSuggestionService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LabTests()
        {
            return View();
        }
        public ActionResult LabTesting()
        {
            return View();
        }
        public ActionResult LabTestingResult()
        {
            return View();
        }
        public ActionResult LabTestingDone()
        {
            return View();
        }
        public ActionResult Result()
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
        public JsonResult GetAllLabTesting(int page = 1, int pageSize = 20)
        {
            var queryResult = _labTestingService.GetAllLabTesting();
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllLabTestingHaveAppointmentCode(string code = "", int page = 1, int pageSize = 20)
        {
            var queryResult = _labTestingService.GetAllLabTestingHaveAppointmentCode(code);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllLabTestingIndexHaveLabtestingId(int id)
        {
            var queryResult = _labTestingIndexService.GetAllLabTestingIndexHaveLabtestingId(id);
            var result = Mapper.Map<IEnumerable<LabTestingIndex>, IEnumerable<LabTestingIndexDto>>(queryResult);
            return Json(new
            {
                data = result,
                success = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllLabTestingResult(int page = 1, int pageSize = 20)
        {
            var queryResult = _labTestingService.GetAllLabTestingResult();
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllResult(int page = 1, int pageSize = 20)
        {
            var queryResult = _labTestingService.GetAllResult();
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllLabTestings()
        {
            var queryResult = _labTestingService.GetAllLabTesting();
            var result = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult);
            return Json(new
            {
                success = true,
                data = result,
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
        public JsonResult UpdateLabTesting(List<LabTesting> labTesting)
        {
            var result = _labTestingService.Update(labTesting);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult UpdateStatus(List<LabTesting> labTesting)
        {
            var result = _labTestingService.UpdateStatus(labTesting);
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult UpdateResult(string code,string con)
        {
            var result = _appointmentService.Update(code,con);
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
        public JsonResult AddLabTestingIndex(List<LabTestingIndex> labTestingIndex)
        {
            var result = _labTestingIndexService.AddLabTestingIndex(labTestingIndex);
            return Json(new
            {
                success = result
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


        [HttpPost]
        public ActionResult ExportOrderDetailToPdf(string code)
        {
            StringBuilder sb = new StringBuilder();
            var queryResult2 = _appointmentService.GetResultByAppCode(code);
            var result2 = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetAllDto>>(queryResult2);
            var queryResult1 = _labTestingService.GetAllLabTestingHaveAppointmentCode(code);
            var result1 = Mapper.Map<IEnumerable<LabTesting>, IEnumerable<LabTestingDto>>(queryResult1);
            foreach (var item1 in result1)
            {
                var queryResult = _labTestingIndexService.GetAllLabTestingIndexHaveLabtestingId(item1.LabTestingId);
                var result = Mapper.Map<IEnumerable<LabTestingIndex>, IEnumerable<LabTestingIndexDto>>(queryResult);
             
                sb.AppendLine($"<tr><td><h3>{item1.LabTestName}</h3><td></tr>");
                foreach (var item in result)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td class='no'>{item.IndexName}</td>");
                    sb.AppendLine($"<td class='colUnit'>{item.IndexValue}</td>");
                    sb.AppendLine($"<td class='colUnit'>{item.LowNormalHigh}</td>");
                    sb.AppendLine($"<td class='colUnit'>{item.NormalRange}</td>");
                    sb.AppendLine($"<td class='colUnit'>{item.Unit}</td>");
                    sb.AppendLine("</tr>");
                }
            }
            var Renderer = new IronPdf.HtmlToPdf();
            var allData = System.IO.File.ReadAllText(Server.MapPath("~/template-pdf/result.html"));
            foreach (var item2 in result2)
            {
                allData = allData.Replace("{{InvoiceDate}}", $"{item2.Date}");
                allData = allData.Replace("{{ClientName}}", $"{item2.PatientName}");
                allData = allData.Replace("{{ClientCompany}}", $"{item2.Phone}");
                allData = allData.Replace("{{ClientAddress}}", $"{item2.Address}");
                allData = allData.Replace("{{Con}}", $"<h3>{item2.Conclusion}</h3>");
                string x = item2.Conclusion + "";
                var queryResult3 = _hospitalSuggestionService.GetAllHospitalSuggestions(x);
                var result3 = Mapper.Map<IEnumerable<HospitalSuggestion>, IEnumerable<HospitalSuggestionDto>>(queryResult3);
                foreach (var item3 in result3)
                { allData = allData.Replace("{{Hos}}", $"{item3.HospitalList}"); }
            }

            allData = allData.Replace("{{DataResult}}", sb.ToString());
            var PDF = Renderer.RenderHtmlAsPdf(allData);
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", $"attachment;filename=\"Result.pdf\"");
            // edit this line to display ion browser and change the file name
            Response.BinaryWrite(PDF.BinaryData);
            Response.Flush();
            Response.End();
            return null;
        }


    }
}