using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YearBook.Constants
{
    public class Consts
    {
        public static class RegExpPatterns
        {
            public static string DAY_MONTH = @"\d{2}\/\d{2}";
            public static string HOUR_MIN = @"\d{2}\:\d{2}";
        }
        public static class ErrorMessages
        {
            public static string UNAVIALABLE_SLOT = "The Appointment time is not available";
            public static string SUCCESS_MESSAGE = "The Appointment time ";
        }
    }
}
