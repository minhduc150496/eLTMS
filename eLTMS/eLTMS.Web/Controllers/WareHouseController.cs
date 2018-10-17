using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models.dto;
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
        public WareHouseController(ISupplyService supplyService)
        {
            this._supplyService = supplyService;
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
        public JsonResult GetAllSupplies(int page = 1,int pageSize = 20)
        {
            var queryResult = _supplyService.GetAllSupplies();
            var totalRows = queryResult.Count();
            var result = Mapper.Map<IEnumerable<Supply>, IEnumerable<SupplyDto>>(queryResult.Skip((page-1) * pageSize).Take(pageSize)) ;
            return Json(new
            {
                success = true,
                data = result,
                total = totalRows
            }, JsonRequestBehavior.AllowGet);
        }
    }
}