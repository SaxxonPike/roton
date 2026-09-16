using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Intrinsics;
using ImGuiNET.SDL3;
using static Hexa.NET.ImGui.ImGui;

namespace Torch.Gui.Impl;

/// <inheritdoc />
internal sealed class ImGuiBackend : IImGuiBackend
{
    /// <inheritdoc />
    public ImGuiContextPtr Context { get; }

    /// <summary>
    /// Function definition for a callback to set clipboard data.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalUsing(typeof(Utf8StringMarshaller))]
    private unsafe delegate string? ImGuiGetClipboardCallback(ImGuiContext* ctx);

    /// <summary>
    /// Function definition for a callback to set clipboard data.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private unsafe delegate void ImGuiSetClipboardCallback(
        ImGuiContext* ctx,
        [MarshalUsing(typeof(Utf8StringMarshaller))]
        string? text
    );

    /// <summary>
    /// Function definition for a callback that can be specified instead of
    /// the standard rendering pipeline.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private unsafe delegate void ImGuiUserCallback(
        ImDrawList* cmdList,
        ImDrawCmd* drawCmd
    );

    /// <summary>
    /// Function definition for a callback for when an input interface is
    /// activated (such as virtual keyboards, etc.)
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private unsafe delegate void ImGuiPlatformSetImeDataCallback(
        ImGuiContext* ctx,
        ImGuiViewport* viewport,
        ImGuiPlatformImeData* data
    );

    /// <summary>
    /// Number of mouse cursors that ImGui may use.
    /// </summary>
    private const int CursorCount = (int)ImGuiMouseCursor.Count;

    /// <summary>
    /// Stores data for all contexts for which this backend has been
    /// initialized.
    /// </summary>
    private static readonly Dictionary<ImGuiContextPtr, ImGuiBackend> Contexts = [];

    private readonly unsafe SDL_Cursor*[] _cursors = new SDL_Cursor*[CursorCount];

    private ulong _lastTime;
    private readonly unsafe SDL_Renderer* _renderer;
    private readonly IImGuiKeyMapper _keyMapper;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly ImGuiPlatformSetImeDataCallback? _platformSetImeDataCallback;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly ImGuiGetClipboardCallback? _getClipboardCallback;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly ImGuiSetClipboardCallback? _setClipboardCallback;
    private readonly unsafe SDL_Window* _window;
    private unsafe SDL_Window* _imeWindow;
    private bool _begunFrame;

    /// <summary>
    /// This hack is used to swap horizontal scrolling on OSX, which seems to
    /// behave opposite what is expected.
    /// </summary>
    private static readonly float HorizontalScrollFactor =
        RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? -1f : 1f;

    /// <summary>
    /// Creates an instance of <see cref="ImGuiBackend"/>.
    /// </summary>
    /// <param name="ctx">
    /// ImGui context that the backend will use.
    /// </param>
    /// <param name="windowPtr">
    /// SDL window pointer.
    /// </param>
    /// <param name="rendererPtr">
    /// SDL renderer pointer.
    /// </param>
    /// <param name="keyMapper">
    /// Key mapper service.
    /// </param>
    public unsafe ImGuiBackend(
        ImGuiContextPtr ctx,
        IntPtr windowPtr,
        IntPtr rendererPtr,
        IImGuiKeyMapper keyMapper)
    {
        _keyMapper = keyMapper;

        Context = ctx;
        SetCurrentContext(ctx);

        //
        // Check to see if this context has been initialized with the same
        // properties. If so, nothing needs to be done.
        //

        if (Contexts.TryGetValue(ctx, out var ctxData) &&
            ((IntPtr)ctxData._window != windowPtr || (IntPtr)ctxData._renderer != rendererPtr))
            ImGuiBackendException.ThrowAlreadyInitialized(ctx);

        if (ctxData != null)
            return;

        //
        // Set up backend IO flags. These tell ImGui what is supported by this
        // backend.
        //

        var io = GetIO();

        io.BackendFlags = ImGuiBackendFlags.RendererHasVtxOffset |
                          ImGuiBackendFlags.HasMouseCursors |
                          ImGuiBackendFlags.HasSetMousePos |
                          ImGuiBackendFlags.RendererHasTextures;

        if (SDL_WasInit(SDL_InitFlags.SDL_INIT_GAMEPAD) == SDL_InitFlags.SDL_INIT_GAMEPAD)
            io.BackendFlags |= ImGuiBackendFlags.HasGamepad;

        var vp = GetMainViewport();
        vp.PlatformHandle = (SDL_Window*)windowPtr;

        //
        // Set up the default font. This is so that there will always be
        // at least one font available.
        //

        io.Fonts.AddFontDefault();

        //
        // Configure the context for rendering using this backend.
        //

        Contexts[ctx] = this;
        _renderer = (SDL_Renderer*)rendererPtr;
        _keyMapper = keyMapper;
        _window = (SDL_Window*)windowPtr;
        _lastTime = SDL_GetTicksNS();
        _platformSetImeDataCallback = PlatformSetImeData;
        _getClipboardCallback = GetClipboard;
        _setClipboardCallback = SetClipboard;

        //
        // Configure callbacks. These references must be kept so that the
        // GC does not free them.
        //

        var pio = GetPlatformIO();

        pio.PlatformSetImeDataFn = (void*)Marshal.GetFunctionPointerForDelegate(
            _platformSetImeDataCallback!
        );

        pio.PlatformGetClipboardTextFn = (void*)Marshal.GetFunctionPointerForDelegate(
            _getClipboardCallback!
        );

        pio.PlatformSetClipboardTextFn = (void*)Marshal.GetFunctionPointerForDelegate(
            _setClipboardCallback!
        );
    }

    /// <summary>
    /// Free resources used by this backend for the current ImGui context.
    /// </summary>
    public void Dispose()
    {
        //
        // If this context hasn't been initialized, nothing to do.
        //

        if (!Contexts.Remove(Context))
            return;

        //
        // Clean up the atlas texture.
        //

        DestroyDeviceObjects();
    }

    /// <summary>
    /// Set up the current ImGui context for a new frame.
    /// </summary>
    public unsafe void NewFrame()
    {
        //
        // If NewFrame was already called, do nothing.
        //

        if (_begunFrame)
            return;

        //
        // Determine the amount of time that has elapsed since the last frame.
        //

        SetCurrentContext(Context);
        var io = GetIO();
        var now = SDL_GetTicksNS();
        var elapsed = now - _lastTime;

        io.DeltaTime = (float)TimeSpan
            .FromTicks(unchecked((long)(elapsed / TimeSpan.NanosecondsPerTick)))
            .TotalSeconds;

        //
        // Determine the render size and scale for this frame.
        //

        int width, height;
        SDL_Rect logicalRect;

        if (SDL_GetRenderViewport(_renderer, &logicalRect))
            (width, height) = (logicalRect.w, logicalRect.h);
        else if (!SDL_GetRenderOutputSize(_renderer, &width, &height))
            (width, height) = (0, 0);

        float scaleX, scaleY;
        if (!SDL_GetRenderScale(_renderer, &scaleX, &scaleY))
            (scaleX, scaleY) = (1, 1);

        io.DisplaySize = new Vector2(width, height);
        io.DisplayFramebufferScale = new Vector2(scaleX, scaleY);

        //
        // Update input for this frame.
        //

        if (io.WantSetMousePos)
            SDL_WarpMouseInWindow(_window, io.MousePos.X, io.MousePos.Y);

        if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) == 0)
            SetMouseCursor(GetMouseCursor());

        //
        // Mark the current frame in progress.
        //

        _begunFrame = true;
        _lastTime = now;
    }

    /// <summary>
    /// Render an ImGui draw data structure to SDL.
    /// </summary>
    /// <param name="drawData">
    /// ImGui draw data for the frame to be rendered.
    /// </param>
    public unsafe void RenderDrawData(ImDrawDataPtr drawData)
    {
        //
        // If RenderDrawData was already called, do nothing. (short-circuit)
        //

        if (!_begunFrame)
            return;

        //
        // Indicate the end of this frame.
        //

        _begunFrame = false;

        //
        // Update textures as necessary.
        //

        // if (!Unsafe.IsNullRef(ref drawData.Textures))
        {
            for (var i = 0; i < drawData.Textures.Size; i++)
            {
                var tex = drawData.Textures[i];
                if (tex.Status != ImTextureStatus.Ok)
                    UpdateTexture(tex);
            }
        }

        //
        // Preserve viewport and render clip settings on the SDL render
        // surface.
        //

        var hasViewport = SDL_RenderViewportSet(_renderer);
        var hasClipRect = SDL_RenderClipEnabled(_renderer);
        var oldViewport = default(SDL_Rect);
        var oldClipRect = default(SDL_Rect);

        if (hasViewport)
        {
            if (!SDL_GetRenderViewport(_renderer, &oldViewport))
                Debug.WriteLine("Failed to retrieve render viewport: {0}",
                    [SDL_GetError()]);
        }

        if (hasClipRect)
        {
            if (!SDL_GetRenderClipRect(_renderer, &oldClipRect))
                Debug.WriteLine("Failed to retrieve render clip rectangle: {0}",
                    [SDL_GetError()]);
        }

        //
        // Render the draw list.
        //

        Render(drawData);

        //
        // Restore preserved render state.
        //

        if (hasViewport)
        {
            if (!SDL_SetRenderViewport(_renderer, &oldViewport))
                Debug.WriteLine("Failed to set render viewport after render: {0}",
                    [SDL_GetError()]);
        }
        else
        {
            if (!SDL_SetRenderViewport(_renderer, null))
                Debug.WriteLine("Failed to set render viewport after render: {0}",
                    [SDL_GetError()]);
        }

        if (hasClipRect)
        {
            if (!SDL_SetRenderClipRect(_renderer, &oldClipRect))
                Debug.WriteLine("Failed to set clip rectangle after render: {0}",
                    [SDL_GetError()]);
        }
        else
        {
            if (!SDL_SetRenderClipRect(_renderer, null))
                Debug.WriteLine("Failed to set clip rectangle after render: {0}",
                    [SDL_GetError()]);
        }
    }

    private unsafe void UpdateTexture(ImTextureDataPtr tex)
    {
        SetCurrentContext(Context);

        switch (tex.Status)
        {
            case ImTextureStatus.WantCreate:
            {
                if (!tex.TexID.IsNull || tex.BackendUserData != null)
                    ImGuiBackendException.ThrowTextureAlreadyCreated(Context, tex);
                if (tex.Format != ImTextureFormat.Rgba32)
                    ImGuiBackendException.ThrowInvalidTextureFormat(Context, tex);

                var sdlTex = SDL_CreateTexture(
                    _renderer,
                    SDL_PIXELFORMAT_RGBA32,
                    SDL_TextureAccess.SDL_TEXTUREACCESS_STATIC,
                    tex.Width,
                    tex.Height
                );

                if (sdlTex == null)
                    ImGuiBackendException.ThrowCreateTextureFailed(Context, tex);

                var updateSuccess = SDL_UpdateTexture(
                    sdlTex,
                    null,
                    (IntPtr)tex.GetPixels(),
                    tex.GetPitch()
                );

                if (!updateSuccess)
                    ImGuiBackendException.ThrowUpdateTextureFailed(Context, tex, (IntPtr)sdlTex);

                if (!SDL_SetTextureBlendMode(sdlTex, SDL_BlendMode.SDL_BLENDMODE_BLEND))
                    ImGuiBackendException.ThrowBlendModeFailed(Context, (IntPtr)sdlTex);

                if (!SDL_SetTextureScaleMode(sdlTex, SDL_ScaleMode.SDL_SCALEMODE_LINEAR))
                    ImGuiBackendException.ThrowScaleModeFailed(Context, (IntPtr)sdlTex);

                tex.SetTexID(new ImTextureID(sdlTex));
                tex.SetStatus(ImTextureStatus.Ok);
                break;
            }
            case ImTextureStatus.WantUpdates:
            {
                var sdlTex = (SDL_Texture*)tex.TexID.Handle;

                for (var i = 0; i < tex.Updates.Size; i++)
                {
                    var rect = tex.Updates[i];
                    var sdlRect = new SDL_Rect
                    {
                        x = rect.X,
                        y = rect.Y,
                        w = rect.W,
                        h = rect.H
                    };

                    SDL_UpdateTexture(
                        sdlTex,
                        &sdlRect,
                        (IntPtr)tex.GetPixelsAt(rect.X, rect.Y),
                        tex.GetPitch()
                    );
                }

                tex.SetStatus(ImTextureStatus.Ok);
                break;
            }
            case ImTextureStatus.WantDestroy:
            {
                var sdlTex = (SDL_Texture*)tex.TexID.Handle;
                if (sdlTex == null)
                    return;
                SDL_DestroyTexture(sdlTex);

                tex.SetTexID(ImTextureID.Null);
                tex.SetStatus(ImTextureStatus.Destroyed);
                break;
            }
        }
    }

    /// <summary>
    /// Clean up allocated objects.
    /// </summary>
    private unsafe void DestroyDeviceObjects()
    {
        var textures = GetPlatformIO().Textures;

        for (var i = 0; i < textures.Size; i++)
        {
            if (textures[i].RefCount != 1)
                continue;

            textures[i].SetStatus(ImTextureStatus.WantDestroy);
            UpdateTexture(textures[i]);
        }

        //
        // Clean up cursors.
        //

        for (var i = 0; i < _cursors.Length; i++)
        {
            if (_cursors[i] == null)
                continue;

            SDL_DestroyCursor(_cursors[i]);
            _cursors[i] = null;
        }
    }

    /// <summary>
    /// Send SDL events to ImGui.
    /// </summary>
    /// <param name="ev">
    /// Event to process.
    /// </param>
    public unsafe void ProcessEvent(SDL_Event* ev)
    {
        //
        // Process the event.
        //

        SetCurrentContext(Context);
        var io = GetIO();

        switch (ev->Type)
        {
            //
            // Skip event IDs out of supported range.
            //

            case < SDL_EventType.SDL_EVENT_FIRST or
                > SDL_EventType.SDL_EVENT_LAST:
            {
                break;
            }

            //
            // Handle when a mouse button has been released.
            //

            case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_UP:
            {
                if (ConvertMouseButtonEvent(ev->button) is not { } button)
                    break;

                io.AddMouseButtonEvent((int)button, false);
                break;
            }

            //
            // Handle when a mouse button has been pressed.
            //

            case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN:
            {
                if (ConvertMouseButtonEvent(ev->button) is not { } button)
                    break;

                io.AddMouseButtonEvent((int)button, true);
                break;
            }

            //
            // Handle when a mouse has changed position.
            //

            case SDL_EventType.SDL_EVENT_MOUSE_MOTION:
            {
                var evCopy = *ev;
                if (!SDL_ConvertEventToRenderCoordinates(_renderer, &evCopy))
                    Debug.WriteLine("Failed to convert event coordinates for mouse movement: {0}",
                        [SDL_GetError()]);

                io.AddMouseSourceEvent(ev->motion.which == SDL_TOUCH_MOUSEID
                    ? ImGuiMouseSource.TouchScreen
                    : ImGuiMouseSource.Mouse);

                io.AddMousePosEvent(evCopy.motion.x, evCopy.motion.y);
                break;
            }

            //
            // Handle when a mouse wheel has changed position.
            //

            case SDL_EventType.SDL_EVENT_MOUSE_WHEEL:
            {
                io.AddMouseSourceEvent(ev->wheel.which == SDL_TOUCH_MOUSEID
                    ? ImGuiMouseSource.TouchScreen
                    : ImGuiMouseSource.Mouse);

                io.AddMouseWheelEvent(ev->wheel.x * HorizontalScrollFactor, ev->wheel.y);
                break;
            }

            //
            // Handle when a keyboard key has been released.
            //

            case SDL_EventType.SDL_EVENT_KEY_UP:
            {
                if (_keyMapper.ConvertKeyboardEventKey(ev->key) is not { } nav ||
                    nav == ImGuiKey.None)
                    break;

                UpdateKeyboardModifiers(ev->key.mod);
                io.SetKeyEventNativeData(nav, (int)ev->key.key, (int)ev->key.scancode);
                io.AddKeyEvent(nav, false);
                break;
            }

            //
            // Handle when a keyboard key has been pressed down.
            //

            case SDL_EventType.SDL_EVENT_KEY_DOWN:
            {
                if (_keyMapper.ConvertKeyboardEventScanCode(ev->key) is not { } nav ||
                    nav == ImGuiKey.None)
                    break;

                UpdateKeyboardModifiers(ev->key.mod);
                io.SetKeyEventNativeData(nav, (int)ev->key.key, (int)ev->key.scancode);
                io.AddKeyEvent(nav, true);
                break;
            }

            //
            // Handle when text has been typed.
            //

            case SDL_EventType.SDL_EVENT_TEXT_INPUT:
            {
                if (ev->text.GetText() is not { } text ||
                    string.IsNullOrEmpty(text))
                    break;

                io.AddInputCharactersUTF8(text);
                break;
            }
        }
    }

    /// <summary>
    /// Require that the backend be initialized with the specified ImGui context.
    /// </summary>
    private static void RequireContextData(ImGuiContextPtr ctx, out ImGuiBackend ctxData)
    {
        if (!Contexts.TryGetValue(ctx, out ctxData!))
            ImGuiBackendException.ThrowContextNotInitialized(ctx);
    }

    /// <summary>
    /// Handles when ImGui wishes to set clipboard data.
    /// </summary>
    private static unsafe void SetClipboard(ImGuiContext* ctx, string? data)
    {
        if (!SDL_SetClipboardText(data))
            Debug.WriteLine("Failed to set clipboard: {0}",
                [SDL_GetError()]);
    }

    /// <summary>
    /// Handles when ImGui wishes to get clipboard data.
    /// </summary>
    private static unsafe string? GetClipboard(ImGuiContext* ctx)
    {
        return SDL_GetClipboardText();
    }

    /// <summary>
    /// Handles when IME data is available for an ImGui context. This is called
    /// directly by ImGui to obtain information about an input mechanism such
    /// as a virtual keyboard, or to interact with such input mechanism.
    /// </summary>
    /// <param name="ctx">
    /// ImGui context associated with the call.
    /// </param>
    /// <param name="viewport">
    /// Viewport within which the data is requested.
    /// </param>
    /// <param name="data">
    /// Event data.
    /// </param>
    private static unsafe void PlatformSetImeData(
        ImGuiContext* ctx,
        ImGuiViewport* viewport,
        ImGuiPlatformImeData* data)
    {
        RequireContextData(new ImGuiContextPtr(ctx), out var self);

        //
        // If the current IME window should close, or the focused window has
        // changed, indicate to SDL to stop accepting input.
        //

        var window = (SDL_Window*)viewport->PlatformHandle;
        if ((data->WantVisible == 0 || self._imeWindow != window) &&
            self._imeWindow != null)
        {
            if (!SDL_StopTextInput(self._imeWindow))
                ImGuiBackendException.ThrowStopTextInputFailed(ctx);
            self._imeWindow = null;
        }

        if (data->WantVisible == 0)
            return;

        //
        // If the IME window should be active, indicate to SDL to accept text
        // input.
        //

        var r = new SDL_Rect
        {
            x = (int)data->InputPos.X,
            y = (int)data->InputPos.Y,
            w = 1,
            h = (int)data->InputLineHeight
        };

        if (!SDL_SetTextInputArea(window, &r, 0))
            ImGuiBackendException.ThrowSetTextInputAreaFailed(ctx);

        if (!SDL_StartTextInput(window))
            ImGuiBackendException.ThrowStartTextInputFailed(ctx);

        self._imeWindow = window;
    }


    /// <summary>
    /// Map SDL mouse button event data to an ImGui mouse button.
    /// </summary>
    /// <param name="ev">
    /// SDL event data.
    /// </param>
    /// <returns>
    /// ImGui mouse button index. Will return null if no mapping exists.
    /// </returns>
    private static ImGuiMouseButton? ConvertMouseButtonEvent(
        SDL_MouseButtonEvent ev) =>
        ev.Button switch
        {
            SDLButton.SDL_BUTTON_LEFT =>
                ImGuiMouseButton.Left,

            SDLButton.SDL_BUTTON_MIDDLE =>
                ImGuiMouseButton.Middle,

            SDLButton.SDL_BUTTON_RIGHT =>
                ImGuiMouseButton.Right,
            _ => null
        };

    /// <summary>
    /// Notify ImGui of the current state of keyboard modifier keys.
    /// </summary>
    /// <param name="mod">
    /// SDL key modifiers.
    /// </param>
    public void UpdateKeyboardModifiers(SDL_Keymod mod)
    {
        var io = GetIO();

        if (_keyMapper.ConvertKeyboardMod(SDL_Keymod.SDL_KMOD_CTRL) is { } ctrlKey)
            io.AddKeyEvent(ctrlKey, (mod & SDL_Keymod.SDL_KMOD_CTRL) != 0);

        if (_keyMapper.ConvertKeyboardMod(SDL_Keymod.SDL_KMOD_ALT) is { } altKey)
            io.AddKeyEvent(altKey, (mod & SDL_Keymod.SDL_KMOD_ALT) != 0);

        if (_keyMapper.ConvertKeyboardMod(SDL_Keymod.SDL_KMOD_SHIFT) is { } shiftKey)
            io.AddKeyEvent(shiftKey, (mod & SDL_Keymod.SDL_KMOD_SHIFT) != 0);

        if (_keyMapper.ConvertKeyboardMod(SDL_Keymod.SDL_KMOD_GUI) is { } guiKey)
            io.AddKeyEvent(guiKey, (mod & SDL_Keymod.SDL_KMOD_GUI) != 0);
    }

    /// <summary>
    /// Map an ImGui mouse cursor ID to an SDL mouse cursor ID.
    /// </summary>
    /// <param name="cursor">
    /// ImGui mouse cursor ID.
    /// </param>
    /// <returns>
    /// SDL mouse cursor ID.
    /// </returns>
    private static SDL_SystemCursor? GetCursorId(ImGuiMouseCursor cursor) =>
        cursor switch
        {
            ImGuiMouseCursor.Arrow =>
                SDL_SystemCursor.SDL_SYSTEM_CURSOR_DEFAULT,

            ImGuiMouseCursor.TextInput =>
                SDL_SystemCursor.SDL_SYSTEM_CURSOR_TEXT,

            ImGuiMouseCursor.ResizeAll =>
                SDL_SystemCursor.SDL_SYSTEM_CURSOR_MOVE,

            ImGuiMouseCursor.ResizeNs =>
                SDL_SystemCursor.SDL_SYSTEM_CURSOR_NS_RESIZE,

            ImGuiMouseCursor.ResizeEw =>
                SDL_SystemCursor.SDL_SYSTEM_CURSOR_EW_RESIZE,

            ImGuiMouseCursor.ResizeNesw =>
                SDL_SystemCursor.SDL_SYSTEM_CURSOR_NESW_RESIZE,

            ImGuiMouseCursor.ResizeNwse =>
                SDL_SystemCursor.SDL_SYSTEM_CURSOR_NWSE_RESIZE,

            ImGuiMouseCursor.Hand =>
                SDL_SystemCursor.SDL_SYSTEM_CURSOR_POINTER,

            ImGuiMouseCursor.Wait =>
                SDL_SystemCursor.SDL_SYSTEM_CURSOR_WAIT,

            ImGuiMouseCursor.Progress =>
                SDL_SystemCursor.SDL_SYSTEM_CURSOR_PROGRESS,

            ImGuiMouseCursor.NotAllowed =>
                SDL_SystemCursor.SDL_SYSTEM_CURSOR_NOT_ALLOWED,

            _ => null
        };

    /// <summary>
    /// Set the visible mouse cursor.
    /// </summary>
    /// <param name="cursor">
    /// ImGui mouse cursor ID.
    /// </param>
    private unsafe void SetMouseCursor(
        ImGuiMouseCursor cursor)
    {
        //
        // If the cursor should be hidden, do that.
        //

        var index = (int)cursor;

        if (cursor == ImGuiMouseCursor.None)
        {
            if (!SDL_HideCursor())
                Debug.WriteLine("Failed to hide cursor: {0}",
                    [SDL_GetError()]);
            return;
        }

        //
        // If we have not cached the specified cursor, create and cache it.
        //

        if (_cursors[index] == null)
        {
            var id = GetCursorId(cursor);
            if (id != null)
            {
                var newCursor = SDL_CreateSystemCursor(id.Value);

                if (newCursor == null)
                    ImGuiBackendException.ThrowCreateSystemCursorFailed(
                        GetCurrentContext(),
                        cursor,
                        id.Value);

                _cursors[index] = newCursor;
            }
        }

        //
        // If there is no mouse cursor for the requested ID, just hide the
        // cursor instead.
        //

        if (_cursors[index] == null)
        {
            if (!SDL_HideCursor())
                Debug.WriteLine("Failed to hide cursor: {0}",
                    [SDL_GetError()]);
            return;
        }

        //
        // Switch the currently shown cursor.
        //

        if (!SDL_ShowCursor())
            Debug.WriteLine("Failed to show cursor: {0}",
                [SDL_GetError()]);

        if (!SDL_SetCursor(_cursors[index]))
            Debug.WriteLine("Failed to set cursor: {0}",
                [SDL_GetError()]);
    }

    /// <summary>
    /// Interpret draw data into geometry rendering calls to SDL.
    /// </summary>
    /// <param name="drawData">
    /// ImGui draw data.
    /// </param>
    private unsafe void Render(ImDrawDataPtr drawData)
    {
        //
        // Determine the window dimensions. DisplayPos is a 2D position that
        // ImGui indicates all rendering shall be relative to. We initialize
        // this as a Vector4 here so that it the arithmetic is trivial later.
        //

        var displayPos = drawData.DisplayPos;

        var clipOff = new Vector4(
            displayPos,
            displayPos.X,
            displayPos.Y);

        //
        // Find the maximum number of vertices to allocate data buffers for.
        // Only vertex color conversion is needed; vertices and indices are
        // directly referenced. This works because those tables are allocated
        // in unmanaged memory and are organized by ImGui.
        //

        var maxNumVertices = 0;
        for (var n = 0; n < drawData.CmdListsCount; n++)
        {
            var numVertices = drawData.CmdLists[n].VtxBuffer.Size;
            if (maxNumVertices < numVertices)
                maxNumVertices = numVertices;
        }

        SDL_FColor[] colors = null!;

        try
        {
            //
            // Element counts can get rather large; previous implementations of
            // this used the stack, but that can lead to overflow with particularly
            // complex UIs. We will use MemoryPool to recycle memory spans.
            //

            colors = ArrayPool<SDL_FColor>.Shared.Rent(maxNumVertices + 1);

            //
            // Process command lists.
            //

            for (var n = 0; n < drawData.CmdListsCount; n++)
            {
                var cmdList = drawData.CmdLists[n];
                var vtxBuffer = cmdList.VtxBuffer;
                var idxBuffer = cmdList.IdxBuffer;

                for (var cmdI = 0; cmdI < cmdList.CmdBuffer.Size; cmdI++)
                {
                    var pcmd = cmdList.CmdBuffer[cmdI];
                    if (pcmd.UserCallback != null)
                    {
                        //
                        // If a user callback is specified, call it instead of
                        // the standard render pipeline below.
                        //

                        Marshal
                            .GetDelegateForFunctionPointer<ImGuiUserCallback>((IntPtr)pcmd.UserCallback)
                            .Invoke(cmdList.Handle, &pcmd);
                    }
                    else
                    {
                        //
                        // Determine the clip rectangle.
                        //

                        var bounds = pcmd.ClipRect - clipOff;

                        if (bounds.Z <= bounds.X || bounds.W <= bounds.Y)
                            continue;

                        //
                        // SDL clip rectangles do not use floats; these values
                        // must be converted here.
                        //

                        SDL_Rect clip = default;

                        Vector128.ConvertToInt32Native(
                            bounds.AsVector128() - Vector128.Create(0, 0, bounds.X, bounds.Y)
                        ).StoreUnsafe(ref clip.x);

                        //
                        // If the clip rect can't be set, skip rendering.
                        //

                        if (!SDL_SetRenderClipRect(_renderer, &clip))
                            continue;

                        //
                        // Retrieve render properties from the command.
                        //

                        var sdlTexture = (SDL_Texture*)pcmd.GetTexID();
                        var idxBufferPtr = idxBuffer.Data + pcmd.IdxOffset;
                        var vtxBufferPtr = vtxBuffer.Data + pcmd.VtxOffset;
                        var colMask = Vector128.Create(0xFFU, 0xFFU << 8, 0xFFU << 16, 0xFFU << 24);
                        var colDiv = Vector128.Create(0x1U, 0x1U << 8, 0x1U << 16, 0x1U << 24);

                        fixed (SDL_FColor* colorPtr = colors)
                        {
                            //
                            // Convert vertex color data from RGBA32 to SDL_FColor.
                            //

                            for (var i = 0; i < vtxBuffer.Size; i++)
                            {
                                (Vector128.ConvertToSingle(
                                    (Vector128.Create(vtxBufferPtr[i].Col) & colMask) / colDiv
                                ) / 255f).Store((float*)&colorPtr[i]);
                            }

                            //
                            // Perform the render. Vertex and index data is used
                            // directly from the source as it needs no conversion.
                            //

                            if (!SDL_RenderGeometryRaw(
                                    _renderer,
                                    sdlTexture,
                                    (float*)&vtxBufferPtr->Pos,
                                    sizeof(ImDrawVert),
                                    colorPtr,
                                    sizeof(SDL_FColor),
                                    (float*)&vtxBufferPtr->Uv,
                                    sizeof(ImDrawVert),
                                    (int)(vtxBuffer.Size - pcmd.VtxOffset),
                                    (IntPtr)idxBufferPtr,
                                    (int)pcmd.ElemCount,
                                    sizeof(ushort)))
                                Debug.WriteLine("Failed to render geometry: {0}",
                                    [SDL_GetError()]);
                        }
                    }
                }
            }
        }
        finally
        {
            ArrayPool<SDL_FColor>.Shared.Return(colors);
        }
    }
}