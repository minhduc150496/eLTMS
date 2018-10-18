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

        public DateTime GettingDate { get; set; }

        public TimeSpan StartTime { get; set; }
        
        public TimeSpan FinishTime { get; set; }

    }
}