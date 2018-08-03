using System;
using NodaTime;

namespace WebApiSample.Controllers
{
    public class NodaTimeModel
    {
        public DateTime DateTime { get; set; }

        public DateTimeZone DateTimeZone { get; set; }
        public Instant Instant { get; set; }
        public Interval Interval { get; set; }
        public Period Period { get; set; }
        public ZonedDateTime ZonedDateTime { get; set; }
        public OffsetDateTime OffsetDateTime { get; set; }
        public LocalDate LocalDate => ZonedDateTime.Date;
        public LocalTime LocalTime => ZonedDateTime.TimeOfDay;
        public LocalDateTime LocalDateTime => ZonedDateTime.LocalDateTime;
        public Offset Offset => ZonedDateTime.Offset;
        public Duration Duration => Interval.Duration;
    }
}