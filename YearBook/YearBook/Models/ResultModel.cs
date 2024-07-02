using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YearBook.EntityCore.Entities;

namespace YearBook.Models
{
    public class ResultModel
    {
        public string[] ActionArgs { get; set; }
        public DateTime? ValidatedDate { get; set; }
        public List<TimeSlot> UnAvailableSlots { get; set; }
        public ConcurrentBag<String> errors { get; set; }
        public string SuccessMessage { get; set; }
    }
}
