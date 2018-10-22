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
        public string SampleGroupName { get; set; }

        public List<LabTestDto> LabTests { get; set; }

        public int SampleDuration { get; set; }

        public double OpenTime { get; set; }

        public double CloseTime { get; set; }

        public int? SampleGroupId { get; set; } // SampleGroupID
        public string Description { get; set; } // Description (length: 500)
        public bool? IsDeleted { get; set; } // IsDeleted
    }
}