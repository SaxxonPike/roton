using System;
using Microsoft.Extensions.DependencyInjection;

namespace Torch.Gui.Impl;

/// <inheritdoc />
public class ImGuiBackendManager(
    IServiceProvider serviceProvider)
    : IImGuiBackendManager
{
    /// <inheritdoc />
    public unsafe IImGuiBackend Create(SDL_Window* window, SDL_Renderer* renderer)
    {
        var context = ImGui.CreateContext();

        return ActivatorUtilities.CreateInstance<ImGuiBackend>(
            serviceProvider,
            context,
            (IntPtr)window,
            (IntPtr)renderer
        );
    }
}