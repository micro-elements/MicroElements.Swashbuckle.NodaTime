// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
        /// <returns>Initialized <see cref="Schemas"/> instance.</returns>
        public Schemas CreateSchemas()
        {
            SchemaExamples examples = _settings.SchemaExamples;

            // https://xml2rfc.tools.ietf.org/public/rfc/html/rfc3339.html#anchor14
            return new Schemas
            {
                Instant = () => StringSchema(examples.Instant, "date-time"),
                LocalDate = () => StringSchema(examples.ZonedDateTime.Date, "date"),
                LocalTime = () => StringSchema(examples.ZonedDateTime.TimeOfDay),
                LocalDateTime = () => StringSchema(examples.ZonedDateTime.LocalDateTime),
                OffsetDateTime = () => StringSchema(examples.OffsetDateTime, "date-time"),
                ZonedDateTime = () => StringSchema(examples.ZonedDateTime),
                Interval = () => new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        { ResolvePropertyName(nameof(Interval.Start)), StringSchema(examples.Interval.Start, "date-time") },
                        { ResolvePropertyName(nameof(Interval.End)), StringSchema(examples.Interval.End, "date-time") },
                    },
                },
                DateInterval = () => new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        { ResolvePropertyName(nameof(DateInterval.Start)), StringSchema(examples.DateInterval.Start, "date") },
                        { ResolvePropertyName(nameof(DateInterval.End)), StringSchema(examples.DateInterval.End, "date") },
                    },
                },
                Offset = () => StringSchema(examples.ZonedDateTime.Offset),
                Period = () => StringSchema(examples.Period),
                Duration = () => StringSchema(examples.Interval.Duration),
                OffsetDate = () => StringSchema(examples.OffsetDate),
                OffsetTime = () => StringSchema(examples.OffsetTime),
                DateTimeZone = () => StringSchema(examples.DateTimeZone),
            };
        }

        private OpenApiSchema StringSchema(object exampleObject, string? format = null)
        {
            return new OpenApiSchema
            {
                Type = "string",
                Example = _settings.ShouldGenerateExamples
                    ? new OpenApiString(FormatToJson(exampleObject))
                    : null,
                Format = format
            };
        }

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
