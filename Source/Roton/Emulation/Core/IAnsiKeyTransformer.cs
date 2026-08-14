using System.Collections.Generic;

namespace Roton.Emulation.Core;

public interface IAnsiKeyTransformer
{
    IEnumerable<byte> GetBytes(IKeyPress keyPress);
}