using System;

namespace Roton.Emulation.Data;

public interface IProgrammable
{
    Span<char> Code { get; set; }
}