namespace StandUsers.WebApi.Extensions;

using Microsoft.OpenApi.Models;
using System.Reflection;

public static class SwaggerConfiguration
{
    public static WebApplication AddSwaggerUi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.InjectStylesheet("/swagger-ui/SwaggerDark.css");
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.DefaultModelsExpandDepth(-1);
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnableFilter();
        });
        return app;
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Version = "v1",
                Title = "StandUsers Api",
                Description = "StandUsers user management application",
                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0"),
                }
            });
            var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
        });
    }
}
