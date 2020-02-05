# MicroElements.Swashbuckle.NodaTime
Allows configure Asp.Net Core and swagger to use NodaTime types.

## Latest Builds, Packages
[![License](http://img.shields.io/:license-mit-blue.svg)](https://raw.githubusercontent.com/micro-elements/MicroElements.Swashbuckle.NodaTime/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/MicroElements.Swashbuckle.NodaTime.svg)](https://www.nuget.org/packages/MicroElements.Swashbuckle.NodaTime)
![NuGet](https://img.shields.io/nuget/dt/MicroElements.Swashbuckle.NodaTime.svg)
[![MyGet](https://img.shields.io/myget/micro-elements/v/MicroElements.Swashbuckle.NodaTime.svg)](https://www.myget.org/feed/micro-elements/package/nuget/MicroElements.Swashbuckle.NodaTime)

[![Travis](https://img.shields.io/travis/micro-elements/MicroElements.Swashbuckle.NodaTime/master.svg?logo=travis)](https://travis-ci.org/micro-elements/MicroElements.Swashbuckle.NodaTime)
[![AppVeyor](https://img.shields.io/appveyor/ci/micro-elements/microelements-swashbuckle-nodatime.svg?logo=appveyor)](https://ci.appveyor.com/project/micro-elements/microelements-swashbuckle-nodatime)
[![Coverage Status](https://img.shields.io/coveralls/micro-elements/MicroElements.Swashbuckle.NodaTime.svg)](https://coveralls.io/r/micro-elements/MicroElements.Swashbuckle.NodaTime)

[![Gitter](https://img.shields.io/gitter/room/micro-elements/MicroElements.Swashbuckle.NodaTime.svg)](https://gitter.im/micro-elements/MicroElements.Swashbuckle.NodaTime)

## Installation

### Package Reference:

```
dotnet add package microelements.swashbuckle.nodatime
```

## Getting started
- Add package reference to MicroElements.Swashbuckle.NodaTime
- Configure asp net core to use swagger
- Configure JsonSerializer to properly serialize NodaTime types. see `AddJsonFormatters` or `AddJsonOptions`
- Configure `AddSwaggerGen` with `ConfigureForNodaTime`

## Benefits of MicroElements.Swashbuckle.NodaTime
- Supports Swashbuckle 5, net core 3 and brand new System.Text.Json
- Implemented in c#, no FSharp.Core lib in dependencies
- JsonSerializerSettings ContractResolver uses for NamingStrategy, so you can use DefaultNamingStrategy, CamelCaseNamingStrategy or SnakeCaseNamingStrategy
- Supports new DateInterval (use NodaTime.Serialization.JsonNet >= 2.1.0)

## Sample

Full sample see in samples: https://github.com/micro-elements/MicroElements.Swashbuckle.NodaTime/tree/master/samples/WebApiSample

```csharp
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
                c.ConfigureForNodaTime(configureSerializerSettings: ConfigureNewtonsoftJsonSerializerSettings);
            }

            if (useJsonProvider == JsonProvider.SystemTextJson)
            {
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                ConfigureSystemTextJsonSerializerSettings(jsonSerializerOptions);
                c.ConfigureForNodaTimeWithSystemTextJson(jsonSerializerOptions);
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

```

## How it works
1. MicroElements.Swashbuckle.NodaTime creates Schemas for all NodaTime types
2. MicroElements.Swashbuckle.NodaTime configures JsonSerializer for examples
3. Maps types to [ISO 8601]

## Screenshots

## Without MicroElements.Swashbuckle.NodaTime
![](https://raw.githubusercontent.com/micro-elements/MicroElements.Swashbuckle.NodaTime/master/images/NodaTime0.png)

## With MicroElements.Swashbuckle.NodaTime
![](https://raw.githubusercontent.com/micro-elements/MicroElements.Swashbuckle.NodaTime/master/images/NodaTime1.png)

## With MicroElements.Swashbuckle.NodaTime (camelCase)
![](https://raw.githubusercontent.com/micro-elements/MicroElements.Swashbuckle.NodaTime/master/images/NodaTime2.png)

## Build
Windows: Run `build.ps1`

Linux: Run `build.sh`

## License
This project is licensed under the MIT license. See the [LICENSE] file for more info.

[LICENSE]: https://raw.githubusercontent.com/micro-elements/MicroElements.Swashbuckle.NodaTime/master/LICENSE
[ISO 8601]: https://xml2rfc.tools.ietf.org/public/rfc/html/rfc3339.html#anchor14
