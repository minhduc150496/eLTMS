using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Web.Models.dto
{
    public class SampleDto
    {
        public int sampleId { get; set; }

        public string sampleName { get; set; }
        
        public List<LabTestDto> labTests { get; set; }

        public int sampleDuration { get; set; }
    }
}