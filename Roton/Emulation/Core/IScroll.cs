using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface IScroll
    {
        IScrollResult Show(IEnumerable<string> message);
    }
}