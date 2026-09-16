using JetBrains.Annotations;

namespace Torch.Gui;

/// <summary>
/// Manages the creation of ImGui backends.
/// </summary>
[PublicAPI]
public interface IImGuiBackendManager
{
    /// <summary>
    /// Creates an ImGui backend for the specified window and renderer.
    /// </summary>
    /// <param name="window">
    /// SDL window pointer.
    /// </param>
    /// <param name="renderer">
    /// SDL renderer pointer.
    /// </param>
    /// <returns>
    /// The created ImGui backend.
    /// </returns>
    unsafe IImGuiBackend Create(SDL_Window* window, SDL_Renderer* renderer);
}