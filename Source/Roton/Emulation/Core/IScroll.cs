using System;
using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IScroll
{
    IScrollState ShowHelpFile(string title, string fileName);
    IScrollState ShowMessage(string? title, IEnumerable<string> message, bool isHelp, int index);
    IScrollState ShowMessage(string title, IEnumerable<string> message, bool isHelp, int index, Action<IScrollState> mainLoop);
    int TextWidth { get; }
    int TextHeight { get; }
}