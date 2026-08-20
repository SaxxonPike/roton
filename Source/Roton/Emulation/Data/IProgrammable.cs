using System;

namespace Roton.Emulation.Data;

public interface IProgrammable
{
    Memory<char> Code { get; set; }
}