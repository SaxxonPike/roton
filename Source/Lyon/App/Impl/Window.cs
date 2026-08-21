using Lyon.Presenters;
using Roton;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Lyon.App.Impl;

[Context(Context.Startup)]
internal sealed unsafe class Window(
    IConfig config,
    IKeyboardPresenter keyboardPresenter,
    IScenePresenter scenePresenter,
    IJoystickPresenter joystickPresenter)
    : IWindow
{
    /// <summary>
    /// The SDL window that will be rendered to.
    /// </summary>
    private SDL_Window* _window;

    /// <summary>
    /// The SDL renderer that will be used to render the backbuffer.
    /// </summary>
    private SDL_Renderer* _renderer;

    /// <summary>
    /// Backbuffer texture.
    /// </summary>
    private SDL_Texture* _background;

    /// <summary>
    /// If true, the window is to be closed.
    /// </summary>
    private bool _closeWindow;

    /// <summary>
    /// Width of the backbuffer texture.
    /// </summary>
    public int RenderWidth { get; private set; }

    /// <summary>
    /// Height of the backbuffer texture.
    /// </summary>
    public int RenderHeight { get; private set; }

    /// <summary>
    /// Unscaled width of the window.
    /// </summary>
    public int WindowWidth { get; private set; }

    /// <summary>
    /// Unscaled height of the window.
    /// </summary>
    public int WindowHeight { get; private set; }

    /// <summary>
    /// Title of the window.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// If true, the window loop is running.
    /// </summary>
    public bool Running { get; private set; }

    /// <summary>
    /// Handles <see cref="SDL_EventType.SDL_EVENT_KEY_DOWN"/>.
    /// </summary>
    private void HandleKeyDown(ref SDL_KeyboardEvent e) =>
        keyboardPresenter.Press(e.key, e.mod);

    /// <summary>
    /// Handles <see cref="SDL_EventType.SDL_EVENT_KEY_UP"/>.
    /// </summary>
    private void HandleKeyUp(ref SDL_KeyboardEvent e) =>
        keyboardPresenter.Release(e.key, e.mod);

    /// <summary>
    /// Handles <see cref="SDL_EventType.SDL_EVENT_GAMEPAD_ADDED"/>.
    /// </summary>
    private void HandleGamepadAdded(ref SDL_GamepadDeviceEvent e) =>
        joystickPresenter.Connect(e.which);

    /// <summary>
    /// Handles <see cref="SDL_EventType.SDL_EVENT_GAMEPAD_REMOVED"/>.
    /// </summary>
    private void HandleGamepadRemoved(ref SDL_GamepadDeviceEvent e) =>
        joystickPresenter.Disconnect(e.which);

    /// <summary>
    /// Handles <see cref="SDL_EventType.SDL_EVENT_GAMEPAD_AXIS_MOTION"/>.
    /// </summary>
    private void HandleGamepadAxis(ref SDL_GamepadAxisEvent e)
    {
        switch (e.Axis)
        {
            case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_LEFTX:
                joystickPresenter.UpdateAxis(e.which, JoystickAxis.X, e.value / 32768f);
                break;
            case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_LEFTY:
                joystickPresenter.UpdateAxis(e.which, JoystickAxis.Y, e.value / 32768f);
                break;
        }
    }

    /// <summary>
    /// Handles <see cref="SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_DOWN"/>.
    /// </summary>
    private void HandleGamepadButtonDown(ref SDL_GamepadButtonEvent e)
    {
        switch (e.Button)
        {
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_SOUTH:
                joystickPresenter.UpdateButton(e.which, JoystickButtons.Primary, true);
                break;
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_WEST:
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_EAST:
                joystickPresenter.UpdateButton(e.which, JoystickButtons.Secondary, true);
                break;
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_LEFT:
                joystickPresenter.UpdateAxis(e.which, JoystickAxis.X, -1);
                break;
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_RIGHT:
                joystickPresenter.UpdateAxis(e.which, JoystickAxis.X, 1);
                break;
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_UP:
                joystickPresenter.UpdateAxis(e.which, JoystickAxis.Y, -1);
                break;
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_DOWN:
                joystickPresenter.UpdateAxis(e.which, JoystickAxis.Y, 1);
                break;
        }
    }

    /// <summary>
    /// Handles <see cref="SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_UP"/>.
    /// </summary>
    private void HandleGamepadButtonUp(ref SDL_GamepadButtonEvent e)
    {
        switch (e.Button)
        {
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_SOUTH:
                joystickPresenter.UpdateButton(e.which, JoystickButtons.Primary, false);
                break;
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_WEST:
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_EAST:
                joystickPresenter.UpdateButton(e.which, JoystickButtons.Secondary, false);
                break;
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_LEFT:
                joystickPresenter.UpdateAxis(e.which, JoystickAxis.X, 0);
                break;
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_RIGHT:
                joystickPresenter.UpdateAxis(e.which, JoystickAxis.X, 0);
                break;
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_UP:
                joystickPresenter.UpdateAxis(e.which, JoystickAxis.Y, 0);
                break;
            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_DOWN:
                joystickPresenter.UpdateAxis(e.which, JoystickAxis.Y, 0);
                break;
        }
    }

    /// <summary>
    /// Handles an SDL event.
    /// </summary>
    private void HandleEvent(ref SDL_Event e)
    {
        switch (e.Type)
        {
            case SDL_EventType.SDL_EVENT_QUIT:
            case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                Close();
                break;
            case SDL_EventType.SDL_EVENT_KEY_DOWN:
                HandleKeyDown(ref e.key);
                break;
            case SDL_EventType.SDL_EVENT_KEY_UP:
                HandleKeyUp(ref e.key);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_ADDED:
                HandleGamepadAdded(ref e.gdevice);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_REMOVED:
                HandleGamepadRemoved(ref e.gdevice);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_AXIS_MOTION:
                HandleGamepadAxis(ref e.gaxis);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_DOWN:
                HandleGamepadButtonDown(ref e.gbutton);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_UP:
                HandleGamepadButtonUp(ref e.gbutton);
                break;
        }
    }

    /// <summary>
    /// Runs the window loop until quit.
    /// </summary>
    private void Loop()
    {
        SDL_ShowWindow(_window);
        Running = true;

        while (Running)
        {
            SDL_Event e;

            // Poll for pending events.
            while (SDL_PollEvent(&e))
                HandleEvent(ref e);

            // If the window is closed, exit.
            if (_closeWindow)
                break;

            // Render the scene.
            var bitmap = scenePresenter.Render();
            if (bitmap.Bits.Length > 0)
                SDL_UpdateTexture(_background, null, bitmap.BitsPointer, RenderWidth * 4);

            // Set the scene scale.
            SDL_SetRenderLogicalPresentation(
                _renderer,
                WindowWidth, WindowHeight,
                SDL_RendererLogicalPresentation.SDL_LOGICAL_PRESENTATION_LETTERBOX
            );

            // Present the scene.
            SDL_RenderTexture(_renderer, _background, null, null);
            SDL_RenderPresent(_renderer);

            // Reset the scene scale.
            SDL_SetRenderLogicalPresentation(
                _renderer,
                0, 0,
                SDL_RendererLogicalPresentation.SDL_LOGICAL_PRESENTATION_DISABLED
            );
        }

        // Clean up the window.
        Running = false;
    }

    /// <inheritdoc />
    public void Close()
    {
        _closeWindow = true;
    }

    /// <inheritdoc />
    public void Start()
    {
        // If already running, bail.
        if (Running)
            return;

        // Reset state.
        _closeWindow = false;

        // Window defaults.
        Title = "Lyon";
        WindowWidth = (int)(640 * config.VideoScaleX);
        WindowHeight = (int)(350 * config.VideoScaleY);
        RenderWidth = 640;
        RenderHeight = 350;

        // Start SDL video subsystem.
        SDL_InitSubSystem(SDL_InitFlags.SDL_INIT_VIDEO);

        // Create the window and renderer. The window starts hidden
        // so we can show it when we are ready to render.
        SDL_Window* window;
        SDL_Renderer* renderer;
        SDL_CreateWindowAndRenderer(
            Title,
            WindowWidth, WindowHeight,
            SDL_WindowFlags.SDL_WINDOW_HIDDEN | SDL_WindowFlags.SDL_WINDOW_RESIZABLE,
            &window,
            &renderer
        );
        _window = window;
        _renderer = renderer;

        // Create the background texture to which we will render the scene.
        _background = SDL_CreateTexture(
            _renderer,
            SDL_PIXELFORMAT_BGRA32,
            SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING,
            RenderWidth, RenderHeight
        );

        // Set scale mode to pixel art so that it looks appropriate.
        SDL_SetTextureScaleMode(_background, SDL_ScaleMode.SDL_SCALEMODE_PIXELART);

        // Not all adapters support adaptive vsync, so use the regular
        // method if this fails.
        if (!SDL_SetRenderVSync(renderer, SDL_RENDERER_VSYNC_ADAPTIVE))
            SDL_SetRenderVSync(renderer, 1);

        // Start the main loop.
        Loop();

        // Clean up the window.
        SDL_DestroyWindow(_window);
    }
}