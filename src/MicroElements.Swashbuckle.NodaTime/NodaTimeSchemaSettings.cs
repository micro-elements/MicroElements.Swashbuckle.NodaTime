// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using NodaTime;

namespace MicroElements.Swashbuckle.NodaTime
{
    /// <summary>
    /// Settings that control serialization aspects.
    /// </summary>
    public class NodaTimeSchemaSettings
    {
        /// <summary>
        /// Gets a function that resolves property name by proper naming strategy.
        /// </summary>
        public Func<string, string> ResolvePropertyName { get; }

        /// <summary>
        /// Gets a function that formats object as json text.
        /// </summary>
        public Func<object, string> FormatToJson { get; }

        /// <summary>
        /// Gets <see cref="IDateTimeZoneProvider"/> configured in Startup.
        /// </summary>
        public IDateTimeZoneProvider DateTimeZoneProvider { get; }

        /// <summary>
        /// Should the example node be generated.
        /// </summary>
        public bool ShouldGenerateExamples { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodaTimeSchemaSettings"/> class.
        /// </summary>
        /// <param name="resolvePropertyName">Function that resolves property name by proper naming strategy.</param>
        /// <param name="formatToJson">Function that formats object as json text.</param>
        /// <param name="shouldGenerateExamples">Should the example node be generated.</param>
        /// <param name="dateTimeZoneProvider"><see cref="IDateTimeZoneProvider"/> configured in Startup.</param>
        public NodaTimeSchemaSettings(
            Func<string, string> resolvePropertyName,
            Func<object, string> formatToJson,
            bool shouldGenerateExamples,
            IDateTimeZoneProvider dateTimeZoneProvider = null)
        {
            ResolvePropertyName = resolvePropertyName;
            FormatToJson = formatToJson;
            ShouldGenerateExamples = shouldGenerateExamples;
            DateTimeZoneProvider = dateTimeZoneProvider;
        }
    }
}
