using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperEngineResourceService(IAssemblyResourceService assemblyResourceService)
    : EngineResourceService(assemblyResourceService, "elements-szzt.bin", "memory-szzt.bin");