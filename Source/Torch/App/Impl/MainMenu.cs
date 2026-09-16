namespace Torch.App.Impl;

public static class MainMenu
{
    public static unsafe void Render(IAppWindow window)
    {
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.BeginMenu("New"))
                {
                    if (ImGui.MenuItem("ZZT World"))
                    {
                    }

                    if (ImGui.MenuItem("Super ZZT World"))
                    {
                    }

                    ImGui.EndMenu();
                }

                if (ImGui.MenuItem("Open..."))
                {
                    FileDialog.Show(
                        window.GetWindowPtr(),
                        [("ZZT Worlds", "zzt"), ("Super ZZT Worlds", "szt"), ("All Files", "*")],
                        true,
                        files =>
                        {
                            foreach (var file in files)
                                window.OpenWorld(file);
                        });
                }

                ImGui.Separator();

                if (ImGui.MenuItem("Quit"))
                {
                    unsafe
                    {
                        var ev = new SDL_Event { type = (int)SDL_EventType.SDL_EVENT_QUIT };
                        SDL_PushEvent(&ev);
                    }
                }

                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();
        }
    }
}