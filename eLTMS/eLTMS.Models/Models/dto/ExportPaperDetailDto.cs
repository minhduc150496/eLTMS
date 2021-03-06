﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class ExportPaperDetailDto
    {
        public int ExportPaperDetailId { get; set; } // ImportPaperDetailId (Primary key)
        public int? ExportPaperId { get; set; } // ImportPaperId
        public int? SuppliesId { get; set; } // SuppliesId
        public string Unit { get; set; } // Unit (length: 50)
        public int? Quantity { get; set; } // Quantity
        public string Note { get; set; } // Note
        public bool? IsDeleted { get; set; } // IsDeleted

    }
}