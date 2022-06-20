using Roton.Emulation.Data.Impl;

namespace Roton.Infrastructure.Impl;

public sealed class StartupContextMetadataService : ContextMetadataService
{
    public StartupContextMetadataService() : base(Context.Startup)
    {
    }
}