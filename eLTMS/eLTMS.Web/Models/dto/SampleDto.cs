using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Web.Models.dto
{
    public class SampleDto
    {
        public int SampleId { get; set; }

        public string SampleName { get; set; }
        
        public List<LabTestDto> LabTests { get; set; }

        public int SampleDuration { get; set; }

        public double OpenTime { get; set; }

        public double CloseTime { get; set; }

    }
}