using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [ContextEngine(ContextEngine.Super)]
    public sealed class SuperContextMetadataService : ContextMetadataService
    {
        public SuperContextMetadataService() : base(ContextEngine.Super)
        {
        }
    }
}