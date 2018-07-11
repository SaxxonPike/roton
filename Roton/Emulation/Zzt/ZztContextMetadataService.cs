using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Zzt
{
    [ContextEngine(ContextEngine.Zzt)]
    public sealed class ZztContextMetadataService : ContextMetadataService
    {
        public ZztContextMetadataService() : base(ContextEngine.Zzt)
        {
        }
    }
}