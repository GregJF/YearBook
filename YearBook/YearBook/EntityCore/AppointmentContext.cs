using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using YearBook.EntityCore.Entities;
using YearBook.EntityCore.Interfaces;

namespace YearBook.EntityCore
{
    internal class AppointmentContext : DbContext, IAppointmentDBContext
    {
        public AppointmentContext(DbContextOptions<AppointmentContext> dbOptions) : base(dbOptions) { }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TimeSlot>().HasKey(e => new
            {
                e.TimeSlotID
            });
        }
    }
}
