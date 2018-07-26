using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface IScroll
    {
        IScrollResult Show(string title, IEnumerable<string> message);
        int TextWidth { get; }
        int TextHeight { get; }
    }
}