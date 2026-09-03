using System.Collections.Generic;

namespace Roton.Emulation.Data;

public static class ScrollContentExtensions
{
    public static void AddLines(this IScrollContent scrollContent, params IEnumerable<string> lines)
    {
        foreach (var line in lines)
            scrollContent.AddLine(line);
    }

    public static string GetLine(this IScrollContent scrollContent, int index)
    {
        var line = (stackalloc char[512]);
        var actualLine = scrollContent.GetLine(index, line);
        return actualLine.ToString();
    }
}