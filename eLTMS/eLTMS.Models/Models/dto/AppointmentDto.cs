using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class AppointmentDto
    {
        public int AppCode { get; set; }

        public string PatientName { get; set; }

        public string testPurpose { get; set; }

        public List<SampleGettingDto> SampleGetting { get; set; }
    }
}