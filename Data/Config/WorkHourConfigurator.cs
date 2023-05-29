using Feipder.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feipder.Data.Config
{
    public class WorkHourConfigurator : IEntityTypeConfiguration<WorkHour>
    {
        public void Configure(EntityTypeBuilder<WorkHour> builder)
        {
            builder.HasData(
                new WorkHour
                {
                    Id = 1,
                    Day = DayOfWeek.Monday,
                    From = new TimeOnly(hour: 09, 0, 0, 0, 0),
                    To = new TimeOnly(hour: 20, 0, 0, 0, 0)
                },
                new WorkHour
                {
                    Id = 2,
                    Day = DayOfWeek.Tuesday,
                    From = new TimeOnly(hour: 09, 0, 0, 0, 0),
                    To = new TimeOnly(hour: 20, 0, 0, 0, 0)
                },
                new WorkHour
                {
                    Id = 3,
                    Day = DayOfWeek.Wednesday,
                    From = new TimeOnly(hour: 09, 0, 0, 0, 0),
                    To = new TimeOnly(hour: 20, 0, 0, 0, 0)
                },
                new WorkHour
                {
                    Id = 4,
                    Day = DayOfWeek.Thursday,
                    From = new TimeOnly(hour: 09, 0, 0, 0, 0),
                    To = new TimeOnly(hour: 20, 0, 0, 0, 0)
                },
                new WorkHour
                {
                    Id = 5,
                    Day = DayOfWeek.Friday,
                    From = new TimeOnly(hour: 09, 0, 0, 0, 0),
                    To = new TimeOnly(hour: 20, 0, 0, 0, 0)
                },
                new WorkHour
                {
                    Id = 6,
                    Day = DayOfWeek.Saturday,
                    From = new TimeOnly(hour: 10, 0, 0, 0, 0),
                    To = new TimeOnly(hour: 18, 0, 0, 0, 0)
                },
                new WorkHour
                {
                    Id = 7,
                    Day = DayOfWeek.Sunday,
                    From = new TimeOnly(hour: 10, 0, 0, 0, 0),
                    To = new TimeOnly(hour: 16, 0, 0, 0, 0)
                }
            );
        }
    }
}
