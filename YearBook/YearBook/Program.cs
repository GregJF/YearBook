using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Extensions.Hosting;
using YearBook.Application;
using YearBook.Application.Interfaces;
using YearBook.DBWrap;
using YearBook.DBWrap.Interfaces;
using YearBook.EntityCore;
using YearBook.EntityCore.Interfaces;
using YearBook.Factories;
using YearBook.Factories.Interfaces;
using YearBook.Utilities;
using YearBook.Utilities.Interfaces;
using YearBook.ValidationRules;
using YearBook.ValidationRules.Interfaces;

namespace YearBook
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Aplication Initialising");

            var appBuilder = buildApp(args);
            var app = appBuilder.Services.GetService<IAppointment>();
            Console.Clear();
            Console.WriteLine("Aplication Starts");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");

            await app.RunDateBook(args);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Aplication Finished");
        }
        private static IHost buildApp(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
             .ConfigureServices((hostContext, services) =>
             {
                 CreateServices(services);
             });

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                //replace DataContext with your Db Context name
                var dataContext = scope.ServiceProvider.GetRequiredService<AppointmentContext>();
                dataContext.Database.Migrate();
            }

            return app;
        }
        private static void CreateServices(IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

            var connectionString = configuration.GetConnectionString("AppointmentConnection");
            services.AddSingleton<IConfiguration>(configuration)
                .AddDbContext<IAppointmentDBContext, AppointmentContext>(o => o.UseSqlServer(connectionString)
                )
                .AddSingleton<IAppointment, Appointment>()
                .AddTransient<IAbstractFactory, AbstractFactory>()
                .AddScoped<ITimeSlotDBWrap, TimeSlotDBWrap>()
                .AddTransient<ITimeSlotService, TimeSlotService>()
                .AddTransient<IDateParser, DateParser>()
                .AddTransient<IValidateFullDate, ValidateFullDate>()
                .AddTransient<IValidatePartDate, ValidatePartDate>()
                .AddTransient<IValidateTime, ValidateTime>()
                .AddTransient<IValidateUnAvailableSlots, ValidateUnAvailableSlots>()
                .BuildServiceProvider();
        }
    }
}
