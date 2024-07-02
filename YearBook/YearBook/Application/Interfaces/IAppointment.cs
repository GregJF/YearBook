using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YearBook.Application.Interfaces
{
    public interface IAppointment
    {
        Task RunDateBook(string[] args);
    }
}
