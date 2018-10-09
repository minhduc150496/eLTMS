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
        
        public List<LabTestDto> labTests { get; set; }
    }
}