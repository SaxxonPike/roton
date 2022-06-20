using System;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Original;
using Roton.Emulation.Super;

namespace Roton.Infrastructure.Impl;

public sealed class ContextMetadataServiceFactory : IContextMetadataServiceFactory
{
    public IContextMetadataService Get(Context context)
    {
        return context switch
        {
            Context.Startup => new StartupContextMetadataService(),
            Context.Original => new OriginalContextMetadataService(),
            Context.Super => new SuperContextMetadataService(),
            _ => throw new Exception($"Unknown {nameof(Context)}: {context}")
        };
    }
}