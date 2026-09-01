using System;

namespace Roton.Emulation.Data;

public interface IMemory
{
    Span<byte> Data { get; }
}