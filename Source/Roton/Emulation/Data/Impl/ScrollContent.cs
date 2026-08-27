using System;
using System.IO;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

public abstract class ScrollContent : IScrollContent
{
    private readonly MemoryStream _stream = new();

    public abstract int LineWidth { get; }

    public int LineCount { get; private set; }

    private void SetLinePosition(int index) =>
        _stream.Position = index * (LineWidth + 1);

    public void AddLine(ReadOnlySpan<char> text)
    {
        SetLinePosition(LineCount);
        var len = unchecked((byte)text.Length);
#if NET10_0_OR_GREATER
        var lineBytes = (stackalloc byte[Math.Max(LineWidth, len) + 1]);
        lineBytes[0] = len;
        Cp437.CharsToBytes(text[..len], lineBytes[1..]);
        _stream.Write(lineBytes);
#else
        var lineBytes = new byte[Math.Max(LineWidth, len) + 1];
        lineBytes[0] = len;
        Cp437.CharsToBytes(text.Slice(0, len), lineBytes.AsSpan(1));
        _stream.Write(lineBytes, 0, lineBytes.Length);
#endif
        LineCount++;
    }

    public ReadOnlySpan<char> GetLine(int index, Span<char> buffer)
    {
        SetLinePosition(index);
        var len = (byte)_stream.ReadByte();
#if NET10_0_OR_GREATER
        var lineBytes = (stackalloc byte[len]);
        _stream.ReadExactly(lineBytes);
        var actualLen = Cp437.BytesToChars(lineBytes, buffer);
        return buffer[..actualLen];
#else
        var lineBytes = new byte[len];
        var actualLen = Cp437.BytesToChars(lineBytes, buffer);
        return buffer.Slice(0, actualLen);
#endif
    }

    public void ClearLines() =>
        LineCount = 0;
}