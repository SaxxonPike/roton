using System;
using Lyon.Presenters;
using Roton;
using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Lyon.App.Impl;

/// <inheritdoc />
[Context(Context.Startup)]
public sealed class Launcher(
    IWindow window,
    IAudioPresenter audioPresenter)
    : ILauncher
{
    private IWindow Window => window;
    private IAudioPresenter AudioPresenter => audioPresenter;

    /// <summary>
    /// Handles when the engine exits.
    /// </summary>
    private void OnExited(object? sender, EventArgs e)
    {
        // When the game engine has exited, no need to keep the window open.
        Window.Close();
    }

    /// <inheritdoc />
    public void Launch(IEngine engine)
    {
        AudioPresenter.Start(engine);
        engine.Exited += OnExited;
        engine.Start();
        Window.Start();
        engine.Stop();
        AudioPresenter.Stop();
    }
}