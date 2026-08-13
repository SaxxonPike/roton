using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperContextMetadataService : ContextMetadataService
{
    public SuperContextMetadataService() : base(Context.Super)
    {
    }
}