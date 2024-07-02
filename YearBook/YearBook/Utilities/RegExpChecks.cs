using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YearBook.Constants;

namespace YearBook.Utilities
{
    internal static class RegExpChecks
    {
        public static bool CheckDayMonth(String dayMonth)
        {
            Regex rg = new Regex(Consts.RegExpPatterns.DAY_MONTH);
            return rg.IsMatch(dayMonth);
        }
        public static bool CheckHourMin(String hourMin)
        {
            Regex rg = new Regex(Consts.RegExpPatterns.HOUR_MIN);
            return rg.IsMatch(hourMin);
        }
    }
}
