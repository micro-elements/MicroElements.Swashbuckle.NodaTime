// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;

namespace MicroElements.Swashbuckle.NodaTime
{
    /// <summary>
    /// Factory for <see cref="Schemas"/>.
    /// </summary>
    public class SchemasFactory
    {
        private readonly NodaTimeSchemaSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemasFactory"/> class.
        /// </summary>
        /// <param name="settings"><see cref="JsonSerializerSettings"/> for serializing examples and for <see cref="IContractResolver"/>.</param>
        public SchemasFactory(NodaTimeSchemaSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Creates schemas container.
        /// </summary>
        /// <returns>Initialized <see cref="Schema"/> instance.</returns>
        public Schemas CreateSchemas()
        {
            IDateTimeZoneProvider dateTimeZoneProvider = _settings.DateTimeZoneProvider ?? DateTimeZoneProviders.Tzdb;
            var dateTimeZone = dateTimeZoneProvider.GetSystemDefault();
            var instant = Instant.FromDateTimeUtc(DateTime.UtcNow);
            var zonedDateTime = instant.InZone(dateTimeZone);
            var interval = new Interval(instant,
                instant.PlusTicks(TimeSpan.TicksPerDay)
                    .PlusTicks(TimeSpan.TicksPerHour)
                    .PlusTicks(TimeSpan.TicksPerMinute)
                    .PlusTicks(TimeSpan.TicksPerSecond)
                    .PlusTicks(TimeSpan.TicksPerMillisecond));
            var dateInterval = new DateInterval(zonedDateTime.Date, zonedDateTime.Date.PlusDays(1));
            Period period = Period.Between(zonedDateTime.LocalDateTime, interval.End.InZone(dateTimeZone).LocalDateTime, PeriodUnits.AllUnits);
            var offsetDate = new OffsetDate(zonedDateTime.Date, zonedDateTime.Offset);
            var offsetTime = new OffsetTime(zonedDateTime.TimeOfDay, zonedDateTime.Offset);

            // https://xml2rfc.tools.ietf.org/public/rfc/html/rfc3339.html#anchor14
            return new Schemas
            {
                Instant = () => StringSchema(instant, "date-time"),
                LocalDate = () => StringSchema(zonedDateTime.Date, "full-date"),
                LocalTime = () => StringSchema(zonedDateTime.TimeOfDay, "partial-time"),
                LocalDateTime = () => StringSchema(zonedDateTime.LocalDateTime),
                OffsetDateTime = () => StringSchema(instant.WithOffset(zonedDateTime.Offset), "date-time"),
                ZonedDateTime = () => StringSchema(zonedDateTime),
                Interval = () => new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        { ResolvePropertyName(nameof(Interval.Start)), StringSchema(interval.Start, "date-time") },
                        { ResolvePropertyName(nameof(Interval.End)), StringSchema(interval.End, "date-time") },
                    },
                },
                DateInterval = () => new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        { ResolvePropertyName(nameof(DateInterval.Start)), StringSchema(dateInterval.Start, "full-date") },
                        { ResolvePropertyName(nameof(DateInterval.End)), StringSchema(dateInterval.End, "full-date") },
                    },
                },
                Offset = () => StringSchema(zonedDateTime.Offset, "time-numoffset"),
                Period = () => StringSchema(period),
                Duration = () => StringSchema(interval.Duration),
                OffsetDate = () => StringSchema(offsetDate),
                OffsetTime = () => StringSchema(offsetTime),
                DateTimeZone = () => StringSchema(dateTimeZone),
            };
        }

        private OpenApiSchema StringSchema(object exampleObject, string format = null) => new OpenApiSchema
        {
            Type = "string",
            Example = new OpenApiString(FormatToJson(exampleObject)),
            Format = format,
        };

        private string ResolvePropertyName(string propertyName)
        {
            return _settings.ResolvePropertyName(propertyName);
        }

        private string FormatToJson(object value)
        {
            return _settings.FormatToJson(value);
        }
    }
}
