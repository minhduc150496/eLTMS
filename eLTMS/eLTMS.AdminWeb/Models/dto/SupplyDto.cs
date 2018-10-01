using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.AdminWeb.Models.dto
{
    public class SupplyDto
    {
        
        public int SuppliesId { get; set; } // SuppliesId (Primary key)

        
        public string SuppliesCode { get; set; } // SuppliesCode (length: 50)

        
        public string SuppliesName { get; set; } // SuppliesName (length: 50)

        public string SuppliesTypeName { get; set; }


        public int? Quantity { get; set; } // Quantity

        
        public string Note { get; set; } // Note
    }
}