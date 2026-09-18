using Lyon.Common.App;
using Roton;

namespace Torch.App;

public interface IAppWindow : IWindow
{
    unsafe SDL_Window* GetWindowPtr();
    void NewWorld(Context context);
    void OpenWorld(string file);
}