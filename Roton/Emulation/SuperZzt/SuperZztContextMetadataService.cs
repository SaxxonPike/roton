using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.SuperZzt
{
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class SuperZztContextMetadataService : ContextMetadataService
    {
        public SuperZztContextMetadataService() : base(ContextEngine.SuperZzt)
        {
        }
    }
}