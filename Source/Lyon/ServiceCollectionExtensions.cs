using Lyon.App;
using Lyon.App.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Lyon;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLyon(this IServiceCollection services)
    {
        services.AddSingleton<IWindow, Window>();
        services.AddSingleton<ILauncher, Launcher>();
        return services;
    }
}