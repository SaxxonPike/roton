using System.Linq;
using Autofac;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Lyon.Autofac
{
    public class RotonModule : Module
    {
        private readonly ContextEngine _contextEngine;

        public RotonModule(ContextEngine contextEngine)
        {
            _contextEngine = contextEngine;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var cmsf = new ContextMetadataServiceFactory();

            builder.RegisterInstance(cmsf)
                .As<IContextMetadataServiceFactory>()
                .SingleInstance();

            builder.Register(c => cmsf.Get(_contextEngine))
                .As<IContextMetadataService>()
                .SingleInstance();
            
            builder.RegisterTypes(cmsf.Get(ContextEngine.Startup).GetTypes().ToArray())
                .AsImplementedInterfaces()
                .SingleInstance();
            
            builder.RegisterTypes(cmsf.Get(_contextEngine).GetTypes().ToArray())
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}