using Lyon.Presenters;
using Roton;
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

    private bool _closeWindow;

    public Window(
        IConfig config,
        IKeyboardPresenter keyboardPresenter,
        IScenePresenter scenePresenter)
    {
        Title = "Lyon";
        WindowWidth = (int)(640 * config.VideoScaleX);
        WindowHeight = (int)(350 * config.VideoScaleY);
        RenderWidth = 640;
        RenderHeight = 350;

        _keyboardPresenter = keyboardPresenter;
        _scenePresenter = scenePresenter;

        SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO);

        SDL_Window* window;
        SDL_Renderer* renderer;

        // Create the window and renderer. The window starts hidden
        // so we can show it when we are ready to render.
        SDL_CreateWindowAndRenderer(Title, WindowWidth, WindowHeight,
            SDL_WindowFlags.SDL_WINDOW_HIDDEN, &window, &renderer);

        _window = window;
        _renderer = renderer;

        // Create the background texture to which we will render the scene.
        _background = SDL_CreateTexture(
            _renderer,
            SDL_PIXELFORMAT_BGRA32,
            SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING,
            RenderWidth,
            RenderHeight
        );

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

    private void Loop(float rate)
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
                }
            }

            var bitmap = _scenePresenter.Render();
            if (bitmap.Bits.Length > 0)
                SDL_UpdateTexture(_background, null, bitmap.BitsPointer, RenderWidth * 4);

            SDL_RenderTexture(_renderer, _background, null, null);
            SDL_RenderPresent(_renderer);

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
        Loop(rate);
    }
}