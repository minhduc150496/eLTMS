using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class SampleGettingForReceptionistDto
    {
        public int SampleGettingId { get; set; }

        public string AppointmentCode { get; set; }

        public string PatientName { get; set; }

        public string PatientPhone { get; set; }

        public string PatientAddress { get; set; }
        
        public string FmStartTime { get; set; }

        public string FmFinishTime { get; set; }

        public string TableName { get; set; }

        public bool IsPaid { get; set; }

    }
}