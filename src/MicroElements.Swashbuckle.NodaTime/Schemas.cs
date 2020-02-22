// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.OpenApi.Models;

namespace MicroElements.Swashbuckle.NodaTime
{
    /// <summary>
    /// Swagger schemas.
    /// </summary>
    public class Schemas
    {
        public Func<OpenApiSchema> Instant { get; set; }

        public Func<OpenApiSchema> LocalDate { get; set; }

        public Func<OpenApiSchema> LocalTime { get; set; }

        public Func<OpenApiSchema> LocalDateTime { get; set; }

        public Func<OpenApiSchema> OffsetDateTime { get; set; }

        public Func<OpenApiSchema> ZonedDateTime { get; set; }

        public Func<OpenApiSchema> Interval { get; set; }

        public Func<OpenApiSchema> DateInterval { get; set; }

        public Func<OpenApiSchema> Offset { get; set; }

        public Func<OpenApiSchema> Period { get; set; }

        public Func<OpenApiSchema> Duration { get; set; }

        public Func<OpenApiSchema> OffsetDate { get; set; }

        public Func<OpenApiSchema> OffsetTime { get; set; }

        public Func<OpenApiSchema> DateTimeZone { get; set; }
    }
}
