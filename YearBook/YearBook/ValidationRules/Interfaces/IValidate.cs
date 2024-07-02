using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YearBook.Models;

namespace YearBook.ValidationRules.Interfaces
{
    public interface IValidate
    {
        void Validate(ResultModel model);
    }
}
