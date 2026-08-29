using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalEngineResourceService(IAssemblyResourceService assemblyResourceService)
    : EngineResourceService(assemblyResourceService, "elements-zzt.bin", "memory-zzt.bin");