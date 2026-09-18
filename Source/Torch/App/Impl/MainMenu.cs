using Torch.Gui.Components;

namespace Torch.App.Impl;

public static class MainMenuUi
{
    public static Component Create(IAppWindow app)
    {
        return MainMenuBar.Create(
            _ => { },
            Menu.Create(
                app,
                file => { file.Label = _ => "File"; },
                Menu.Create(
                    app,
                    fileNew => { fileNew.Label = _ => "New"; },
                    MenuItem.Create(
                        app,
                        fileNewZzt =>
                        {
                            fileNewZzt.Label = _ => "ZZT World";
                            fileNewZzt.OnClick = OnFileNewZztClick;
                        }
                    ),
                    MenuItem.Create(
                        app,
                        fileNewSuperZzt =>
                        {
                            fileNewSuperZzt.Label = _ => "Super ZZT World";
                            fileNewSuperZzt.OnClick = OnFileNewSuperZztClick;
                        })
                ),
                MenuItem.Create(
                    app,
                    fileOpen =>
                    {
                        fileOpen.Label = _ => "Open...";
                        fileOpen.OnClick = OnFileOpenClick;
                    }),
                Separator.Create(),
                MenuItem.Create(
                    app,
                    fileQuit =>
                    {
                        fileQuit.Label = _ => "Quit";
                        fileQuit.OnClick = OnFileQuitClick;
                    })
            )
        );
    }

    private static void OnFileNewSuperZztClick(IAppWindow app)
    {
        app.NewSuperZztWorld();
    }

    private static unsafe void OnFileQuitClick(IAppWindow _)
    {
        var ev = new SDL_Event { type = (int)SDL_EventType.SDL_EVENT_QUIT };
        SDL_PushEvent(&ev);
    }

    private static unsafe void OnFileOpenClick(IAppWindow app)
    {
        FileDialog.Show(
            app.GetWindowPtr(),
            [("ZZT Worlds", "zzt"), ("Super ZZT Worlds", "szt"), ("All Files", "*")],
            true,
            files =>
            {
                foreach (var file in files)
                    app.OpenWorld(file);
            });
    }

    private static void OnFileNewZztClick(IAppWindow app)
    {
        app.NewZztWorld();
    }
}