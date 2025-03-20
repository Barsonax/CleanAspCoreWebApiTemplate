namespace CleanAspCore.Api.Common.ErrorHandling;

public static class ErrorHandlingModule
{
    public static void UseErrorHandling(this WebApplication app)
    {
        app.UseExceptionHandler(_ => { });
    }

    public static void AddExceptionHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.Configure<RouteHandlerOptions>(options => options.ThrowOnBadRequest = true);
    }
}
