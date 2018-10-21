using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class SampleDto
    {
        public int SampleId { get; set; }

        public string SampleName { get; set; }
        
        public List<LabTestDto> LabTests { get; set; }

        public int SampleDuration { get; set; }

        public int OpenTime { get; set; }

        public int CloseTime { get; set; }

    }
}