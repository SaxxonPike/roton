using Lyon.Common.App;

namespace Torch.App;

public interface IAppWindow : IWindow
{
    unsafe SDL_Window* GetWindowPtr();
    void NewZztWorld();
    void NewSuperZztWorld();
    void OpenWorld(string file);
}