using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YearBook.EntityCore.Entities;
using YearBook.Models;
using YearBook.Utilities.Interfaces;

namespace YearBook.Utilities
{
    internal class DateParser : IDateParser
    {
        public DateTime? ParserToDayMonthUptoFutureYear(string dayArg, string monthArg, int yearsToAdd, DateTime dtm)
        {
            var day = 0;
            var month = 0;
            int.TryParse(dayArg, out day);
            int.TryParse(monthArg, out month);
            DateTime dtmToValidate;
            try
            {
                if (month < dtm.Month
                    || (month == dtm.Month
                        && day < dtm.Day))
                {
                    dtmToValidate = new DateTime(dtm.Year + 1, month, day);
                }
                else
                {
                    dtmToValidate = new DateTime(dtm.Year, month, day);
                }
            }
            catch
            {
                return null;
            }
            return dtmToValidate;
        }

        public DateTime? ParserToHourMin(string hourArg, string minArg, DateTime dtm)
        {
            var hour = 0;
            var min = 0;
            int.TryParse(hourArg, out hour);
            int.TryParse(minArg, out min);
            DateTime dtmToValidate;
            try
            {
                dtmToValidate = new DateTime(dtm.Year, dtm.Month, dtm.Day, hour, min, 0);
            }
            catch
            {
                return null;
            }
            return dtmToValidate;
        }
        public void GetTimeUnAvailableSlots(ResultModel model)
        {
            if (model.ValidatedDate.HasValue)
            {
                return;
            }
            var firstDayofThirdWeek = 15;
            var lastDayofThirdWeek = 21;
            var firstDayOfMonth = new DateTime(model.ValidatedDate.Value.Year, model.ValidatedDate.Value.Month, 1);
            switch (firstDayOfMonth.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    break;
                case DayOfWeek.Monday:
                    firstDayofThirdWeek = +(int)DayOfWeek.Monday;
                    lastDayofThirdWeek = +(int)DayOfWeek.Monday;
                    break;
                case DayOfWeek.Tuesday:
                    firstDayofThirdWeek = +(int)DayOfWeek.Tuesday;
                    lastDayofThirdWeek = +(int)DayOfWeek.Tuesday;
                    break;
                case DayOfWeek.Wednesday:
                    firstDayofThirdWeek = +(int)DayOfWeek.Wednesday;
                    lastDayofThirdWeek = +(int)DayOfWeek.Wednesday;
                    break;
                case DayOfWeek.Thursday:
                    firstDayofThirdWeek = +(int)DayOfWeek.Thursday;
                    lastDayofThirdWeek = +(int)DayOfWeek.Thursday;
                    break;
                case DayOfWeek.Friday:
                    firstDayofThirdWeek = +(int)DayOfWeek.Friday;
                    lastDayofThirdWeek = +(int)DayOfWeek.Friday;
                    break;
                case DayOfWeek.Saturday:
                    firstDayofThirdWeek = +(int)DayOfWeek.Saturday;
                    lastDayofThirdWeek = +(int)DayOfWeek.Saturday;
                    break;
            }
            var firstDateOfThirdWeek = new DateTime(model.ValidatedDate.Value.Year, model.ValidatedDate.Value.Month, firstDayofThirdWeek);
            var lastDateOfThirdWeek = new DateTime(model.ValidatedDate.Value.Year, model.ValidatedDate.Value.Month, lastDayofThirdWeek);
            //Add slot for every second day 
            var dtm = firstDateOfThirdWeek.AddDays((int)DayOfWeek.Monday).Date;
            var day1 = firstDateOfThirdWeek.AddDays((int)DayOfWeek.Monday).Date.Day;
            var day2 = firstDateOfThirdWeek.AddDays((int)DayOfWeek.Wednesday).Date.Day;
            var day3 = firstDateOfThirdWeek.AddDays((int)DayOfWeek.Friday).Date.Day;
            var slot1 = new TimeSlot
            {
                SlotDate = new DateOnly(dtm.Year, dtm.Month, day1),
                SlotStart = new TimeOnly(16, 0),
                SlotEnd = new TimeOnly(17, 0)
            };
            var slot2 = new TimeSlot
            {
                SlotDate = new DateOnly(dtm.Year, dtm.Month, day2),
                SlotStart = new TimeOnly(16, 0),
                SlotEnd = new TimeOnly(17, 0)
            };
            var slot3 = new TimeSlot
            {
                SlotDate = new DateOnly(dtm.Year, dtm.Month, day2),
                SlotStart = new TimeOnly(16, 0),
                SlotEnd = new TimeOnly(17, 0)
            };
            model.UnAvailableSlots.Add(slot1);
            model.UnAvailableSlots.Add(slot2);
            model.UnAvailableSlots.Add(slot3);

        }

        public bool IsTimeInOpenHours(TimeSlot slot, int openHour, int closeHour)
        {
            bool passes = false;
            if (slot.SlotStart.Hour >= openHour
                && slot.SlotStart.Hour < closeHour)
            {
                passes = true;
            }
            return passes;
        }

        public TimeSlot GetTimeSlot(ResultModel model)
        {
            var dayMonthArgs = model.ActionArgs[1].Split(@"/");
            var day = 0;
            var month = 0;
            int.TryParse(dayMonthArgs[0], out day);
            int.TryParse(dayMonthArgs[1], out month);
            var dtm = ParserToDayMonthUptoFutureYear(dayMonthArgs[0], dayMonthArgs[1], 1, DateTime.Now);
            var dateOnly = new DateOnly(dtm.Value.Year, month, day);
            var hour = 0;
            var min = 0;
            if (model.ActionArgs.Length > 2)
            {
                var hourMinArgs = model.ActionArgs[2].Split(@":");

                int.TryParse(hourMinArgs[0], out hour);
                int.TryParse(hourMinArgs[1], out min);
            }
            var timeOnly = new TimeOnly(hour, min);
            var timeSlot = new TimeSlot
            {
                SlotDate = dateOnly,
                SlotStart = timeOnly
            };
            return timeSlot;
        }
    }
}
