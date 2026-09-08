using System.Reflection;
using Lyon.Common.App;
using Microsoft.Extensions.DependencyInjection;
using Roton;
using Roton.Composers.Audio.AudioStreams;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;

namespace Lyon.Common;

public static class ServiceCollectionExtensions
{
    private static readonly ThreadLocal<Stack<Type>> DependencyStack = new(() => []);

    extension(IServiceCollection services)
    {
        public IServiceCollection AddLyonCommon(string[] args,
            IConfig config)
        {
            services.AddScoped(_ => config);
            services.AddScoped<ICommandLine>(_ => new CommandLine { Args = args });

            services.AddScoped<IFileSystem>(c =>
                c.GetRequiredService<IFileSystemFactory>().Create(c.GetRequiredService<IConfig>().HomePath ?? ".")
            );

            services.AddScoped<ISceneComposer>(c => c.GetRequiredService<ISceneComposerFactory>().Get());
            services.AddScoped<ISpeaker>(c => c.GetRequiredService<IAudioStreamComposer>());
            services.AddScoped<ITerminal>(c => c.GetRequiredService<ISceneComposer>());

            return services;
        }

        public IServiceCollection AddRoton(Context context,
            params Assembly[] additionalAssemblies)
        {
            var assemblies = new[] { typeof(ServiceCollectionExtensions).Assembly }
                .Concat(additionalAssemblies)
                .Distinct()
                .ToArray();
            
            var map = RotonServices.Get(context, assemblies)
                .GroupBy(s => s.Implementation);

            foreach (var serviceGroup in map)
            {
                // Add concrete implementation.
                if (serviceGroup.Key.IsGenericTypeDefinition)
                {
                    services.AddScoped(serviceGroup.Single().Service, serviceGroup.Key);
                }
                else
                {
                    services.AddScoped(serviceGroup.Key);

                    // Add service mappings.
                    foreach (var service in serviceGroup)
                        services.AddScoped(service.Service, sp =>
                        {
                            var stack = DependencyStack.Value!;
                            if (stack.Contains(service.Service))
                            {
                                throw new Exception($"Circular dependency detected: {service.Service.FullName} <- " +
                                                    string.Join(" <- ", stack.Select(rs => rs.ToString())));
                            }

                            stack.Push(service.Service);
                            var result = sp.GetRequiredService(serviceGroup.Key);
                            stack.Pop();
                            return result;
                        });
                }
            }
            
            return services;
        }
    }
}