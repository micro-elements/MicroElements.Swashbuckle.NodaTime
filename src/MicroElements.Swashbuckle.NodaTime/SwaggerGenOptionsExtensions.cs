// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MicroElements.Swashbuckle.NodaTime
{
    /// <summary>
    /// Extensions for configuring swagger to use NodaTime types.
    /// </summary>
    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Configures swagger to use NodaTime types.
        /// Uses NewtonsoftJson for serialization aspects.
        /// </summary>
        /// <param name="config">SwaggerGenOptions.</param>
        /// <param name="serializerSettings">Optional serializer settings.</param>
        /// <param name="configureSerializerSettings">Optional action to configure serializerSettings.</param>
        /// <param name="dateTimeZoneProvider">Optional DateTimeZoneProviders.</param>
        /// <param name="shouldGenerateExamples">Should generate example for schema.</param>
        /// <param name="schemaExamples"><see cref="SchemaExamples"/> for schema example values.</param>
        public static void ConfigureForNodaTime(
            this SwaggerGenOptions config,
            JsonSerializerSettings? serializerSettings = null,
            Action<JsonSerializerSettings>? configureSerializerSettings = null,
            IDateTimeZoneProvider? dateTimeZoneProvider = null,
            bool shouldGenerateExamples = true,
            SchemaExamples? schemaExamples = null)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            serializerSettings ??= new JsonSerializerSettings();
            configureSerializerSettings?.Invoke(serializerSettings);

            bool isNodaConvertersRegistered = serializerSettings.Converters.Any(converter => converter is NodaConverterBase<Instant>);
            if (!isNodaConvertersRegistered)
            {
                serializerSettings.ConfigureForNodaTime(dateTimeZoneProvider ?? DateTimeZoneProviders.Tzdb);
            }

            var nodaTimeSchemaSettings = serializerSettings.CreateNodaTimeSchemaSettingsForNewtonsoftJson(
                dateTimeZoneProvider: dateTimeZoneProvider,
                shouldGenerateExamples: shouldGenerateExamples,
                schemaExamples: schemaExamples);
            config.ConfigureForNodaTime(nodaTimeSchemaSettings);
        }

        /// <summary>
        /// Configures swagger to use NodaTime types.
        /// Uses System.Text.Json for serialization aspects.
        /// </summary>
        /// <param name="config">SwaggerGenOptions.</param>
        /// <param name="jsonSerializerOptions">Optional serializer options.</param>
        /// <param name="configureSerializerOptions">Optional action to configure jsonSerializerOptions.</param>
        /// <param name="dateTimeZoneProvider">Optional DateTimeZoneProviders.</param>
        /// <param name="shouldGenerateExamples">Should generate example for schema.</param>
        /// <param name="schemaExamples"><see cref="SchemaExamples"/> for schema example values.</param>
        public static void ConfigureForNodaTimeWithSystemTextJson(
            this SwaggerGenOptions config,
            JsonSerializerOptions? jsonSerializerOptions = null,
            Action<JsonSerializerOptions>? configureSerializerOptions = null,
            IDateTimeZoneProvider? dateTimeZoneProvider = null,
            bool shouldGenerateExamples = true,
            SchemaExamples? schemaExamples = null)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            jsonSerializerOptions ??= new JsonSerializerOptions();
            configureSerializerOptions?.Invoke(jsonSerializerOptions);

            global::NodaTime.Serialization.SystemTextJson.Extensions.ConfigureForNodaTime(jsonSerializerOptions,
                dateTimeZoneProvider ?? DateTimeZoneProviders.Tzdb);

            var nodaTimeSchemaSettings = jsonSerializerOptions.CreateNodaTimeSchemaSettingsForSystemTextJson(
                dateTimeZoneProvider: dateTimeZoneProvider,
                shouldGenerateExamples: shouldGenerateExamples,
                schemaExamples: schemaExamples);
            config.ConfigureForNodaTime(nodaTimeSchemaSettings);
        }

        /// <summary>
        /// Configures swagger to use NodaTime types.
        /// </summary>
        /// <param name="config">Options to configure swagger.</param>
        /// <param name="nodaTimeSchemaSettings">Settings to configure serialization.</param>
        public static void ConfigureForNodaTime(this SwaggerGenOptions config, NodaTimeSchemaSettings nodaTimeSchemaSettings)
        {
            config.ParameterFilter<NamingPolicyParameterFilter>(nodaTimeSchemaSettings);

            Schemas schemas = new SchemasFactory(nodaTimeSchemaSettings).CreateSchemas();

            config.MapType<Instant>        (schemas.Instant);
            config.MapType<LocalDate>      (schemas.LocalDate);
            config.MapType<LocalTime>      (schemas.LocalTime);
            config.MapType<LocalDateTime>  (schemas.LocalDateTime);
            config.MapType<OffsetDateTime> (schemas.OffsetDateTime);
            config.MapType<ZonedDateTime>  (schemas.ZonedDateTime);
            config.MapType<Interval>       (schemas.Interval);
            config.MapType<DateInterval>   (schemas.DateInterval);
            config.MapType<Offset>         (schemas.Offset);
            config.MapType<Period>         (schemas.Period);
            config.MapType<Duration>       (schemas.Duration);
            config.MapType<OffsetDate>     (schemas.OffsetDate);
            config.MapType<OffsetTime>     (schemas.OffsetTime);
            config.MapType<DateTimeZone>   (schemas.DateTimeZone);

            // Nullable structs
            config.MapType<Instant?>       (schemas.Instant);
            config.MapType<LocalDate?>     (schemas.LocalDate);
            config.MapType<LocalTime?>     (schemas.LocalTime);
            config.MapType<LocalDateTime?> (schemas.LocalDateTime);
            config.MapType<OffsetDateTime?>(schemas.OffsetDateTime);
            config.MapType<ZonedDateTime?> (schemas.ZonedDateTime);
            config.MapType<Interval?>      (schemas.Interval);
            config.MapType<Offset?>        (schemas.Offset);
            config.MapType<Duration?>      (schemas.Duration);
            config.MapType<OffsetDate?>    (schemas.OffsetDate);
            config.MapType<OffsetTime?>    (schemas.OffsetTime);
        }
    }
}
