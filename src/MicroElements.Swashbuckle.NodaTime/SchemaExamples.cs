// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using NodaTime;

namespace MicroElements.Swashbuckle.NodaTime
{
    /// <summary>
    /// Schema examples for schema generation.
    /// </summary>
    public class SchemaExamples
    {
        /// <summary>
        /// Gets or sets <see cref="DateTimeZone"/> example.
        /// </summary>
        public DateTimeZone DateTimeZone { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Instant"/> example.
        /// </summary>
        public Instant Instant { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ZonedDateTime"/> example.
        /// </summary>
        public ZonedDateTime ZonedDateTime { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Interval"/> example.
        /// </summary>
        public Interval Interval { get; set; }

        /// <summary>
        /// Gets or sets <see cref="DateInterval"/> example.
        /// </summary>
        public DateInterval DateInterval { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Period"/> example.
        /// </summary>
        public Period Period { get; set; }

        /// <summary>
        /// Gets or sets <see cref="OffsetDate"/> example.
        /// </summary>
        public OffsetDate OffsetDate { get; set; }

        /// <summary>
        /// Gets or sets <see cref="OffsetTime"/> example.
        /// </summary>
        public OffsetTime OffsetTime { get; set; }

        /// <summary>
        /// Gets or sets <see cref="OffsetDateTime"/> example.
        /// </summary>
        public OffsetDateTime OffsetDateTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaExamples"/> class.
        /// Creates example value by provided <see cref="DateTime"/> and <see cref="IDateTimeZoneProvider"/>.
        /// </summary>
        /// <param name="dateTimeZoneProvider">IDateTimeZoneProvider instance.</param>
        /// <param name="dateTimeUtc"><see cref="DateTime"/>. If not set then <see cref="DateTime.UtcNow"/> will be used.</param>
        /// <param name="dateTimeZone">Optional DateTimeZone name. If not set SystemDefault will be used.</param>
        public SchemaExamples(
            IDateTimeZoneProvider dateTimeZoneProvider,
            DateTime? dateTimeUtc = null,
            string? dateTimeZone = null)
        {
            DateTime dateTimeUtcValue = dateTimeUtc ?? DateTime.UtcNow;
            if (dateTimeUtcValue.Kind != DateTimeKind.Utc)
                throw new ArgumentException("dateTimeUtc should be UTC", nameof(dateTimeUtc));

            if (dateTimeZone != null)
                DateTimeZone = dateTimeZoneProvider.GetZoneOrNull(dateTimeZone) ?? dateTimeZoneProvider.GetSystemDefault();
            else
                DateTimeZone = dateTimeZoneProvider.GetSystemDefault();

            Instant = Instant.FromDateTimeUtc(dateTimeUtcValue);

            ZonedDateTime = Instant.InZone(DateTimeZone);

            Interval = new Interval(Instant,
                Instant.PlusTicks(TimeSpan.TicksPerDay)
                    .PlusTicks(TimeSpan.TicksPerHour)
                    .PlusTicks(TimeSpan.TicksPerMinute)
                    .PlusTicks(TimeSpan.TicksPerSecond)
                    .PlusTicks(TimeSpan.TicksPerMillisecond));

            DateInterval = new DateInterval(ZonedDateTime.Date, ZonedDateTime.Date.PlusDays(1));

            Period = Period.Between(ZonedDateTime.LocalDateTime, Interval.End.InZone(DateTimeZone).LocalDateTime, PeriodUnits.AllUnits);

            OffsetDate = new OffsetDate(ZonedDateTime.Date, ZonedDateTime.Offset);

            OffsetTime = new OffsetTime(ZonedDateTime.TimeOfDay, ZonedDateTime.Offset);

            OffsetDateTime = Instant.WithOffset(ZonedDateTime.Offset);
        }
    }
}
