using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperEngineResourceService : EngineResourceService
{
    public SuperEngineResourceService(IAssemblyResourceService assemblyResourceService)
        : base(assemblyResourceService, "elements-szzt.bin", "memory-szzt.bin")
    {
    }
}