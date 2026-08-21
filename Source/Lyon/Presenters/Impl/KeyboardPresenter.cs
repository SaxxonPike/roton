using System.Collections.Generic;
using Roton;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Lyon.Presenters.Impl;

/// <inheritdoc />
[Context(Context.Startup)]
// ReSharper disable once UnusedMember.Global
public sealed class KeyboardPresenter : Keyboard, IKeyboardPresenter
{
    /// <summary>
    /// Maps SDL key codes to Roton key codes.
    /// </summary>
    private static readonly IDictionary<SDL_Keycode, AnsiKey> Map = new Dictionary<SDL_Keycode, AnsiKey>
    {
        { SDL_Keycode.SDLK_A, AnsiKey.A },
        { SDL_Keycode.SDLK_B, AnsiKey.B },
        { SDL_Keycode.SDLK_C, AnsiKey.C },
        { SDL_Keycode.SDLK_D, AnsiKey.D },
        { SDL_Keycode.SDLK_E, AnsiKey.E },
        { SDL_Keycode.SDLK_F, AnsiKey.F },
        { SDL_Keycode.SDLK_G, AnsiKey.G },
        { SDL_Keycode.SDLK_H, AnsiKey.H },
        { SDL_Keycode.SDLK_I, AnsiKey.I },
        { SDL_Keycode.SDLK_J, AnsiKey.J },
        { SDL_Keycode.SDLK_K, AnsiKey.K },
        { SDL_Keycode.SDLK_L, AnsiKey.L },
        { SDL_Keycode.SDLK_M, AnsiKey.M },
        { SDL_Keycode.SDLK_N, AnsiKey.N },
        { SDL_Keycode.SDLK_O, AnsiKey.O },
        { SDL_Keycode.SDLK_P, AnsiKey.P },
        { SDL_Keycode.SDLK_Q, AnsiKey.Q },
        { SDL_Keycode.SDLK_R, AnsiKey.R },
        { SDL_Keycode.SDLK_S, AnsiKey.S },
        { SDL_Keycode.SDLK_T, AnsiKey.T },
        { SDL_Keycode.SDLK_U, AnsiKey.U },
        { SDL_Keycode.SDLK_V, AnsiKey.V },
        { SDL_Keycode.SDLK_W, AnsiKey.W },
        { SDL_Keycode.SDLK_X, AnsiKey.X },
        { SDL_Keycode.SDLK_Y, AnsiKey.Y },
        { SDL_Keycode.SDLK_Z, AnsiKey.Z },
        { SDL_Keycode.SDLK_APOSTROPHE, AnsiKey.Apostophe },
        { SDL_Keycode.SDLK_BACKSLASH, AnsiKey.Backslash },
        { SDL_Keycode.SDLK_BACKSPACE, AnsiKey.Backspace },
        { SDL_Keycode.SDLK_COMMA, AnsiKey.Comma },
        { SDL_Keycode.SDLK_0, AnsiKey.D0 },
        { SDL_Keycode.SDLK_1, AnsiKey.D1 },
        { SDL_Keycode.SDLK_2, AnsiKey.D2 },
        { SDL_Keycode.SDLK_3, AnsiKey.D3 },
        { SDL_Keycode.SDLK_4, AnsiKey.D4 },
        { SDL_Keycode.SDLK_5, AnsiKey.D5 },
        { SDL_Keycode.SDLK_6, AnsiKey.D6 },
        { SDL_Keycode.SDLK_7, AnsiKey.D7 },
        { SDL_Keycode.SDLK_8, AnsiKey.D8 },
        { SDL_Keycode.SDLK_9, AnsiKey.D9 },
        { SDL_Keycode.SDLK_DELETE, AnsiKey.Delete },
        { SDL_Keycode.SDLK_DOWN, AnsiKey.Down },
        { SDL_Keycode.SDLK_END, AnsiKey.End },
        { SDL_Keycode.SDLK_RETURN, AnsiKey.Enter },
        { SDL_Keycode.SDLK_PLUS, AnsiKey.Equals },
        { SDL_Keycode.SDLK_ESCAPE, AnsiKey.Escape },
        { SDL_Keycode.SDLK_F1, AnsiKey.F1 },
        { SDL_Keycode.SDLK_F2, AnsiKey.F2 },
        { SDL_Keycode.SDLK_F3, AnsiKey.F3 },
        { SDL_Keycode.SDLK_F4, AnsiKey.F4 },
        { SDL_Keycode.SDLK_F5, AnsiKey.F5 },
        { SDL_Keycode.SDLK_F6, AnsiKey.F6 },
        { SDL_Keycode.SDLK_F7, AnsiKey.F7 },
        { SDL_Keycode.SDLK_F8, AnsiKey.F8 },
        { SDL_Keycode.SDLK_F9, AnsiKey.F9 },
        { SDL_Keycode.SDLK_F10, AnsiKey.F10 },
        { SDL_Keycode.SDLK_F11, AnsiKey.F11 },
        { SDL_Keycode.SDLK_F12, AnsiKey.F12 },
        { SDL_Keycode.SDLK_GRAVE, AnsiKey.Grave },
        { SDL_Keycode.SDLK_HOME, AnsiKey.Home },
        { SDL_Keycode.SDLK_INSERT, AnsiKey.Insert },
        { SDL_Keycode.SDLK_LEFT, AnsiKey.Left },
        { SDL_Keycode.SDLK_LEFTBRACKET, AnsiKey.LeftSquare },
        { SDL_Keycode.SDLK_MINUS, AnsiKey.Minus },
        { SDL_Keycode.SDLK_KP_0, AnsiKey.D0 },
        { SDL_Keycode.SDLK_KP_1, AnsiKey.D1 },
        { SDL_Keycode.SDLK_KP_2, AnsiKey.D2 },
        { SDL_Keycode.SDLK_KP_3, AnsiKey.D3 },
        { SDL_Keycode.SDLK_KP_4, AnsiKey.D4 },
        { SDL_Keycode.SDLK_KP_5, AnsiKey.D5 },
        { SDL_Keycode.SDLK_KP_6, AnsiKey.D6 },
        { SDL_Keycode.SDLK_KP_7, AnsiKey.D7 },
        { SDL_Keycode.SDLK_KP_8, AnsiKey.D8 },
        { SDL_Keycode.SDLK_KP_9, AnsiKey.D9 },
        { SDL_Keycode.SDLK_KP_ENTER, AnsiKey.NumEnter },
        { SDL_Keycode.SDLK_KP_MINUS, AnsiKey.NumMinus },
        { SDL_Keycode.SDLK_KP_PERIOD, AnsiKey.NumPeriod },
        { SDL_Keycode.SDLK_KP_PLUS, AnsiKey.NumPlus },
        { SDL_Keycode.SDLK_KP_DIVIDE, AnsiKey.NumSlash },
        { SDL_Keycode.SDLK_KP_MULTIPLY, AnsiKey.NumStar },
        { SDL_Keycode.SDLK_PAGEDOWN, AnsiKey.PageDown },
        { SDL_Keycode.SDLK_PAGEUP, AnsiKey.PageUp },
        { SDL_Keycode.SDLK_PAUSE, AnsiKey.Pause },
        { SDL_Keycode.SDLK_PERIOD, AnsiKey.Period },
        { SDL_Keycode.SDLK_PRINTSCREEN, AnsiKey.PrintScreen },
        { SDL_Keycode.SDLK_RIGHT, AnsiKey.Right },
        { SDL_Keycode.SDLK_RIGHTBRACKET, AnsiKey.RightSquare },
        { SDL_Keycode.SDLK_SEMICOLON, AnsiKey.Semicolon },
        { SDL_Keycode.SDLK_SLASH, AnsiKey.Slash },
        { SDL_Keycode.SDLK_SPACE, AnsiKey.Space },
        { SDL_Keycode.SDLK_TAB, AnsiKey.Tab },
        { SDL_Keycode.SDLK_UP, AnsiKey.Up },
        { SDL_Keycode.SDLK_QUESTION, AnsiKey.Slash },
        { SDL_Keycode.SDLK_EQUALS, AnsiKey.Equals }
    };

    /// <summary>
    /// Converts SDL key modifiers to Roton key modifiers.
    /// </summary>
    private static KeyMod ConvertKeyMod(SDL_Keymod mod) =>
        (mod.HasFlag(SDL_Keymod.SDL_KMOD_LSHIFT) |
         mod.HasFlag(SDL_Keymod.SDL_KMOD_RSHIFT)
            ? KeyMod.Shift
            : 0) |
        (mod.HasFlag(SDL_Keymod.SDL_KMOD_LCTRL) |
         mod.HasFlag(SDL_Keymod.SDL_KMOD_RCTRL)
            ? KeyMod.Control
            : 0) |
        (mod.HasFlag(SDL_Keymod.SDL_KMOD_LALT) |
         mod.HasFlag(SDL_Keymod.SDL_KMOD_RALT)
            ? KeyMod.Alt
            : 0);

    /// <summary>
    /// Update key modifier state.
    /// </summary>
    private KeyMod UpdateMod(SDL_Keymod mod)
    {
        var newMod = ConvertKeyMod(mod);
        SetMod(newMod);
        return newMod;
    }

    /// <inheritdoc />
    public void Press(SDL_Keycode key, SDL_Keymod mod)
    {
        var newMod = UpdateMod(mod);

        if (key == 0)
            return;

        // Don't process command/Windows key shortcuts.
        if ((mod & SDL_Keymod.SDL_KMOD_GUI) != 0)
            return;

        if (!Map.TryGetValue(key, out var value))
            return;

        Enqueue(new KeyPress
        (
            key: value,
            mod: newMod
        ));
    }

    /// <inheritdoc />
    public void Release(SDL_Keycode key, SDL_Keymod mod)
    {
        UpdateMod(mod);
    }
}