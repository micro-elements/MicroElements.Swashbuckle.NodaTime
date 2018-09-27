using System;
using Swashbuckle.AspNetCore.Swagger;

namespace MicroElements.Swashbuckle.NodaTime
{
    /// <summary>
    /// Swager schemas.
    /// </summary>
    public class Schemas
    {
        public Func<Schema> Instant { get; set; }

        public Func<Schema> LocalDate { get; set; }

        public Func<Schema> LocalTime { get; set; }

        public Func<Schema> LocalDateTime { get; set; }

        public Func<Schema> OffsetDateTime { get; set; }

        public Func<Schema> ZonedDateTime { get; set; }

        public Func<Schema> Interval { get; set; }

        public Func<Schema> DateInterval { get; set; }

        public Func<Schema> Offset { get; set; }

        public Func<Schema> Period { get; set; }

        public Func<Schema> Duration { get; set; }

        public Func<Schema> DateTimeZone { get; set; }
    }
}
