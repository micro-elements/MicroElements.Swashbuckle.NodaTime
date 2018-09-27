// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;
using Swashbuckle.AspNetCore.Swagger;

namespace MicroElements.Swashbuckle.NodaTime
{
    /// <summary>
    /// Factory for <see cref="Schema"/>.
    /// </summary>
    public class SchemasFactory
    {
        private readonly JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemasFactory"/> class.
        /// </summary>
        /// <param name="serializerSettings"><see cref="JsonSerializerSettings"/> for serializing examples and for <see cref="IContractResolver"/>.</param>
        public SchemasFactory(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings;
        }

        /// <summary>
        /// Creates schemas container.
        /// </summary>
        /// <returns>Initialized <see cref="Schema"/> instance.</returns>
        public Schemas CreateSchemas()
        {
            var dateTimeZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
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

            // https://xml2rfc.tools.ietf.org/public/rfc/html/rfc3339.html#anchor14
            return new Schemas
            {
                Instant = () => StringSchema(instant, "date-time"),
                LocalDate = () => StringSchema(zonedDateTime.Date, "full-date"),
                LocalTime = () => StringSchema(zonedDateTime.TimeOfDay, "partial-time"),
                LocalDateTime = () => StringSchema(zonedDateTime.LocalDateTime),
                OffsetDateTime = () => StringSchema(instant.WithOffset(zonedDateTime.Offset), "date-time"),
                ZonedDateTime = () => StringSchema(zonedDateTime),
                Interval = () => new Schema
                {
                    Type = "object",
                    Properties = new Dictionary<string, Schema>
                    {
                        { ResolvePropertyName(_serializerSettings, nameof(Interval.Start)), StringSchema(interval.Start, "date-time") },
                        { ResolvePropertyName(_serializerSettings, nameof(Interval.End)), StringSchema(interval.End, "date-time") },
                    },
                },
                DateInterval = () => new Schema
                {
                    Type = "object",
                    Properties = new Dictionary<string, Schema>
                    {
                        { ResolvePropertyName(_serializerSettings, nameof(DateInterval.Start)), StringSchema(dateInterval.Start, "full-date") },
                        { ResolvePropertyName(_serializerSettings, nameof(DateInterval.End)), StringSchema(dateInterval.End, "full-date") },
                    },
                },
                Offset = () => StringSchema(zonedDateTime.Offset, "time-numoffset"),
                Period = () => StringSchema(period),
                Duration = () => StringSchema(interval.Duration),
                DateTimeZone = () => StringSchema(dateTimeZone),
            };
        }

        private Schema StringSchema(object exampleObject, string format = null) => new Schema
        {
            Type = "string",
            Example = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(exampleObject, _serializerSettings)),
            Format = format
        };

        /// <summary>
        /// Resolves property name according <see cref="DefaultContractResolver.NamingStrategy"/>.
        /// <para>If serializer is not DefaultContractResolver then original propertyName returns.</para>
        /// </summary>
        /// <param name="serializer">The serializer to use name resolve.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Resolved propertyName.</returns>
        internal static string ResolvePropertyName(JsonSerializerSettings serializer, string propertyName)
        {
            return (serializer.ContractResolver as DefaultContractResolver)?.GetResolvedPropertyName(propertyName) ?? propertyName;
        }
    }
}
