using System.Collections.Generic;

namespace Roton.Emulation.Data;

internal static class ScrollContentExtensions
{
    extension(IScrollContent scrollContent)
    {
        public void AddLines(params IEnumerable<string?> lines)
        {
            foreach (var line in lines)
                scrollContent.AddLine(line);
        }

        public string GetLine(int index)
        {
            var line = (stackalloc char[512]);
            var actualLine = scrollContent.GetLine(index, line);
            return actualLine.ToString();
        }
    }
}