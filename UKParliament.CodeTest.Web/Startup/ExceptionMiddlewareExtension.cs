using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace UKParliament.CodeTest.Web.Startup;

public static class ExceptionMiddlewareExtension
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                var env = context.RequestServices.GetRequiredService<IWebHostEnvironment>();

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    logger.LogError(contextFeature.Error,
                        "Unhandled exception occurred while processing request: {Path}",
                        contextFeature.Path);

                    var response = new
                    {
                        context.Response.StatusCode,
                        Message = "Internal Server Error",
                        Detail = env.IsDevelopment() ? contextFeature.Error.Message : null,
                        Type = contextFeature.Error.GetType().Name
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });
        });
    }
}
