using System;
using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IScroll
{
    ScrollResult ShowHelpFile(string? title, string fileName);

    ScrollResult ShowMessage(ReadOnlySpan<char> title, bool isHelp, int index,
        Func<ScrollState, ScrollResult>? mainLoop = null);

    int TextWidth { get; }
    int TextHeight { get; }
}
