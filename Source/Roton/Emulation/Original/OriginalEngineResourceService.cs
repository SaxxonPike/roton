using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalEngineResourceService(IAssemblyResourceService assemblyResourceService)
    : EngineResourceService(assemblyResourceService, "elements-zzt.bin", "memory-zzt.bin");