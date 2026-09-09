using System.Reflection;
using Lyon.Common.App;
using Lyon.Common.App.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Roton;
using Roton.Composers.Audio.AudioStreams;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Lyon.Common;

public static class ServiceCollectionExtensions
{
    private static readonly ThreadLocal<Stack<Type>> DependencyStack = new(() => []);

    extension(IServiceCollection services)
    {
        public IServiceCollection AddLyonUi() =>
            services.AddRoton(Context.Ui);

        public IServiceCollection AddLyonConfig(string[] args, out IConfiguration config)
        {
            var conf = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile(json =>
                {
                    json.Optional = false;
                    json.ReloadOnChange = true;
                    json.Path = "RotonConfig.Default.json";
                })
                .AddJsonFile(json =>
                {
                    json.Optional = true;
                    json.ReloadOnChange = true;
                    json.Path = "RotonConfig.json";
                })
                .AddCommandLine(args)
                .Build();

            services.Configure<AudioConfig>(c => { conf.GetSection("Roton:Audio").Bind(c); });
            services.Configure<EngineConfig>(c => { conf.GetSection("Roton:Engine").Bind(c); });
            services.Configure<JoystickConfig>(c => { conf.GetSection("Roton:Joystick").Bind(c); });

            services.AddScoped(c => c.GetRequiredService<IOptions<AudioConfig>>().Value);
            services.AddScoped(c => c.GetRequiredService<IOptions<EngineConfig>>().Value);
            services.AddScoped(c => c.GetRequiredService<IOptions<JoystickConfig>>().Value);

            config = conf;
            return services;
        }

        public IServiceCollection AddLyonCommon(string[] args)
        {
            services.AddScoped<IFileSystem>(c =>
            {
                var config = c.GetRequiredService<IOptions<EngineConfig>>().Value;
                var assemblyResourceService = c.GetRequiredService<IAssemblyResourceService>();

                var fileSystem = FileSystems.Aggregate([
                    FileSystems.Disk(config.HomePath ?? "."),
                    assemblyResourceService.GetFromAssemblyOf<IGame>().Root
                ]);

                return fileSystem;
            });

            services.AddScoped<ISceneComposer>(c =>
                c.GetRequiredService<ISceneComposerFactory>().Get());

            services.AddScoped<ISpeaker>(c =>
                c.GetRequiredService<IAudioStreamComposer>());

            services.AddScoped<ITerminal>(c =>
                c.GetRequiredService<ISceneComposer>());

            services.AddOptions();

            return services;
        }

        public IServiceCollection AddRotonEditor()
        {
            services.AddSingleton<IWorldEditorFactory, WorldEditorFactory>();
            return services.AddRoton(Context.Editor);
        }

        public IServiceCollection AddRoton(
            Context context,
            params Assembly[] additionalAssemblies)
        {
            services.AddScoped<IContextMetadataService>(_ => ContextMetadataServiceFactory.GetForContext(context));

            var assemblies = new[] { typeof(ServiceCollectionExtensions).Assembly }
                .Concat(additionalAssemblies)
                .Distinct()
                .ToArray();

            var map = RotonServices
                .Get(context, assemblies)
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