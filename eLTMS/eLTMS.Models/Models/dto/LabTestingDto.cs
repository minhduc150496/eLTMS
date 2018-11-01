using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class LabTestingDto
    {
        public string LabTestName { get; set; }

        public List<LabTestingIndexDto> LabTestingIndexDtos { get; set; }
        public int LabTestingId { get; set; } // LabTestingID (Primary key)
        public int? LabTestId { get; set; } // LabTestID
        public string AppointmentCode { get; set; } // AppointmentCode (length: 20)
        public int SampleId { get; set; } // AppointmentCode (length: 20)
        public string SampleName { get; set; } // Status (length: 20)
        public string Status { get; set; } // Status (length: 20)
        public bool? IsDeleted { get; set; } // IsDeleted
        public int? MachineSlot { get; set; } // MachineSlot
    }
}
