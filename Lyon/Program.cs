using System;
using Autofac;
using Lyon.App;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;
using Roton.Interface.Infrastructure;
using Roton.Interface.Input;
using Roton.Interface.Video.Glyphs;

namespace Lyon
{
    public static class Program
    {
        // STAThread is required for open/save dialogs.

        [STAThread]
        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(
                    typeof(ILauncher).Assembly,
                    typeof(IGlyphComposer).Assembly)
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

            builder.RegisterType<InterfaceResourceService>()
                .As<IInterfaceResourceService>()
                .SingleInstance();

            builder.RegisterType<ComposerProxy>()
                .As<IComposerProxy>()
                .OnActivated(e =>
                {
                    var resource = e.Context.Resolve<IInterfaceResourceService>();
                    e.Instance.SetFont(resource.GetFontData());
                    e.Instance.SetPalette(resource.GetPaletteData());
                    e.Instance.SetScene(80, 25, false);
                })
                .SingleInstance();

            builder.RegisterType<SpeakerProxy>().As<ISpeaker>().SingleInstance();
            builder.RegisterType<TerminalProxy>().As<ITerminal>().SingleInstance();
            builder.RegisterType<OpenTkKeyBuffer>().AsImplementedInterfaces().SingleInstance();
        }
    }
}