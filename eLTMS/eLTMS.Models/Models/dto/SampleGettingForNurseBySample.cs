﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class SampleGettingForNurseBySample
    {
        public int SampleGettingId { get; set; }

        public string SampleName { get; set; }

        public string PatientName { get; set; }

        public string Date { get; set; }

        public string StartTime { get; set; }

        public bool? IsGot { get; set; }
    }
}