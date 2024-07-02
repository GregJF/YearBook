using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YearBook.EntityCore.Entities;
using YearBook.EntityCore.Interfaces;

namespace YearBook.DBWrap.Interfaces
{
    public interface ITimeSlotDBWrap : IAppointmentDBWrap<TimeSlot>
    {
        
    }
}
