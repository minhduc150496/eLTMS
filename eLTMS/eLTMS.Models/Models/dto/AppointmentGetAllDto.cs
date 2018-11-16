using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class AppointmentGetAllDto
    {
        public string AppointmentCode { get; set; }

        public string PatientName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
        public DateTime Date { get; set; }
        public string Conclusion { get; set; }
        public List<SampleGettingDto> SampleGettingDtos { get; set; }
    }
}