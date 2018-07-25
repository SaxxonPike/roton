using System;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Original;
using Roton.Emulation.Super;

namespace Roton.Infrastructure.Impl
{
    public sealed class ContextMetadataServiceFactory : IContextMetadataServiceFactory
    {
        public IContextMetadataService Get(ContextEngine contextEngine)
        {
            switch (contextEngine)
            {
                case ContextEngine.Startup:
                    return new StartupContextMetadataService();
                case ContextEngine.Original:
                    return new OriginalContextMetadataService();
                case ContextEngine.Super:
                    return new SuperContextMetadataService();
                default:
                    throw new Exception($"Unknown {nameof(ContextEngine)}: {contextEngine}");
            }
        }
    }
}