using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Lyon.App;
using Microsoft.Extensions.DependencyInjection;
using Roton;
using Roton.Composers.Audio;
using Roton.Composers.Audio.AudioStreams;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;

namespace Lyon;

public static class ServiceCollectionExtensions
{
    private static readonly ThreadLocal<Stack<Type>> DependencyStack = new(() => []);
    
    extension(IServiceCollection services)
    {
        public void AddLyon(string[] args,
            IConfig config)
        {
            services.AddSingleton(config);
            services.AddSingleton<ICommandLine>(new CommandLine { Args = args });

            services.AddSingleton<IFileSystem>(c =>
                c.GetRequiredService<IFileSystemFactory>().Create(c.GetRequiredService<IConfig>().HomePath ?? ".")
            );

            services.AddSingleton<ISceneComposer>(c => c.GetRequiredService<ISceneComposerFactory>().Get());
            services.AddSingleton<ISpeaker>(c => c.GetRequiredService<IAudioStreamComposer>());
            services.AddSingleton<ITerminal>(c => c.GetRequiredService<ISceneComposer>());
        }

        public void AddRoton(Context context,
            params Assembly[] additionalAssemblies)
        {
            var map = RotonServices.Get(context, additionalAssemblies)
                .GroupBy(s => s.Implementation);

            foreach (var serviceGroup in map)
            {
                // Add concrete implementation.
                if (serviceGroup.Key.IsGenericTypeDefinition)
                {
                    services.AddSingleton(serviceGroup.Single().Service, serviceGroup.Key);
                }
                else
                {
                    services.AddSingleton(serviceGroup.Key);

                    // Add service mappings.
                    foreach (var service in serviceGroup)
                        services.AddSingleton(service.Service, sp =>
                        {
                            var stack = DependencyStack.Value!;
                            if (stack.Contains(service.Service))
                                throw new Exception($"Circular dependency detected: {service.Service.FullName} <- " +
                                                    string.Join(" <- ", stack.Select(rs => rs.ToString())));
                            stack.Push(service.Service);
                            var result = sp.GetRequiredService(serviceGroup.Key);
                            stack.Pop();
                            return result;
                        });
                }
            }
        }
    }
}