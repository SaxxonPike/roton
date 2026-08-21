using System;
using Lyon.Presenters;
using Roton;
using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Lyon.App.Impl;

[Context(Context.Startup)]
public sealed class Launcher(
    IWindow window,
    IAudioPresenter audioPresenter)
    : ILauncher
{
    private IWindow Window => window;
    private IAudioPresenter AudioPresenter => audioPresenter;

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