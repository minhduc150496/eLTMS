using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Web.Models.dto
{
    public class TestingDto
    {
        public int TestingID { get; set; }

        public string PatientName { get; set; }

        public string LabTestName { get; set; }

        public String BookedDateString
        {
            get; set;
        }
        public String BookedTimeString
        {
            get; set;

        }
    }
}