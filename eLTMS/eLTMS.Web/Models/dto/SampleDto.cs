using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Web.Models.dto
{
    public class SampleDto
    {
        public string sampleName { get; set; }

        //public string description { get; set; }

        //public int price { get; set; }

        //public string labTestName { get; set; }

        public List<LabTestDto> labTests { get; set; }
    }
}