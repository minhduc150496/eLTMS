using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class SampleDto
    {
        public string SampleName { get; set; }
        
        public List<LabTestDto> labTests { get; set; }
    }
}