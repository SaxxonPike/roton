using System;

namespace Roton.Emulation.Data;

public interface IScrollContent
{
    int LineCount { get; }
    void AddLine(ReadOnlySpan<char> text);
    ReadOnlySpan<char> GetLine(int index, Span<char> buffer);
    void ClearLines();
}