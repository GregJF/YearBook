using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YearBook.EntityCore.Interfaces;

namespace YearBook.EntityCore.Entities
{
    public class TimeSlot : IEntity
    {
        public int TimeSlotID { get; set; }
        public DateOnly SlotDate { get; set; }
        public TimeOnly SlotStart { get; set; }
        public TimeOnly SlotEnd { get; set; }
        public bool? Deleted { get; set; }
        public bool? Kept { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
