using MicroElements.Swashbuckle.NodaTime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Swashbuckle.AspNetCore.Swagger;

namespace WebApiSample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            void InitJsonSettings(JsonSerializerSettings serializerSettings)
            {
                // Use DefaultContractResolver or CamelCasePropertyNamesContractResolver;
                serializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    //NamingStrategy = new SnakeCaseNamingStrategy()
                    //NamingStrategy = new CamelCaseNamingStrategy()
                };

                // Configures JsonSerializer to properly serialize NodaTime types.
                serializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            }

            // CASE1: AddMvcCore with AddJsonFormatters
            services
                .AddMvcCore()
                .AddApiExplorer()
                .AddJsonFormatters(InitJsonSettings)
                ;

            // CASE2: AddMvc with AddJsonOptions
            //services
            //    .AddMvc()
            //    .AddJsonOptions(options => InitJsonSettings(options.SerializerSettings))
            //    ;

            // Adds swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                // Configures swagger to use NodaTime.
                // c.ConfigureForNodaTime();

                // Configures swagger to use NodaTime. Use the same InitJsonSettings action that in AddJsonFormatters
                c.ConfigureForNodaTime(InitJsonSettings);

                // Configures swagger to use NodaTime with serializerSettings.
                // c.ConfigureForNodaTime(serializerSettings);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc().UseSwagger();

            // Adds swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
