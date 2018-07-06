using System;
using Autofac;
using Lyon.App;
using Roton.Core;
using Roton.Interface.Infrastructure;
using Roton.Interface.Video.Glyphs;
using Roton.Interface.Video.Palettes;
using Roton.Interface.Video.Scenes.Composition;

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
                    typeof(IContext).Assembly,
                    typeof(IGlyphComposer).Assembly)
                .Where(t => !t.IsAbstract && t.IsClass)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            Register(builder);

            var container = builder.Build();
            
            container
                .Resolve<IBootstrap>()
                .Boot(args);
        }

        private static void Register(ContainerBuilder builder)
        {
            builder.Register(c => new InterfaceResourceProvider(c.Resolve<IResourceService>()
                .GetResource(typeof(IInterfaceResourceProvider).Assembly)))
                .As<IInterfaceResourceProvider>();

            builder.RegisterType<ComposerProxy>()
                .As<IComposerProxy>()
                .OnActivated(e =>
                {
                    var resource = e.Context.Resolve<IInterfaceResourceProvider>();
                    e.Instance.SetFont(resource.GetFontData());
                    e.Instance.SetPalette(resource.GetPaletteData());
                });
        }
    }
}