using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Web.Models
{
    public class LoginResultModel
    {
        public bool LoginSuccess { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
    }
}