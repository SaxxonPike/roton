using System;
using System.Linq;
using Autofac;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Original;
using Roton.Emulation.Super;
using Roton.Infrastructure;

namespace Lyon.App.Impl
{
    public class ContextFactory : IContextFactory
    {
        private readonly ILifetimeScope _rootLifetimeScope;

        public ContextFactory(ILifetimeScope rootLifetimeScope)
        {
            _rootLifetimeScope = rootLifetimeScope;
        }

        private IContextMetadataService GetContextMetadataService(ContextEngine contextEngine)
        {
            switch (contextEngine)
            {
                case ContextEngine.Original:
                    return new OriginalContextMetadataService();
                case ContextEngine.Super:
                    return new SuperContextMetadataService();
                default:
                    throw new Exception($"Unknown {nameof(ContextEngine)}: {contextEngine}");
            }
        }

        public IContext Create(ContextEngine contextEngine, IConfig config)
        {
            var scope = _rootLifetimeScope.BeginLifetimeScope(builder =>
            {
                var contextMetadataService = GetContextMetadataService(contextEngine);
                
                builder.RegisterInstance(contextMetadataService)
                    .As<IContextMetadataService>()
                    .SingleInstance();
                
                builder.RegisterTypes(contextMetadataService.GetTypes().ToArray())
                    .AsImplementedInterfaces()
                    .SingleInstance();

                builder.Register(c => c.Resolve<IFileSystemFactory>().Create(config.HomePath))
                    .AsImplementedInterfaces()
                    .SingleInstance();

                builder.RegisterInstance(config).As<IConfig>().SingleInstance();
            });

            return scope.Resolve<IContext>();
        }
    }
}