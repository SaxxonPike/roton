using System;

namespace Roton.Emulation.Core;

public interface ITextEntryHud
{
    /// <remarks>
    /// RoZ: PromptString
    /// </remarks>
    string Show(int x, int y, int maxLength, int textColor, int pipColor, ReadOnlySpan<char> initText = default);
}