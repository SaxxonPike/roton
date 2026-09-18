using System;
using System.Collections.Generic;
using System.IO;
using Lyon.Common;
using Microsoft.Extensions.DependencyInjection;
using Roton;
using Roton.Editors;
using Roton.Emulation.Core;
using Roton.Infrastructure;
using Torch.Gui;
using Torch.Gui.Components;

namespace Torch.App.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed unsafe class AppWindow(
    IConfig config,
    IImGuiBackendManager imGuiBackendManager,
    IServiceProvider serviceProvider)
    : IAppWindow
{
    /// <summary>
    /// Used for SDL subsystem reference counting.
    /// </summary>
    private SdlContext? _sdlContext;

    /// <summary>
    /// The SDL window that will be rendered to.
    /// </summary>
    private SDL_Window* _window;

    /// <summary>
    /// The SDL renderer that will be used to render the backbuffer.
    /// </summary>
    private SDL_Renderer* _renderer;

    /// <summary>
    /// If true, the window is to be closed.
    /// </summary>
    private bool _closeWindow;

    /// <summary>
    /// ImGui backend renderer.
    /// </summary>
    private IImGuiBackend? _imGuiBackend;

    private readonly List<Component> _tempComponents = [];

    private readonly List<Component> _components = [];

    private TextureManager? _textureManager;

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
    public string Title { get; private set; } = "Lyon";

    /// <summary>
    /// If true, the window loop is running.
    /// </summary>
    public bool Running { get; private set; }

    /// <summary>
    /// Handles an SDL event.
    /// </summary>
    private void HandleEvent(ref SDL_Event e)
    {
        var ev = e;
        _imGuiBackend?.ProcessEvent(&ev);

        switch (ev.Type)
        {
            case SDL_EventType.SDL_EVENT_QUIT:
            case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                Close();
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

            // Begin the scene.
            _imGuiBackend?.NewFrame();
            ImGui.NewFrame();

            _tempComponents.Clear();
            _tempComponents.AddRange(_components);

            foreach (var component in _tempComponents)
                component.Render();

            // Render the scene.
            SDL_SetRenderDrawColor(_renderer, 0, 0, 0, 255);
            SDL_RenderClear(_renderer);
            ImGui.Render();
            _imGuiBackend?.RenderDrawData(ImGui.GetDrawData());

            // Present the scene.
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

    /// <summary>
    /// Finds the largest integer scale for the given window size that will fit on screen.
    /// </summary>
    private static int FindMaxIntegerScale(int width, int height)
    {
        SDL_Rect rect;
        using var displays = SDL_GetDisplays();

        if (displays is null || displays.Count < 1 || !SDL_GetDisplayBounds(displays[0], &rect))
            return 1;

        return Math.Max(1, Math.Min(rect.w / width, rect.h / height));
    }

    /// <inheritdoc />
    public void Start()
    {
        // If already running, bail.
        if (Running)
            return;

        // Reset state.
        _closeWindow = false;

        // Start SDL video subsystem.
        _sdlContext = SdlContext.Create(SDL_InitFlags.SDL_INIT_VIDEO);

        // Window defaults.
        RenderWidth = 640;
        RenderHeight = 350;
        var winWidth = (int)Math.Round(Math.Max(RenderWidth * config.Video.ScaleX, 1));
        var winHeight = (int)Math.Round(Math.Max(RenderHeight * config.Video.ScaleY, 1));
        var integerScale = FindMaxIntegerScale(winWidth, winHeight);
        WindowWidth = winWidth * integerScale;
        WindowHeight = winHeight * integerScale;

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
        _imGuiBackend = imGuiBackendManager.Create(_window, _renderer);
        _textureManager = new TextureManager(_renderer);

        // Not all adapters support adaptive vsync, so use the regular
        // method if this fails.
        if (!SDL_SetRenderVSync(renderer, SDL_RENDERER_VSYNC_ADAPTIVE))
            SDL_SetRenderVSync(renderer, 1);

        // Build main UI component.
        var mainMenuUi = new MainMenuUi(this);
        _components.Add(mainMenuUi);

        // Start the main loop.
        Loop();

        // Tear down main UI component.
        _components.Remove(mainMenuUi);

        // Clean up the window.
        SDL_DestroyWindow(_window);

        // Free SDL subsystems.
        _imGuiBackend.Dispose();
        _imGuiBackend = null;
        _sdlContext.Dispose();
        _sdlContext = null;
        _textureManager = null;
    }

    public SDL_Window* GetWindowPtr() => _window;

    public void NewWorld(Context context) =>
        OpenNewEditor(context);

    private IServiceProvider CreateServiceProvider(Context context) =>
        new ServiceCollection()
            .AddSingleton(config)
            .AddSingleton<IAppWindow>(this)
            .AddRoton(Context.Ui, typeof(Program).Assembly)
            .AddRoton(context)
            .AddLyonCommon()
            .BuildServiceProvider();

    private void AddEditorWindow(Editor editor)
    {
        var editorWindow = ActivatorUtilities.CreateInstance<EditorWindow>(
            editor.ServiceProvider!,
            editor,
            _textureManager!
        );

        _components.Add(editorWindow);
    }

    private void OpenNewEditor(Context context)
    {
        var editor = new Editor(CreateServiceProvider);
        editor.Reset(context);
        AddEditorWindow(editor);
    }

    private void OpenWorldEditor(Stream stream)
    {
        var editor = new Editor(CreateServiceProvider);
        editor.Load(stream, (int)stream.Length);
        AddEditorWindow(editor);
    }

    public void OpenWorld(string file)
    {
        try
        {
            OpenWorldEditor(File.OpenRead(file));
        }
        catch
        {
            // simply don't load on failure
        }
    }
}