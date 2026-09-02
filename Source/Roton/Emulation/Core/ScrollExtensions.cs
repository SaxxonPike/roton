using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public static class ScrollExtensions
{
    public static IScrollState Show(this IScroll scroll, string? title, params IEnumerable<string> message) =>
        scroll.ShowMessage(title, message, false, 0);
}