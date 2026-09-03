using System.Collections.Generic;

namespace Roton.Emulation.Data;

public static class ScrollContentExtensions
{
    public static void AddLines(this IScrollContent scrollContent, IEnumerable<string> lines)
    {
        foreach (var line in lines)
            scrollContent.AddLine(line);
    }
}