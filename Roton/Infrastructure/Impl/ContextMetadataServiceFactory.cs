using System;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Original;
using Roton.Emulation.Super;

namespace Roton.Infrastructure.Impl
{
    public sealed class ContextMetadataServiceFactory : IContextMetadataServiceFactory
    {
        public IContextMetadataService Get(Context context)
        {
            switch (context)
            {
                case Context.Startup:
                    return new StartupContextMetadataService();
                case Context.Original:
                    return new OriginalContextMetadataService();
                case Context.Super:
                    return new SuperContextMetadataService();
                default:
                    throw new Exception($"Unknown {nameof(Context)}: {context}");
            }
        }
    }
}