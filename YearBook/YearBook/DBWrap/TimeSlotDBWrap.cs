using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YearBook.DBWrap.Interfaces;
using YearBook.EntityCore.Entities;
using YearBook.EntityCore.Interfaces;
using YearBook.EntityCore;

namespace YearBook.DBWrap
{
    internal class TimeSlotDBWrap : AppointmentDbWrap<TimeSlot>, ITimeSlotDBWrap
    {
        public TimeSlotDBWrap(IAppointmentDBContext context) : base(context) { }
    }
}
