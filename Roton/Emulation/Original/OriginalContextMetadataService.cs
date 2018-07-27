using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalContextMetadataService : ContextMetadataService
    {
        public OriginalContextMetadataService() : base(Context.Original)
        {
        }
    }
}