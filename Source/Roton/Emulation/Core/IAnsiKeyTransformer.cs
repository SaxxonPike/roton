using System;

namespace Roton.Emulation.Core;

public interface IAnsiKeyTransformer
{
    ReadOnlySpan<byte> GetBytes(KeyPress keyPress);
}