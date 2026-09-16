using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SDL;

// ReSharper disable once CheckNamespace

namespace ImGuiNET.SDL3;

/// <summary>
/// Represents errors that occur during ImGui functions.
/// </summary>
/// <param name="message">
/// Message that describes the error.
/// </param>
/// <param name="data">
/// Additional data associated with the error.
/// </param>
/// <param name="innerException">
/// The exception that caused the current exception.
/// </param>
public class ImGuiBackendException(
    string message,
    Dictionary<string, object?>? data = null,
    Exception? innerException = null) : Exception(message, innerException)
{
    public override IDictionary Data { get; } = data ?? [];

    [DoesNotReturn]
    private static void Throw(
        string message,
        Dictionary<string, object?>? data = null,
        Exception? innerException = null) =>
        throw new ImGuiBackendException(
            message: message,
            data: data,
            innerException: innerException);

    [DoesNotReturn]
    internal static void ThrowAlreadyInitialized(ImGuiContextPtr ctx) =>
        Throw(
            message: "This context is already initialized with a different window or renderer.",
            data: new Dictionary<string, object?>
            {
                ["Context"] = ctx
            }
        );

    [DoesNotReturn]
    internal static void ThrowCreateTextureFailed(ImGuiContextPtr ctx, ImTextureDataPtr texData) =>
        Throw(
            message: "Failed to create the texture.",
            innerException: new Exception(SDL_GetError()),
            data: new Dictionary<string, object?>
            {
                ["Context"] = ctx,
                ["TextureData"] = texData
            }
        );

    [DoesNotReturn]
    internal static void ThrowUpdateTextureFailed(ImGuiContextPtr ctx, ImTextureDataPtr texData, IntPtr sdlTexture) =>
        Throw(
            message: "Failed to update the texture.",
            innerException: new Exception(SDL_GetError()),
            data: new Dictionary<string, object?>
            {
                ["Context"] = ctx,
                ["TextureData"] = texData,
                ["SdlTexture"] = sdlTexture
            }
        );

    [DoesNotReturn]
    internal static void ThrowBlendModeFailed(ImGuiContextPtr ctx, IntPtr texture) =>
        Throw(
            message: "Failed to set the blend mode for the ImGui texture atlas.",
            innerException: new Exception(SDL_GetError()),
            data: new Dictionary<string, object?>
            {
                ["Context"] = ctx,
                ["Texture"] = texture
            }
        );

    [DoesNotReturn]
    internal static void ThrowScaleModeFailed(ImGuiContextPtr ctx, IntPtr texture) =>
        Throw(
            message: "Failed to set the scale mode for the ImGui texture atlas.",
            innerException: new Exception(SDL_GetError()),
            data: new Dictionary<string, object?>
            {
                ["Context"] = ctx,
                ["Texture"] = texture
            }
        );

    [DoesNotReturn]
    internal static void ThrowContextNotInitialized(ImGuiContextPtr ctx) =>
        Throw(
            message: "The SDL3 backend was not initialized for the current ImGui context.",
            innerException: new Exception(SDL_GetError()),
            data: new Dictionary<string, object?>
            {
                ["Context"] = ctx
            }
        );

    [DoesNotReturn]
    public static void ThrowStopTextInputFailed(ImGuiContextPtr ctx)
    {
        Throw(
            message: "Failed to stop IME text input.",
            innerException: new Exception(SDL_GetError()),
            data: new Dictionary<string, object?>
            {
                ["Context"] = ctx
            }
        );
    }

    [DoesNotReturn]
    public static void ThrowSetTextInputAreaFailed(ImGuiContextPtr ctx)
    {
        Throw(
            message: "Failed to set the input text area for IME text input.",
            innerException: new Exception(SDL_GetError()),
            data: new Dictionary<string, object?>
            {
                ["Context"] = ctx
            }
        );
    }

    [DoesNotReturn]
    public static void ThrowStartTextInputFailed(ImGuiContextPtr ctx)
    {
        Throw(
            message: "Failed to start IME text input.",
            innerException: new Exception(SDL_GetError()),
            data: new Dictionary<string, object?>
            {
                ["Context"] = ctx
            }
        );
    }

    [DoesNotReturn]
    public static void ThrowCreateSystemCursorFailed(
        ImGuiContextPtr ctx,
        ImGuiMouseCursor imGuiCursor,
        SDL_SystemCursor sdlCursor)
    {
        Throw(
            message: "Failed to create a system cursor.",
            innerException: new Exception(SDL_GetError()),
            data: new Dictionary<string, object?>
            {
                ["Context"] = ctx,
                ["ImGuiMouseCursor"] = imGuiCursor,
                ["SdlMouseCursor"] = sdlCursor
            }
        );
    }

    [DoesNotReturn]
    public static void ThrowInvalidTextureFormat(
        ImGuiContextPtr ctx,
        ImTextureDataPtr tex)
    {
        Throw(
            message: "Invalid texture format.",
            data: new Dictionary<string, object?>
            {
                ["Context"] = ctx,
                ["ImTextureData"] = tex
            }
        );
    }

    public static void ThrowTextureAlreadyCreated(
        ImGuiContextPtr ctx,
        ImTextureDataPtr tex)
    {
        Throw(
            message: "Texture already created.",
            data: new Dictionary<string, object?>
            {
                ["Context"] = ctx,
                ["ImTextureData"] = tex
            }
        );
    }
}