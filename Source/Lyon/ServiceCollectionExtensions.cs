using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Lyon.App;
using Microsoft.Extensions.DependencyInjection;
using Roton;
using Roton.Composers.Audio;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;

namespace Lyon;

public static class ServiceCollectionExtensions
{
    public static void AddLyon(
        this IServiceCollection services,
        string[] args,
        IConfig config)
    {
        services.AddSingleton(config);
        services.AddSingleton<ICommandLine>(new CommandLine { Args = args });

        services.AddSingleton<IFileSystem>(c =>
            c.GetRequiredService<IFileSystemFactory>().Create(c.GetRequiredService<IConfig>().HomePath ?? ".")
        );

        services.AddSingleton<ISceneComposer>(c => c.GetRequiredService<ISceneComposerFactory>().Get());
        services.AddSingleton<ISpeaker>(c => c.GetRequiredService<IAudioComposer>());
        services.AddSingleton<ITerminal>(c => c.GetRequiredService<ISceneComposer>());
    }

    public static void AddRoton(
        this IServiceCollection services,
        Context context,
        params Assembly[] additionalAssemblies)
    {
        // Each concrete type must have all its services registered at the same time
        // so that AutoFac knows that they all refer to the same instance.

        var map = RotonServices.Get(context, additionalAssemblies)
            .GroupBy(s => s.Implementation);

        foreach (var serviceGroup in map)
        {
            // Add concrete implementation.
            services.AddSingleton(serviceGroup.Key);

            // Add service mappings.
            foreach (var service in serviceGroup)
                services.AddSingleton(service.Service, sp =>
                {
                    Debug.WriteLine($"Resolving service ${service.Service.FullName}");
                    return sp.GetRequiredService(serviceGroup.Key);
                });
        }
    }
}