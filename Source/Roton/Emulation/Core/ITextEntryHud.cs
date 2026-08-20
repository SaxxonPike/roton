using System;

namespace Roton.Emulation.Core;

public interface ITextEntryHud
{
    string Show(int x, int y, int maxLength, int textColor, int pipColor, ReadOnlySpan<char> initText = default);
}