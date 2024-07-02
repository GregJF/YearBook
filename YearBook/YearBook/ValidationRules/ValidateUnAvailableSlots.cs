using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YearBook.Constants;
using YearBook.EntityCore.Entities;
using YearBook.Models;
using YearBook.Utilities.Interfaces;
using YearBook.ValidationRules.Interfaces;

namespace YearBook.ValidationRules
{
    internal class ValidateUnAvailableSlots : IValidateUnAvailableSlots
    {
        private readonly IDateParser dateParser;
        public ValidateUnAvailableSlots(IDateParser dateParser)
        {
            this.dateParser = dateParser;
        }
        public void Validate(ResultModel model)
        {
            var inUnavailable = false;
            var timeSlot = dateParser.GetTimeSlot(model);
            foreach (var slot in model.UnAvailableSlots.Where(w=> w.SlotDate == timeSlot.SlotDate).ToList())
            {
                if(slot.SlotStart.Hour == timeSlot.SlotStart.Hour
                    && slot.SlotStart.Minute <= timeSlot.SlotStart.Minute
                    && slot.SlotEnd.Minute >= timeSlot.SlotStart.Minute)
                {
                    inUnavailable = true;
                    break;
                }
            };
            if (inUnavailable) {
                model.errors.Add(Consts.ErrorMessages.UNAVIALABLE_SLOT);
            }
        }
    }
}
