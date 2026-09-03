using System;

namespace Roton.Emulation.Data;

public interface IScrollContent
{
    int LineWidth { get; }
    int LineCount { get; }
    void AddLine(ReadOnlySpan<char> text);
    ReadOnlySpan<char> GetLine(int index, Span<char> buffer);
    void ClearLines();
}