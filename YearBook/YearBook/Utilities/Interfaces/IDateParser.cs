using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YearBook.EntityCore.Entities;
using YearBook.Models;

namespace YearBook.Utilities.Interfaces
{
    public interface IDateParser
    {
        DateTime? ParserToDayMonthUptoFutureYear(string dayArg, string monthArg, int yearsToAdd, DateTime dtm);
        DateTime? ParserToHourMin(string hourArg, string minArg, DateTime dtm);
        void GetTimeUnAvailableSlots(ResultModel model);
        bool IsTimeInOpenHours(TimeSlot slot, int openHour, int closeHour);
        TimeSlot GetTimeSlot(ResultModel model);
    }
}
