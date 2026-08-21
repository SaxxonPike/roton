using Lyon.Presenters;
using Roton;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Lyon.App.Impl;

[Context(Context.Startup)]
public sealed unsafe class Window : IWindow
{
    private readonly SDL_Window* _window;
    private readonly SDL_Renderer* _renderer;
    private readonly SDL_Texture* _background;
    private readonly IKeyboardPresenter _keyboardPresenter;
    private readonly IScenePresenter _scenePresenter;
    private readonly IJoystickPresenter _joystickPresenter;

    private bool _closeWindow;

    public Window(
        IConfig config,
        IKeyboardPresenter keyboardPresenter,
        IScenePresenter scenePresenter,
        IJoystickPresenter joystickPresenter)
    {
        _keyboardPresenter = keyboardPresenter;
        _scenePresenter = scenePresenter;
        _joystickPresenter = joystickPresenter;

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
    }

    public int RenderWidth { get; }

    public int RenderHeight { get; }

    public int WindowWidth { get; }

    public int WindowHeight { get; }

    public string Title { get; }

    public bool Running { get; private set; }

    private void Loop()
    {
        SDL_ShowWindow(_window);
        Running = true;

        while (Running)
        {
            SDL_Event e;

            while (SDL_PollEvent(&e))
            {
                switch (e.Type)
                {
                    case SDL_EventType.SDL_EVENT_QUIT:
                    case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                        Close();
                        break;
                    case SDL_EventType.SDL_EVENT_KEY_DOWN:
                        _keyboardPresenter.Press(e.key);
                        break;
                    case SDL_EventType.SDL_EVENT_KEY_UP:
                        _keyboardPresenter.Release(e.key);
                        break;
                    case SDL_EventType.SDL_EVENT_GAMEPAD_ADDED:
                        _joystickPresenter.Connect(e.gdevice.which);
                        break;
                    case SDL_EventType.SDL_EVENT_GAMEPAD_REMOVED:
                        _joystickPresenter.Disconnect(e.gdevice.which);
                        break;
                    case SDL_EventType.SDL_EVENT_GAMEPAD_AXIS_MOTION:
                        switch (e.gaxis.Axis)
                        {
                            case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_LEFTX:
                                _joystickPresenter.UpdateAxis(e.gaxis.which, JoystickAxis.X, e.gaxis.value / 32768f);
                                break;
                            case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_LEFTY:
                                _joystickPresenter.UpdateAxis(e.gaxis.which, JoystickAxis.Y, e.gaxis.value / 32768f);
                                break;
                        }

                        break;
                    case SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_DOWN:
                        switch (e.gbutton.Button)
                        {
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_SOUTH:
                                _joystickPresenter.UpdateButton(e.gbutton.which, JoystickButtons.Primary, true);
                                break;
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_WEST:
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_EAST:
                                _joystickPresenter.UpdateButton(e.gbutton.which, JoystickButtons.Secondary, true);
                                break;
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_LEFT:
                                _joystickPresenter.UpdateAxis(e.gbutton.which, JoystickAxis.X, -1);
                                break;
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_RIGHT:
                                _joystickPresenter.UpdateAxis(e.gbutton.which, JoystickAxis.X, 1);
                                break;
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_UP:
                                _joystickPresenter.UpdateAxis(e.gbutton.which, JoystickAxis.Y, -1);
                                break;
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_DOWN:
                                _joystickPresenter.UpdateAxis(e.gbutton.which, JoystickAxis.Y, 1);
                                break;
                        }

                        break;
                    case SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_UP:
                        switch (e.gbutton.Button)
                        {
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_SOUTH:
                                _joystickPresenter.UpdateButton(e.gbutton.which, JoystickButtons.Primary, false);
                                break;
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_WEST:
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_EAST:
                                _joystickPresenter.UpdateButton(e.gbutton.which, JoystickButtons.Secondary, false);
                                break;
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_LEFT:
                                _joystickPresenter.UpdateAxis(e.gbutton.which, JoystickAxis.X, 0);
                                break;
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_RIGHT:
                                _joystickPresenter.UpdateAxis(e.gbutton.which, JoystickAxis.X, 0);
                                break;
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_UP:
                                _joystickPresenter.UpdateAxis(e.gbutton.which, JoystickAxis.Y, 0);
                                break;
                            case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_DOWN:
                                _joystickPresenter.UpdateAxis(e.gbutton.which, JoystickAxis.Y, 0);
                                break;
                        }

                        break;
                }
            }

            var bitmap = _scenePresenter.Render();
            if (bitmap.Bits.Length > 0)
                SDL_UpdateTexture(_background, null, bitmap.BitsPointer, RenderWidth * 4);

            SDL_SetRenderLogicalPresentation(
                _renderer,
                WindowWidth, WindowHeight,
                SDL_RendererLogicalPresentation.SDL_LOGICAL_PRESENTATION_LETTERBOX
            );
            
            SDL_RenderTexture(_renderer, _background, null, null);
            SDL_RenderPresent(_renderer);

            SDL_SetRenderLogicalPresentation(
                _renderer,
                0, 0,
                SDL_RendererLogicalPresentation.SDL_LOGICAL_PRESENTATION_DISABLED
            );

            if (_closeWindow)
                break;
        }

        Running = false;
        SDL_DestroyWindow(_window);
    }

    public void SetSize(int width, int height)
    {
        _scenePresenter.UpdateViewport();
    }

    public void Close()
    {
        _closeWindow = true;
    }

    public void Start(float rate)
    {
        Loop();
    }
}