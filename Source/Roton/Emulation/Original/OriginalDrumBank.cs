using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalDrumBank(IMemory memory) : MemoryDrumBank(memory, 0x7FA4);