using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Web.Models.dto
{
    public class SampleGettingDto
    {
        public int SampleId { get; set; }

        public List<int> LabTestIds { get; set; }

        public string SampleName { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime FinishTime { get; set; }

    }
}