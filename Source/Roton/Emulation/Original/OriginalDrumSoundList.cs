using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalDrumSoundList(IMemory memory)
    : DrumSoundList(memory, 0x7FA4, 0x200);