using System;
using Autofac;
using Lyon.App;
using Lyon.Presenters;
using Roton.Composers.Video.Glyphs;
using Roton.Composers.Video.Palettes;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Lyon
{
    public static class Program
    {
        // STAThread is required for open/save dialogs.

        [STAThread]
        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(ILauncher).Assembly)
                .Where(t => !t.IsAbstract && t.IsClass)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterInstance(new CommandLine {Args = args})
                .As<ICommandLine>()
                .SingleInstance();

            Register(builder);

            using (var container = builder.Build())
            {
                container
                    .Resolve<IBootstrap>()
                    .Boot(args);                
            }
        }

        private static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<AssemblyResourceService>()
                .As<IAssemblyResourceService>()
                .SingleInstance();

            builder.RegisterType<AudioPresenter>()
                .As<IAudioPresenter>()
                .SingleInstance();
        }
    }
}