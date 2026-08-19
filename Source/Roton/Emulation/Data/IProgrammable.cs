using System;

namespace Roton.Emulation.Data;

public interface IProgrammable
{
    Memory<byte> Code { get; set; }
}