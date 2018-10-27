using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class SampleGettingDto
    {
        public int SampleId { get; set; }

        public List<int> LabTestIds { get; set; }

        public string SampleName { get; set; }

        public string GettingDate { get; set; }

        public string StartTime { get; set; }
        
        public string FinishTime { get; set; }

    }
}