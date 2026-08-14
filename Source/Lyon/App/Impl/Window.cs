using System;
using DotSDL.Events;
using DotSDL.Graphics;
using Lyon.Presenters;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Lyon.App.Impl;

[Context(Context.Startup)]
public sealed class Window : SdlWindow, IWindow
{
    private readonly Lazy<IKeyboardPresenter> _keyboardPresenter;
    private readonly Lazy<IScenePresenter> _scenePresenter;
    private bool _closeWindow;

    private IKeyboardPresenter KeyboardPresenter => _keyboardPresenter.Value;
    private IScenePresenter ScenePresenter => _scenePresenter.Value;

    public Window(
        IConfig config,
        Lazy<IKeyboardPresenter> keyboardPresenter,
        Lazy<IScenePresenter> scenePresenter) : base("Lyon",
        new Point {X = WindowPosUndefined, Y = WindowPosUndefined},
        (int)(640 * config.VideoScaleX), (int)(350 * config.VideoScaleY),
        640, 350)
    {
        _keyboardPresenter = keyboardPresenter;
        _scenePresenter = scenePresenter;
        KeyPressed += OnKeyDown;
        Closed += OnClosed;

        ScalingQuality = ScalingQuality.PixelArt;

        Background.GetCanvasPointer = () => {
            var bitmap = ScenePresenter.Render();
            return bitmap.Bits.Length == 0 ? IntPtr.Zero : bitmap.BitsPointer;
        };
    }

    private void OnClosed(object sender, WindowEvent e)
    {
        Close();
    }

    public void SetSize(int width, int height)
    {
        ScenePresenter.UpdateViewport();
    }

    public void Close()
    {
        _closeWindow = true;
    }

    private void OnKeyDown(object obj, KeyboardEvent e)
    {
        KeyboardPresenter.Press(e);
    }

    private void OnKeyUp(object obj, KeyboardEvent e)
    {
        KeyboardPresenter.Release(e);
    }

    protected override void OnUpdate(float delta)
    {
        if(_closeWindow)
            Stop();
    }
}