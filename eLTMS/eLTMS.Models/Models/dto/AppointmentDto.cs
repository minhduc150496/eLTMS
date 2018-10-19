﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class AppointmentDto
    {
        public string AppointmentCode { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; }

        public string TestPurpose { get; set; }
        
        public List<SampleGettingDto> SampleGettingDtos { get; set; }
    }
}