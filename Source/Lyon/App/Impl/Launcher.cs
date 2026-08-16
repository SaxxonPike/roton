using System;
using Lyon.Presenters;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Lyon.App.Impl;

[Context(Context.Startup)]
public sealed class Launcher : ILauncher
{
    private readonly IWindow _window;
    private readonly IAudioPresenter _audioPresenter;

    public Launcher(
        IWindow window,
        IAudioPresenter audioPresenter)
    {
        _window = window;
        _audioPresenter = audioPresenter;
    }

    private IWindow Window => _window;
    private IAudioPresenter AudioPresenter => _audioPresenter;

    private void OnExited(object sender, EventArgs e)
    {
        Window.Close();
    }

    public void Launch(IEngine engine)
    {
        AudioPresenter.Start();
        engine.Exited += OnExited;
        engine.Start();
        Window.Start(72.75f);
        engine.Stop();
        AudioPresenter.Stop();
    }
}