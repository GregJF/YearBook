using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YearBook.Application.Interfaces;
using YearBook.Constants;
using YearBook.DBWrap.Interfaces;
using YearBook.EntityCore.Entities;
using YearBook.Models;
using YearBook.Utilities;
using YearBook.Utilities.Interfaces;
using YearBook.ValidationRules.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YearBook.Application
{
    internal class TimeSlotService : ITimeSlotService
    {
        private readonly IValidateFullDate validateFullDate;
        private readonly IValidateUnAvailableSlots validateUnAvailableSlots;
        private readonly ITimeSlotDBWrap timeSlotDBWrap;
        private readonly IDateParser dateParser;
        
        public TimeSlotService(IValidateFullDate validateFullDate,
            IValidateUnAvailableSlots validateUnAvailableSlots,
            ITimeSlotDBWrap timeSlotDBWrap,
            IDateParser dateParser)
        {
            this.validateFullDate = validateFullDate;
            this.validateUnAvailableSlots = validateUnAvailableSlots;
            this.timeSlotDBWrap = timeSlotDBWrap;
            this.dateParser = dateParser;
        }
        public async Task Add(ResultModel model)
        {
            validateFullDate.Validate(model);
            if (model.errors.Any())
            {
                return;
            }
            validateUnAvailableSlots.Validate(model);
            if (model.errors.Any())
            {
                return;
            }
            var timeslot = dateParser.GetTimeSlot(model);
            if(!dateParser.IsTimeInOpenHours(timeslot, 9, 17))
            {
                model.errors.Add(Consts.ErrorMessages.UNAVIALABLE_SLOT);
                return;
            }

            var inUnavailable = false;
            
            var timeslots = await timeSlotDBWrap.FindByCondition(f => f.SlotDate == timeslot.SlotDate);
            if (timeslots is not null
                && timeslots.Any())
            {
                foreach (var slot in timeslots.Where(w => w.SlotDate == timeslot.SlotDate).ToList())
                {
                    if (slot.SlotStart.Hour == timeslot.SlotStart.Hour
                        && slot.SlotStart.Minute <= timeslot.SlotStart.Minute
                        && slot.SlotEnd.Minute >= timeslot.SlotStart.Minute)
                    {
                        inUnavailable = true;
                        break;
                    }
                };
                if (inUnavailable)
                {
                    model.errors.Add(Consts.ErrorMessages.UNAVIALABLE_SLOT);
                }
            }
            if (model.errors.Any())
            {
                return;
            }
            if(timeslot.SlotStart.Minute > 30)
            {
                timeslot.SlotEnd = new TimeOnly(timeslot.SlotStart.Hour+1,0);
                timeslot.SlotStart = new TimeOnly(timeslot.SlotStart.Hour, 30);
            } else
            {
                timeslot.SlotStart = new TimeOnly(timeslot.SlotStart.Hour, 0);
                timeslot.SlotEnd = new TimeOnly(timeslot.SlotStart.Hour, 30);
            }
            var newTimeSlot = new TimeSlot
            {
                SlotDate = timeslot.SlotDate,
                SlotStart = timeslot.SlotStart,
                SlotEnd = timeslot.SlotEnd,
                CreatedBy = "UserName",
                CreatedOn = DateTime.Now,
            };
            await timeSlotDBWrap.Add(newTimeSlot,true);
            var msg = Consts.ErrorMessages.SUCCESS_MESSAGE + $" on {newTimeSlot.SlotDate.ToString("dd MMM yyyy")} from {newTimeSlot.SlotStart.ToString("HH:mm")} to {newTimeSlot.SlotEnd.ToString("HH:mm")} ";
            model.SuccessMessage = msg ;
        }

        public async Task Delete(ResultModel model)
        {
            TimeSlot slotFound = null;
            var timeslot = dateParser.GetTimeSlot(model);
            var timeslots = await timeSlotDBWrap.FindByCondition(f => f.SlotDate == timeslot.SlotDate);
            if (timeslots is not null
                && timeslots.Any())
            {
                foreach (var slot in timeslots.Where(w => w.SlotDate == timeslot.SlotDate).ToList())
                {
                    if (slot.SlotStart.Hour == timeslot.SlotStart.Hour
                        && slot.SlotStart.Minute <= timeslot.SlotStart.Minute
                        && slot.SlotEnd.Minute >= timeslot.SlotStart.Minute)
                    {
                        slotFound = slot;
                        break;
                    }
                };             
            }
            if (slotFound is null)
            {
                model.errors.Append($"Your Appointment could not be found");
                return;
            }
            slotFound.Deleted = true;
            await timeSlotDBWrap.Remove(slotFound, true);
            model.SuccessMessage = "Your Appointment has been deleted";
        }

        public async Task Find(ResultModel model)
        {
            TimeSlot slotFound = null;
            var timeslot = dateParser.GetTimeSlot(model);
            var timeslots = await timeSlotDBWrap.FindByCondition(f => f.SlotDate == timeslot.SlotDate);
            Tuple<int, int> hourMin = new Tuple<int, int>(0, 0);
            if (timeslots is not null
                && timeslots.Any())
            {
                
                for (var i = 9; i < 17; i++)
                {
                    slotFound = null;
                    slotFound = timeslots.FirstOrDefault(f => f.SlotStart.Hour == i && f.SlotEnd.Minute == 0);
                    if (slotFound is null)
                    {
                        hourMin = new Tuple<int, int>(i, 0);
                        break;
                    }
                    else
                    {
                        slotFound = timeslots.FirstOrDefault(f => f.SlotStart.Hour == i && f.SlotEnd.Minute == 30);
                        if (slotFound is null)
                        {
                            hourMin = new Tuple<int, int>(i, 30);
                            break;
                        }
                    }
                }
                if (hourMin.Item1 == 0 && hourMin.Item2 == 0)
                {
                    model.errors.Append($"There are no appointments available");
                    return;
                }                
            }
            if(hourMin.Item1 == 0)
            {
                hourMin = new Tuple<int, int>(9, 00);
                List<string> tempList = new List<string>();
                tempList.Add(model.ActionArgs[0]);
                tempList.Add(model.ActionArgs[1]);
                tempList.Add("");
                model.ActionArgs = tempList.ToArray();
            }
            model.ActionArgs[2] = hourMin.Item1.ToString("00") + ":" + hourMin.Item2.ToString("00");
        

        var newTimeSlot = dateParser.GetTimeSlot(model);
        var msg = $"There is an appointment available on {newTimeSlot.SlotDate.ToString("dd MMM yyyy")} from {newTimeSlot.SlotStart.ToString("HH:mm")} to {newTimeSlot.SlotEnd.ToString("HH:mm")} ";
        model.SuccessMessage = msg;
            
        }

        public async Task Keep(ResultModel model)
        {
            TimeSlot slotFound = null;
            var dtm = DateTime.Now;
            var second = dtm.Date.Day.ToString("00") + "/" + dtm.Date.Month.ToString("00");
            List<string> tempList = new List<string>();
            tempList.Add(model.ActionArgs[0]);
            tempList.Add(second);
            tempList.Add(model.ActionArgs[1]);
            model.ActionArgs = tempList.ToArray();


            model.ActionArgs[1] = "05/08";
            var timeslot = dateParser.GetTimeSlot(model);
            var timeslots = await timeSlotDBWrap.FindByCondition(f => f.SlotDate == timeslot.SlotDate);
            if (timeslots is not null
                && timeslots.Any())
            {
                foreach (var slot in timeslots.Where(w => w.SlotDate == timeslot.SlotDate).ToList())
                {
                    if (slot.SlotStart.Hour == timeslot.SlotStart.Hour
                        && slot.SlotStart.Minute <= timeslot.SlotStart.Minute
                        && slot.SlotEnd.Minute >= timeslot.SlotStart.Minute)
                    {
                        slotFound = slot;
                        break;
                    }
                }
            }
            if (slotFound is null)
            {
                model.errors.Append($"Your Appointment could not be found");
                return;
            }
            slotFound.Kept = true;
            await timeSlotDBWrap.Update(slotFound, true);
            model.SuccessMessage = "Your Appointment has been Kept";
        }

    }
}
