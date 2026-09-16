using System;
using JetBrains.Annotations;

namespace Torch.Gui;

/// <summary>
/// Represents an ImGui backend implementation.
/// </summary>
[PublicAPI]
public interface IImGuiBackend : IDisposable
{
    /// <summary>
    /// Gets the ImGui context pointer.
    /// </summary>
    ImGuiContextPtr Context { get; }

    /// <summary>
    /// Prepares for a new ImGui frame.
    /// </summary>
    void NewFrame();

    /// <summary>
    /// Renders the ImGui draw data.
    /// </summary>
    /// <param name="drawData">
    /// ImGui draw data.
    /// </param>
    void RenderDrawData(ImDrawDataPtr drawData);

    /// <summary>
    /// Processes an SDL event.
    /// </summary>
    /// <param name="ev">
    /// SDL event to process.
    /// </param>
    unsafe void ProcessEvent(SDL_Event* ev);
}