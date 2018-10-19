using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Utils
{
    public class DateTimeUtils
    {
        // DucBM
        public static string ConvertDateToString(DateTime? dateTime)
        {
            if (dateTime==null)
            {
                return null;
            }
            var dt = (DateTime)dateTime;
            var s = dt.ToString("yyyy-MM-dd");
            return s;
        }

        public static DateTime? ConvertStringToDate(string s)
        {
            try
            {
                DateTime dt = DateTime.Parse(s);
                return (DateTime?)dt;
            } catch (Exception ex)
            {
                // 
            }
            return null;
        }
    }
}
