using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YearBook.Factories.Interfaces
{
    public interface IAbstractFactory
    {
        TService? GetService<TService>();
    }
}
