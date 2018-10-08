using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Web.Models.dto
{
    public class AppointmentDto
    {
        public int PatientId { get; set; }

        public List<SampleGettingDto> Samples { get; set; }
    }
}