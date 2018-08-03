using System;
using System.Linq;
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
        /// </summary>
        /// <param name="config">Options to configure swagger.</param>
        public static void ConfigureForNodaTime(this SwaggerGenOptions config)
        {
            var serializerSettings = new JsonSerializerSettings();
            config.ConfigureForNodaTime(serializerSettings);
        }

        /// <summary>
        /// Configures swagger to use NodaTime types.
        /// </summary>
        /// <param name="config">Options to configure swagger.</param>
        /// <param name="initJsonSettings">Action to initialize jsonSettings.</param>
        public static void ConfigureForNodaTime(this SwaggerGenOptions config, Action<JsonSerializerSettings> initJsonSettings)
        {
            var serializerSettings = new JsonSerializerSettings();
            initJsonSettings(serializerSettings);
            config.ConfigureForNodaTime(serializerSettings);
        }

        /// <summary>
        /// Configures swagger to use NodaTime types.
        /// </summary>
        /// <param name="config">Options to configure swagger.</param>
        /// <param name="serializerSettings">Settings to configure serialization.</param>
        public static void ConfigureForNodaTime(this SwaggerGenOptions config, JsonSerializerSettings serializerSettings)
        {
            bool isNodaConvertersRegistered = serializerSettings.Converters.Any(converter => converter is NodaConverterBase<Instant>);
            if (!isNodaConvertersRegistered)
                serializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

            Schemas schemas = new SchemasFactory(serializerSettings).CreateSchemas();
            config.MapType<Instant>        (() => schemas.Instant);
            config.MapType<LocalDate>      (() => schemas.LocalDate);
            config.MapType<LocalTime>      (() => schemas.LocalTime);
            config.MapType<LocalDateTime>  (() => schemas.LocalDateTime);
            config.MapType<OffsetDateTime> (() => schemas.OffsetDateTime);
            config.MapType<ZonedDateTime>  (() => schemas.ZonedDateTime);
            config.MapType<Interval>       (() => schemas.Interval);
            config.MapType<Offset>         (() => schemas.Offset);
            config.MapType<Period>         (() => schemas.Period);
            config.MapType<Duration>       (() => schemas.Duration);
            config.MapType<DateTimeZone>   (() => schemas.DateTimeZone);

            config.MapType<Instant?>       (() => schemas.Instant);
            config.MapType<LocalDate?>     (() => schemas.LocalDate);
            config.MapType<LocalTime?>     (() => schemas.LocalTime);
            config.MapType<LocalDateTime?> (() => schemas.LocalDateTime);
            config.MapType<OffsetDateTime?>(() => schemas.OffsetDateTime);
            config.MapType<ZonedDateTime?> (() => schemas.ZonedDateTime);
            config.MapType<Interval?>      (() => schemas.Interval);
            config.MapType<Offset?>        (() => schemas.Offset);
            config.MapType<Duration?>      (() => schemas.Duration);
        }
    }
}
