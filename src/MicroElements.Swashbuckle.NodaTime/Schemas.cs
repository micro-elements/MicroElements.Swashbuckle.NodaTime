// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.OpenApi.Models;

namespace MicroElements.Swashbuckle.NodaTime
{
    /// <summary>
    /// Swagger schema generators.
    /// </summary>
    public class Schemas
    {
        /// <summary>
        /// Gets or sets schema generator for <see cref="Instant"/>.
        /// </summary>
        public Func<OpenApiSchema> Instant { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="LocalDate"/>.
        /// </summary>
        public Func<OpenApiSchema> LocalDate { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="LocalTime"/>.
        /// </summary>
        public Func<OpenApiSchema> LocalTime { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="LocalDateTime"/>.
        /// </summary>
        public Func<OpenApiSchema> LocalDateTime { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="OffsetDateTime"/>.
        /// </summary>
        public Func<OpenApiSchema> OffsetDateTime { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="ZonedDateTime"/>.
        /// </summary>
        public Func<OpenApiSchema> ZonedDateTime { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="Interval"/>.
        /// </summary>
        public Func<OpenApiSchema> Interval { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="DateInterval"/>.
        /// </summary>
        public Func<OpenApiSchema> DateInterval { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="Offset"/>.
        /// </summary>
        public Func<OpenApiSchema> Offset { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="Period"/>.
        /// </summary>
        public Func<OpenApiSchema> Period { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="Duration"/>.
        /// </summary>
        public Func<OpenApiSchema> Duration { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="OffsetDate"/>.
        /// </summary>
        public Func<OpenApiSchema> OffsetDate { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="OffsetTime"/>.
        /// </summary>
        public Func<OpenApiSchema> OffsetTime { get; set; } = null!;

        /// <summary>
        /// Gets or sets schema generator for <see cref="DateTimeZone"/>.
        /// </summary>
        public Func<OpenApiSchema> DateTimeZone { get; set; } = null!;
    }
}
