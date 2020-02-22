using System.Text.Encodings.Web;
using System.Text.Json;
using MicroElements.Swashbuckle.NodaTime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using NodaTime.Serialization.SystemTextJson;

namespace WebApiSample
{
    public class Startup
    {
        enum JsonProvider
        {
            NewtonsoftJson,
            SystemTextJson
        }

        // JsonProvider
        private static JsonProvider useJsonProvider = JsonProvider.SystemTextJson;

        // USING NewtonsoftJson settings as PropertyNamingPolicy for System.Text.Json
        private static bool useNewtonsoftJsonAsNamingPolicy = true;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            void ConfigureNewtonsoftJsonSerializerSettings(JsonSerializerSettings serializerSettings)
            {
                // Use DefaultContractResolver or CamelCasePropertyNamesContractResolver;
                // serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                serializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    //NamingStrategy = new DefaultNamingStrategy()
                    //NamingStrategy = new CamelCaseNamingStrategy()
                    //NamingStrategy = new SnakeCaseNamingStrategy()
                    NamingStrategy = new CamelCaseNamingStrategy()
                };

                // Configures JsonSerializer to properly serialize NodaTime types.
                serializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            }

            void ConfigureSystemTextJsonSerializerSettings(JsonSerializerOptions serializerOptions)
            {
                if (useNewtonsoftJsonAsNamingPolicy)
                {
                    // USING NewtonsoftJson settings as PropertyNamingPolicy for System.Text.Json
                    JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
                    ConfigureNewtonsoftJsonSerializerSettings(jsonSerializerSettings);
                    serializerOptions.PropertyNamingPolicy = new NewtonsoftJsonNamingPolicy(jsonSerializerSettings);
                }

                // Configures JsonSerializer to properly serialize NodaTime types.
                serializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

                serializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }

            if (useJsonProvider == JsonProvider.NewtonsoftJson)
            {
                services
                    .AddMvcCore()
                    .AddApiExplorer()
                    .AddNewtonsoftJson(options => ConfigureNewtonsoftJsonSerializerSettings(options.SerializerSettings));
            }

            if (useJsonProvider == JsonProvider.SystemTextJson)
            {
                services
                    .AddMvcCore()
                    .AddApiExplorer()
                    .AddJsonOptions(options => ConfigureSystemTextJsonSerializerSettings(options.JsonSerializerOptions))
                    ;
            }

            //services.AddTransient<IConfigureOptions<JsonOptions>, ConfigureNewtonsoftJsonJsonNamingPolicy>();

            // Adds swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                if (useJsonProvider == JsonProvider.NewtonsoftJson)
                {
                    // Configures swagger to use NodaTime with serializerSettings.
                    c.ConfigureForNodaTime(configureSerializerSettings: ConfigureNewtonsoftJsonSerializerSettings, shouldGenerateExamples: true);
                }

                if (useJsonProvider == JsonProvider.SystemTextJson)
                {
                    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                    ConfigureSystemTextJsonSerializerSettings(jsonSerializerOptions);
                    c.ConfigureForNodaTimeWithSystemTextJson(jsonSerializerOptions, shouldGenerateExamples: true);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            // Adds swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }

    public class NewtonsoftJsonNamingPolicy : JsonNamingPolicy
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        /// <inheritdoc />
        public NewtonsoftJsonNamingPolicy(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
        }

        /// <inheritdoc />
        public override string ConvertName(string name)
        {
            var contractResolver = _jsonSerializerSettings.ContractResolver;
            return (contractResolver as DefaultContractResolver)?.GetResolvedPropertyName(name) ?? name;
        }
    }

    public class ConfigureNewtonsoftJsonJsonNamingPolicy : IConfigureOptions<JsonOptions>
    {
        private readonly IOptions<MvcNewtonsoftJsonOptions> _newtonsoftJsonOptions;

        public ConfigureNewtonsoftJsonJsonNamingPolicy(IOptions<MvcNewtonsoftJsonOptions> newtonsoftJsonOptions)
        {
            _newtonsoftJsonOptions = newtonsoftJsonOptions;
        }

        /// <inheritdoc />
        public void Configure(JsonOptions options)
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = new NewtonsoftJsonNamingPolicy(_newtonsoftJsonOptions.Value.SerializerSettings);
        }
    }
}
