using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class DateCollection
    {
        public string Date { get; set; }
        public List<SampleGettingSlot> SampleGettingSlots { get; set; }
    }
}
