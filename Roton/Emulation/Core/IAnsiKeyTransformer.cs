using System.Collections.Generic;
using Roton.Emulation.Core.Impl;

namespace Roton.Emulation.Core
{
    public interface IAnsiKeyTransformer
    {
        IEnumerable<byte> GetBytes(IKeyPress keyPress);
    }
}