﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class LabTestDto
    {
        public int LabTestId { get; set; }
        public string LabTestName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }        
    }
}