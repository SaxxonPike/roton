using Microsoft.Extensions.DependencyInjection;
using Silverwind.Ursula;

namespace Torch.ImGui;

/// <inheritdoc />
public class ImGuiBackendManager(
    IServiceProvider serviceProvider)
    : IImGuiBackendManager
{
    /// <inheritdoc />
    public unsafe IImGuiBackend Create(SDL_Window* window, SDL_Renderer* renderer)
    {
        var context = Hexa.NET.ImGui.ImGui.CreateContext();

        return ActivatorUtilities.CreateInstance<IImGuiBackend>(
            serviceProvider,
            context,
            (IntPtr)window,
            (IntPtr)renderer
        );
    }
}