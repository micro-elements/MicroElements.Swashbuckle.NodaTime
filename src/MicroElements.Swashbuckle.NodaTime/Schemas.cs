using Swashbuckle.AspNetCore.Swagger;

namespace MicroElements.Swashbuckle.NodaTime
{
    /// <summary>
    /// Swager schemas.
    /// </summary>
    public class Schemas
    {
        public Schema Instant { get; set; }

        public Schema LocalDate{ get; set; }

        public Schema LocalTime { get; set; }

        public Schema LocalDateTime { get; set; }

        public Schema OffsetDateTime { get; set; }

        public Schema ZonedDateTime { get; set; }

        public Schema Interval { get; set; }

        public Schema DateInterval { get; set; }

        public Schema Offset { get; set; }

        public Schema Period { get; set; }

        public Schema Duration { get; set; }

        public Schema DateTimeZone { get; set; }
    }
}
