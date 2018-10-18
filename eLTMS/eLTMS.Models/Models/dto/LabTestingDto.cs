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
    }
}
