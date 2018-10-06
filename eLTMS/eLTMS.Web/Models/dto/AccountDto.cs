using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Web.Models.dto
{
    public class AccountDto
    {
        public string FullName { get; set; }
        
        public string PhoneNumber { get; set; }

        public string AvatarURL { get; set; }
    }
}