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
    public class WareHouseController : Controller
    {
        // GET: WareHouse
        private readonly ISupplyService _supplyService;
        
        private readonly IImportPaperService _importPaperService;
        public WareHouseController(ISupplyService supplyService, IImportPaperService importPaperService)
        {
            this._supplyService = supplyService;
            this._importPaperService = importPaperService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Supplies()
        {
            return View();
        }

        public ActionResult ImportSupplies()
        {
            return View();
        }

        public ActionResult ExportSupplies()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllSupplies(string suppliesCode = "",int page = 1,int pageSize = 20)
        {
            var queryResult = _supplyService.GetAllSupplies(suppliesCode);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Supply>, IEnumerable<SupplyDto>>(queryResult.Skip((page-1) * pageSize).Take(pageSize)) ;
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllImportPapers(string createDate = "", int page = 1, int pageSize = 20)
        {
            var queryResult = _importPaperService.GetAllImportPapers(createDate);
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<ImportPaper>, IEnumerable<ImportPaperDto>>(queryResult.Skip((page - 1) * pageSize).Take(pageSize));
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadPaperImportDetailId(int id)
        {
                     var queryResult = _importPaperService.GetImportPaperById(id);
            var importPaper = Mapper.Map<ImportPaper, ImportPaperDto>(queryResult);
            return Json(new
            {
                success = true,
                data = importPaper,
                            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddSupply(Supply supply)
        {
            var result =  _supplyService.AddSupply(supply);
            return Json(new
            {
                sucess = result
            });
        }
        [HttpGet]
        public JsonResult SupplyDetail(int id)
        {
            var result = _supplyService.GetSupplyById(id);
            var supply = Mapper.Map<Supply,SupplyDto>(result);
            return Json(new
            {
                sucess = true,
                data = supply
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateSupply(Supply supply)
        {
            var result = _supplyService.Update(supply.SuppliesId, supply.SuppliesCode, supply.SuppliesName,supply.SuppliesTypeId.Value,supply.Unit,supply.Note);
            return Json(new
            {
                sucess = result
            });
        }

        [HttpGet]
        public JsonResult GetAllSupply()

        {
            var queryResult = _supplyService.GetAllSupplies(string.Empty);

            var result = queryResult.Select(x => new { x.SuppliesId, x.SuppliesName, x.SuppliesCode , x.Unit}).ToList();
            return Json(new
            {
                success = true,
                data = result, 
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddImportPaper(ImportPaper importPaper)
        {
            var result = _importPaperService.AddImportPaper(importPaper);
            return Json(new
            {
                success = true,
                data = result,
            });
        }
        [HttpPost]
        public JsonResult Delete(int supplyId)
        {
            var result = _supplyService.Delete(supplyId);
            return Json(new
            {
                success = result
            });
        }
        [HttpGet]
        public FileResult DownloadImportPaperTemplate()
        { 
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/templates/ImportPaperTemplate.xlsx"));
            string fileName = "ImportPaperTemplate.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}