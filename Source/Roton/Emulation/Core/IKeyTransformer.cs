using System;

namespace Roton.Emulation.Core;

public interface IKeyTransformer
{
    ReadOnlySpan<byte> GetBytes(KeyPress keyPress);
}