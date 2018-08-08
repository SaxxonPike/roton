using System;
using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface IScroll
    {
        IScrollState Show(string title, string fileName);
        IScrollState Show(string title, IEnumerable<string> message, bool isHelp);
        IScrollState Show(string title, IEnumerable<string> message, bool isHelp, Action<IScrollState> mainLoop);
        int TextWidth { get; }
        int TextHeight { get; }
    }
}