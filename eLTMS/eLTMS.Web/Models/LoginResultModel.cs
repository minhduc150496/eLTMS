using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Web.Models
{
    public class LoginResultModel
    {
        public string phoneNumber { get; set; }

        public string fullname { get; set; }

        public bool result;
    }
}