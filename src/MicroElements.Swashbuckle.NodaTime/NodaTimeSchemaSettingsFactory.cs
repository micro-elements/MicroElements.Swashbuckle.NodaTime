// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;

namespace MicroElements.Swashbuckle.NodaTime
{
    /// <summary>
    /// Factory methods for <see cref="NodaTimeSchemaSettings"/>.
    /// </summary>
    public static class NodaTimeSchemaSettingsFactory
    {
        /// <summary>
        /// Creates <see cref="NodaTimeSchemaSettings"/> for NewtonsoftJson.
        /// </summary>
        /// <param name="serializerSettings"><see cref="JsonSerializerSettings"/>.</param>
        /// <param name="shouldGenerateExamples">Should generate example for schema.</param>
        /// <param name="schemaExamples"><see cref="SchemaExamples"/> for schema example values.</param>
        /// <param name="dateTimeZoneProvider">Optional <see cref="IDateTimeZoneProvider"/>.</param>
        /// <returns><see cref="NodaTimeSchemaSettings"/>.</returns>
        public static NodaTimeSchemaSettings CreateNodaTimeSchemaSettingsForNewtonsoftJson(
            this JsonSerializerSettings serializerSettings,
            bool shouldGenerateExamples = true,
            SchemaExamples? schemaExamples = null,
            IDateTimeZoneProvider? dateTimeZoneProvider = null)
        {
            string FormatToJson(object value)
            {
                string formatToJson = JsonConvert.SerializeObject(value, serializerSettings);
                if (formatToJson.StartsWith("\"") && formatToJson.EndsWith("\""))
                    formatToJson = formatToJson.Substring(1, formatToJson.Length - 2);
                return formatToJson;
            }

            string ResolvePropertyName(string propertyName)
            {
                return (serializerSettings.ContractResolver as DefaultContractResolver)?.GetResolvedPropertyName(propertyName) ?? propertyName;
            }

            return new NodaTimeSchemaSettings(
                ResolvePropertyName,
                FormatToJson,
                shouldGenerateExamples,
                schemaExamples,
                dateTimeZoneProvider);
        }

        /// <summary>
        /// Creates <see cref="NodaTimeSchemaSettings"/> for SystemTextJson.
        /// </summary>
        /// <param name="jsonSerializerOptions"><see cref="JsonSerializerOptions"/>.</param>
        /// <param name="shouldGenerateExamples">Should generate example for schema.</param>
        /// <param name="schemaExamples"><see cref="SchemaExamples"/> for schema example values.</param>
        /// <param name="dateTimeZoneProvider">Optional <see cref="IDateTimeZoneProvider"/>.</param>
        /// <returns><see cref="NodaTimeSchemaSettings"/>.</returns>
        public static NodaTimeSchemaSettings CreateNodaTimeSchemaSettingsForSystemTextJson(
            this JsonSerializerOptions jsonSerializerOptions,
            bool shouldGenerateExamples = true,
            SchemaExamples? schemaExamples = null,
            IDateTimeZoneProvider? dateTimeZoneProvider = null)
        {
            string FormatToJson(object value)
            {
                if (value is DateTimeZone dateTimeZone)
                {
                    // TODO: remove after PR released: https://github.com/nodatime/nodatime.serialization/pull/57
                    return dateTimeZone.Id;
                }

                string formatToJson = System.Text.Json.JsonSerializer.Serialize(value, jsonSerializerOptions);
                if (formatToJson.StartsWith("\"") && formatToJson.EndsWith("\""))
                    formatToJson = formatToJson.Substring(1, formatToJson.Length - 2);
                return formatToJson;
            }

            string ResolvePropertyName(string propertyName)
            {
                return jsonSerializerOptions.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName;
            }

            return new NodaTimeSchemaSettings(
                ResolvePropertyName,
                FormatToJson,
                shouldGenerateExamples,
                schemaExamples,
                dateTimeZoneProvider);
        }
    }
}
