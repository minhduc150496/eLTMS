using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Web.Models.dto
{
    public class AppointmentDto
    {
        public int AppId { get; set; }

        public string PatientName { get; set; }

        public List<SampleGettingDto> SampleGettingDtos { get; set; }
    }
}