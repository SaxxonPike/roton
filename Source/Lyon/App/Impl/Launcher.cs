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
    /// <summary>
    /// Handles when the engine exits.
    /// </summary>
    private void OnExited(object? sender, EventArgs e)
    {
        // When the game engine has exited, no need to keep the window open.
        window.Close();
    }

    /// <inheritdoc />
    public void Launch(IEngine engine)
    {
        audioPresenter.Start(engine);
        engine.Exited += OnExited;
        engine.Start();
        window.Start();
        engine.Stop();
        audioPresenter.Stop();
    }
}