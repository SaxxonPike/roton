namespace Roton.Infrastructure.Impl;

public static class ContextMetadataServiceFactory
{
    public static IContextMetadataService GetForContext(Context context) =>
        context switch
        {
            0 => throw new RotonException($"Unknown {nameof(Context)}."),
            _ => new ContextMetadataService(context)
        };
}