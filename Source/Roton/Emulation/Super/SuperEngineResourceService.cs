using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperEngineResourceService(IAssemblyResourceService assemblyResourceService)
    : EngineResourceService(assemblyResourceService, "elements-szzt.bin", "memory-szzt.bin");