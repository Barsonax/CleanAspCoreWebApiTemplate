using CleanAspCore.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CleanAspCore.Api.Tests.Helpers;

public static class WebApplicationFactoryExtensions
{
    public static IServiceScope CreateScope(this WebApplicationFactory<Program> webApplicationFactory) => webApplicationFactory.Services.CreateScope();
    
    public static void SeedData(this WebApplicationFactory<Program> webApplicationFactory, Action<HrContext> seedAction)
    {
        using var scope = webApplicationFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HrContext>();
        seedAction(context);
        context.SaveChanges();
    }
    
    public static void AssertDatabase(this WebApplicationFactory<Program> webApplicationFactory, Action<HrContext> seedAction)
    {
        using var scope = webApplicationFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HrContext>();
        seedAction(context);
    }
}