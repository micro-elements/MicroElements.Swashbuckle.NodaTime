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
        public DateInterval DateInterval { get; set; }
        public Period Period { get; set; }
        public ZonedDateTime ZonedDateTime { get; set; }
        public OffsetDateTime OffsetDateTime { get; set; }
        public LocalDate LocalDate { get; set; }
        public LocalTime LocalTime { get; set; }
        public LocalDateTime LocalDateTime { get; set; }
        public Offset Offset { get; set; }
        public Duration Duration { get; set; }
        public OffsetDate OffsetDate { get; set; }
        public OffsetTime OffsetTime { get; set; }
    }
}
