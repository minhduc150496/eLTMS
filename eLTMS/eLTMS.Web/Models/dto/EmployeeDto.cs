using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Web.Models.dto
{
    public class EmployeeDto
    {
        public int EmployeeID { get; set; } // PatientID (Primary key)
        public int? AccountId { get; set; } // AccountID
        public string Status { get; set; }//Status of Employee
        public string FullName { get; set; } // FullName (length: 100)
        public string Gender { get; set; } // Gender (length: 10)
        public System.DateTime? DateOfBirth { get; set; } // DateOfBirth
        public string PhoneNumber { get; set; } // PhoneNumber (length: 20)
        public string HomeAddress { get; set; } // HomeAddress (length: 200)
        public System.DateTime? DateOfStart { get; set; } // Date Start Of Work
        public bool? IsDeleted { get; set; } // IsDeleted
    }
}