using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperDrumBank(IMemory memory) : MemoryDrumBank(memory, 0xD0B2);