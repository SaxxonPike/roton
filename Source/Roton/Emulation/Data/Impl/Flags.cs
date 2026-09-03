namespace Roton.Emulation.Data.Impl;

public abstract class Flags(IMemory memory, int offset) : FixedStringSet(memory, offset, true), IFlags;