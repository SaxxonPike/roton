using System;
using System.Collections.Generic;

namespace Roton.Emulation.Core;

public interface IAnsiKeyTransformer
{
    ReadOnlySpan<byte> GetBytes(IKeyPress keyPress);
}