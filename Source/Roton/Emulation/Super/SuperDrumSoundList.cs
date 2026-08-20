using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperDrumSoundList(IMemory memory) : MemoryDrumSoundList(memory, 0xD0B2);