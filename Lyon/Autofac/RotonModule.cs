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

            builder.Register(c => cmsf.Get(_contextEngine))
                .As<IContextMetadataService>()
                .AutoActivate()
                .SingleInstance();
            
            builder.RegisterTypes(cmsf.Get(ContextEngine.Startup).GetTypes().ToArray())
                .AsImplementedInterfaces()
                .AutoActivate()
                .SingleInstance();
            
            builder.RegisterTypes(cmsf.Get(_contextEngine).GetTypes().ToArray())
                .AsImplementedInterfaces()
                .AutoActivate()
                .SingleInstance();
        }
    }
}