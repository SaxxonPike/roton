using Lyon.Common.App;

namespace Torch.App;

public interface IAppWindow : IWindow
{
    unsafe SDL_Window* GetWindowPtr();
    void OpenWorld(string file);
}