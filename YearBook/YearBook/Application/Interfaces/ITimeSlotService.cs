using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YearBook.Models;

namespace YearBook.Application.Interfaces
{
    public interface ITimeSlotService
    {
        Task Add(ResultModel model);
        Task Delete(ResultModel model);
        Task Find(ResultModel model);
        Task Keep(ResultModel model);
    }
}
