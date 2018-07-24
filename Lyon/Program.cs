using System;
using Autofac;
using Lyon.App;
using Lyon.Presenters;
using Lyon.Presenters.Impl;
using Roton.Composers.Audio;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;

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

            builder.RegisterType<AudioPresenter>()
                .As<IAudioPresenter>();

            using (var container = builder.Build())
            {
                container
                    .Resolve<IBootstrap>()
                    .Boot(args);
            }
        }
    }
}