using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalEngineResourceService : EngineResourceService
    {
        public OriginalEngineResourceService(IAssemblyResourceService assemblyResourceService)
            : base(assemblyResourceService, "elements-zzt.bin", "memory-zzt.bin")
        {
        }
    }
}