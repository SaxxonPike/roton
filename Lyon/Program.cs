using System;
using Autofac;
using Lyon.App;
using Roton.Core;
using Roton.Interface.Resources;
using Roton.Interface.Video.Glyphs;

namespace Lyon
{
    internal static class Program
    {
        // STAThread is required for open/save dialogs.
        
        [STAThread]
        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(
                    typeof(IGame).Assembly,
                    typeof(IContext).Assembly,
                    typeof(IGlyphComposer).Assembly)
                .Where(t => !t.IsAbstract && t.IsClass)
                .AsImplementedInterfaces();

            var container = builder.Build();
            
            container
                .Resolve<IBootstrap>()
                .Boot(args);
        }

        private static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<ComposerProxy>()
                .As<IComposerProxy>()
                .OnActivated(e =>
                {
                    var resource = e.Context.Resolve<ICommonResourceArchive>();
                    e.Instance.SetFont(resource.GetFont());
                    e.Instance.SetPalette(resource.GetPalette());
                });
        }
    }
}