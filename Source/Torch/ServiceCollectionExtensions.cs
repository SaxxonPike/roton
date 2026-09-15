using Lyon.App;
using Lyon.Common.App;
using Microsoft.Extensions.DependencyInjection;
using Torch.App.Impl;

namespace Torch;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTorch(this IServiceCollection services)
    {
        services.AddSingleton<IWindow, Window>();
        services.AddSingleton<ILauncher, Launcher>();
        return services;
    }
}