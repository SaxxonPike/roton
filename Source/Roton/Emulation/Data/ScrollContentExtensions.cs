using System;

namespace Roton.Emulation.Data;

internal static class ScrollContentExtensions
{
    extension(IScrollContent scrollContent)
    {
        public void AddLine(char command, ReadOnlySpan<char> text)
        {
            var buffer = (stackalloc char[text.Length + 1]);
            buffer[0] = command;
            text.CopyTo(buffer.Slice(1));
            scrollContent.AddLine(buffer);
        }
        
        public void AddLines(params string[] lines)
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