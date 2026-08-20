using Roton.Emulation.Original;
using Roton.Emulation.Super;

namespace Roton.Infrastructure.Impl;

public static class ContextMetadataServiceFactory
{
    public static IContextMetadataService GetForContext(Context context) =>
        context switch
        {
            Context.Unknown => throw new RotonException($"Unknown {nameof(Context)}."),
            Context.Startup => new StartupContextMetadataService(),
            Context.Original => new OriginalContextMetadataService(),
            Context.Super => new SuperContextMetadataService(),
            _ => throw new RotonException($"Unknown {nameof(Context)}: {context}.")
        };
}