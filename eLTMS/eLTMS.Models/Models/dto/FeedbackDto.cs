using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; } // FeedbackID (Primary key)
        public int? PatientName { get; set; } // PatientID
        public int? EmployeeName { get; set; } // EmployeeID
        public string Content { get; set; } // Content (length: 500)
        public System.DateTime? ReceivedDateTime { get; set; } // ReceivedDateTime
        public bool? IsDeleted { get; set; } // IsDeleted
        public string Status { get; set; } // Status (length: 100)
    }
}

