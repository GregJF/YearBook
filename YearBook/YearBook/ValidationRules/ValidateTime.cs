using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YearBook.Models;
using YearBook.Utilities.Interfaces;
using YearBook.Utilities;
using YearBook.ValidationRules.Interfaces;

namespace YearBook.ValidationRules
{
    internal class ValidateTime : IValidateTime
    {
        private readonly IDateParser dateParser;
        public ValidateTime(IDateParser dateParser)
        {
            this.dateParser = dateParser;
        }
        public void Validate(ResultModel model)
        {
            if (model.ActionArgs == null
                || model.ActionArgs.Length == 0
              || model.ActionArgs.Length > 2)
            {
                model.errors.Append("Action has wrong number of parameters!");
                return;
            }
            if (RegExpChecks.CheckHourMin(model.ActionArgs[2]))
            {
                model.errors.Append($"{model.ActionArgs[0]} time has incorrect format!");
                return;
            }
            var hourMinArgs = model.ActionArgs[1].Split(@"/");
            var addDate = dateParser.ParserToHourMin(hourMinArgs[0], hourMinArgs[1], DateTime.Now);
            if (addDate is null)
            {
                model.errors.Append($"{model.ActionArgs[0]} date time is not a valid date time!");
                return;
            }
            model.ValidatedDate = addDate;
        }
    }
}
