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
    internal class ValidatePartDate : IValidatePartDate
    {
        private readonly IDateParser dateParser;
        public ValidatePartDate(IDateParser dateParser)
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
            if (!RegExpChecks.CheckDayMonth(model.ActionArgs[1]))
            {
                model.errors.Append($"{model.ActionArgs[0]} date has incorrect format!");
                return;
            }
            var dayMonthArgs = model.ActionArgs[1].Split(@"/");
            var addDate = dateParser.ParserToDayMonthUptoFutureYear(dayMonthArgs[0], dayMonthArgs[1], 1, DateTime.Now);

            if (addDate is null)
            {
                model.errors.Append($"{model.ActionArgs[0]} date is not a valid date!");
                return;
            }
            model.ValidatedDate = addDate;
        }
    }
}
