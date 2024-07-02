using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using YearBook.Application.Interfaces;
using YearBook.EntityCore.Entities;
using YearBook.Factories.Interfaces;
using YearBook.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YearBook.Application
{
    internal class Appointment : IAppointment
    {
        private readonly ITimeSlotService timeSlotService;
        public Appointment(ITimeSlotService timeSlotService)
        {
            this.timeSlotService = timeSlotService;
        }



        public async Task RunDateBook(string[] args)
        {

            if (args.Length == 0
                || args.Length > 3)
            {
                DisplayError("Incorrect Parameters");
                return;
            }
            var resultModel = new ResultModel
            {
                ActionArgs = args,
                errors = new System.Collections.Concurrent.ConcurrentBag<string>(),
                UnAvailableSlots = new List<TimeSlot>(),

            };
            var validCommand = false;
            try
            {
                switch (resultModel.ActionArgs[0].ToUpper())
                {
                    case "ADD":
                        validCommand = true;
                        await timeSlotService.Add(resultModel);
                        break;
                    case "DELETE":
                        validCommand = true;
                        await timeSlotService.Delete(resultModel);
                        break;
                    case "FIND":
                        validCommand = true;
                        await timeSlotService.Find(resultModel);
                        break;
                    case "KEEP":
                        validCommand = true;
                        await timeSlotService.Keep(resultModel);
                        break;
                    default:
                        break;
                }
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("UnExpected Error, see Administrator");
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (!validCommand)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Incorrect Command");
                Console.ForegroundColor = ConsoleColor.White;

                DisplayError("");
            }
            else if (resultModel.errors.Any())
            {
                foreach (var error in resultModel.errors)
                {
                    Console.WriteLine(error);
                }
            } else
            {
                if (!string.IsNullOrEmpty(resultModel.SuccessMessage))
                {
                    Console.WriteLine(resultModel.SuccessMessage);
                }
            
            }
            return;

        }
        private void DisplayError(string header)
        {
            Console.WriteLine(header);
            Console.WriteLine("");
            Console.WriteLine("Year Book command parameter [parameter]");
            Console.WriteLine("");
            Console.WriteLine("      ADD DD/MM hh:mm       to add an appointment.");
            Console.WriteLine("      DELETE DD/MM hh:mm    to remove an appointment.");
            Console.WriteLine("      FIND DD/MM            to find a free timeslot for the day");
            Console.WriteLine("      KEEP hh:mm            keep a timeslot for any day.");
        }


    }
}
