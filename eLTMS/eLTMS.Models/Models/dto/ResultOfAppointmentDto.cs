using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class ResultOfAppointmentDto
    {
        public int AppointmentId { get; set; }

        public string AppointmentCode { get; set; }

        public bool IsEmergency { get; set; }

        public string DoctorName { get; set; }

        public string PatientName { get; set; }

        public string TestPurpose { get; set; }

        public DateTime EnterTime { get; set; }

        public DateTime ReturnTime { get; set; }

        public List<ResultOfSampleGettingDto> SampleGettings { get; set; }

        public string Conclusion { get; set; } // Conclusion (length: 500)
    }
}