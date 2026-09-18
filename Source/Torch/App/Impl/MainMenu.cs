using Roton;
using Torch.Gui.Components;

namespace Torch.App.Impl;

public class MainMenuUi : Container
{
    private readonly IAppWindow _app;

    public MainMenuUi(IAppWindow app) : base([])
    {
        _app = app;
        Components.Add(
            new MainMenuBar(
                new Menu(
                    x => { x.Label = _ => "File"; },
                    new Menu(
                        x => { x.Label = _ => "New"; },
                        new MenuItem(x =>
                        {
                            x.Label = _ => "ZZT World";
                            x.OnClick = OnFileNewClick;
                            x.DataObject = Context.Original;
                        }),
                        new MenuItem(x =>
                        {
                            x.Label = _ => "Super ZZT World";
                            x.OnClick = OnFileNewClick;
                            x.DataObject = Context.Super;
                        })
                    ),
                    new MenuItem(x =>
                    {
                        x.Label = _ => "Open...";
                        x.OnClick = OnFileOpenClick;
                    }),
                    new Separator(),
                    new MenuItem(x =>
                    {
                        x.Label = _ => "Quit";
                        x.OnClick = OnFileQuitClick;
                    })
                )
            )
        );
    }

    private static unsafe void OnFileQuitClick(MenuItem menuItem)
    {
        var ev = new SDL_Event { type = (int)SDL_EventType.SDL_EVENT_QUIT };
        SDL_PushEvent(&ev);
    }

    private unsafe void OnFileOpenClick(MenuItem menuItem)
    {
        FileDialog.Show(
            _app.GetWindowPtr(),
            [("ZZT Worlds", "zzt"), ("Super ZZT Worlds", "szt"), ("All Files", "*")],
            true,
            files =>
            {
                foreach (var file in files)
                    _app.OpenWorld(file);
            });
    }

    private void OnFileNewClick(MenuItem menuItem) =>
        _app.NewWorld(menuItem.Data<Context>());
}