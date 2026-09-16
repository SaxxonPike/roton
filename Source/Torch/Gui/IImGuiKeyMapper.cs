using JetBrains.Annotations;

namespace Torch.Gui;

[PublicAPI]
public interface IImGuiKeyMapper
{
    /// <summary>
    /// Map an SDL keyboard modifier to ImGui key.
    /// </summary>
    /// <param name="mod">
    /// SDL key modifier.
    /// </param>
    /// <returns>
    /// ImGui key. Will return null if no mapping exists.
    /// </returns>
    ImGuiKey? ConvertKeyboardMod(SDL_Keymod mod);

    /// <summary>
    /// Map an SDL keyboard key to ImGui key. Falls back to scan codes for
    /// unmapped keys.
    /// </summary>
    /// <param name="ev">
    /// SDL event data.
    /// </param>
    /// <returns>
    /// ImGui key. Will return null if no mapping exists.
    /// </returns>
    ImGuiKey? ConvertKeyboardEventKey(SDL_KeyboardEvent ev);

    /// <summary>
    /// Map an SDL keyboard scan code to ImGui key.
    /// </summary>
    /// <param name="ev">
    /// SDL event data.
    /// </param>
    /// <returns>
    /// ImGui key. Will return null if no mapping exists.
    /// </returns>
    ImGuiKey? ConvertKeyboardEventScanCode(SDL_KeyboardEvent ev);
}