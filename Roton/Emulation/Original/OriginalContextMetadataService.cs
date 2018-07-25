using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalContextMetadataService : ContextMetadataService
    {
        public OriginalContextMetadataService() : base(ContextEngine.Original)
        {
        }
    }
}