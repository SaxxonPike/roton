using System.Collections.Generic;
using DotSDL.Events;
using DotSDL.Input.Keyboard;
using Roton.Emulation.Core.Impl;
using Keyboard = Roton.Emulation.Core.Impl.Keyboard;

namespace Lyon.Presenters.Impl
{
    public class KeyboardPresenter : Keyboard, IKeyboardPresenter
    {
        private static readonly IDictionary<Keycode, AnsiKey> Map = new Dictionary<Keycode, AnsiKey>
        {
            {Keycode.A, AnsiKey.A},
            {Keycode.B, AnsiKey.B},
            {Keycode.C, AnsiKey.C},
            {Keycode.D, AnsiKey.D},
            {Keycode.E, AnsiKey.E},
            {Keycode.F, AnsiKey.F},
            {Keycode.G, AnsiKey.G},
            {Keycode.H, AnsiKey.H},
            {Keycode.I, AnsiKey.I},
            {Keycode.J, AnsiKey.J},
            {Keycode.K, AnsiKey.K},
            {Keycode.L, AnsiKey.L},
            {Keycode.M, AnsiKey.M},
            {Keycode.N, AnsiKey.N},
            {Keycode.O, AnsiKey.O},
            {Keycode.P, AnsiKey.P},
            {Keycode.Q, AnsiKey.Q},
            {Keycode.R, AnsiKey.R},
            {Keycode.S, AnsiKey.S},
            {Keycode.T, AnsiKey.T},
            {Keycode.U, AnsiKey.U},
            {Keycode.V, AnsiKey.V},
            {Keycode.W, AnsiKey.W},
            {Keycode.X, AnsiKey.X},
            {Keycode.Y, AnsiKey.Y},
            {Keycode.Z, AnsiKey.Z},
            {Keycode.Quote, AnsiKey.Apostophe},
            {Keycode.Backslash, AnsiKey.Backslash},
            {Keycode.Backspace, AnsiKey.Backspace},
            {Keycode.Comma, AnsiKey.Comma},
            {Keycode.Num0, AnsiKey.D0},
            {Keycode.Num1, AnsiKey.D1},
            {Keycode.Num2, AnsiKey.D2},
            {Keycode.Num3, AnsiKey.D3},
            {Keycode.Num4, AnsiKey.D4},
            {Keycode.Num5, AnsiKey.D5},
            {Keycode.Num6, AnsiKey.D6},
            {Keycode.Num7, AnsiKey.D7},
            {Keycode.Num8, AnsiKey.D8},
            {Keycode.Num9, AnsiKey.D9},
            {Keycode.Delete, AnsiKey.Delete},
            {Keycode.Down, AnsiKey.Down},
            {Keycode.End, AnsiKey.End},
            {Keycode.Return, AnsiKey.Enter},
            {Keycode.Plus, AnsiKey.Equals},
            {Keycode.Escape, AnsiKey.Escape},
            {Keycode.F1, AnsiKey.F1},
            {Keycode.F2, AnsiKey.F2},
            {Keycode.F3, AnsiKey.F3},
            {Keycode.F4, AnsiKey.F4},
            {Keycode.F5, AnsiKey.F5},
            {Keycode.F6, AnsiKey.F6},
            {Keycode.F7, AnsiKey.F7},
            {Keycode.F8, AnsiKey.F8},
            {Keycode.F9, AnsiKey.F9},
            {Keycode.F10, AnsiKey.F10},
            {Keycode.F11, AnsiKey.F11},
            {Keycode.F12, AnsiKey.F12},
            {Keycode.Backquote, AnsiKey.Grave},
            {Keycode.Home, AnsiKey.Home},
            {Keycode.Insert, AnsiKey.Insert},
            {Keycode.Left, AnsiKey.Left},
            {Keycode.LeftBracket, AnsiKey.LeftSquare},
            {Keycode.Minus, AnsiKey.Minus},
            {Keycode.NumPad0, AnsiKey.Num0},
            {Keycode.NumPad1, AnsiKey.Num1},
            {Keycode.NumPad2, AnsiKey.Num2},
            {Keycode.NumPad3, AnsiKey.Num3},
            {Keycode.NumPad4, AnsiKey.Num4},
            {Keycode.NumPad5, AnsiKey.Num5},
            {Keycode.NumPad6, AnsiKey.Num6},
            {Keycode.NumPad7, AnsiKey.Num7},
            {Keycode.NumPad8, AnsiKey.Num8},
            {Keycode.NumPad9, AnsiKey.Num9},
            {Keycode.NumPadEnter, AnsiKey.NumEnter},
            {Keycode.NumPadMinus, AnsiKey.NumMinus},
            {Keycode.NumPadPeriod, AnsiKey.NumPeriod},
            {Keycode.NumPadPlus, AnsiKey.NumPlus},
            {Keycode.NumPadDivide, AnsiKey.NumSlash},
            {Keycode.NumPadMuliply, AnsiKey.NumStar},
            {Keycode.PageDown, AnsiKey.PageDown},
            {Keycode.PageUp, AnsiKey.PageUp},
            {Keycode.Pause, AnsiKey.Pause},
            {Keycode.Period, AnsiKey.Period},
            {Keycode.PrintScreen, AnsiKey.PrintScreen},
            {Keycode.Right, AnsiKey.Right},
            {Keycode.RightBracket, AnsiKey.RightSquare},
            {Keycode.Semicolon, AnsiKey.Semicolon},
            {Keycode.Slash, AnsiKey.Slash},
            {Keycode.Space, AnsiKey.Space},
            {Keycode.Tab, AnsiKey.Tab},
            {Keycode.Up, AnsiKey.Up}
        };

        public bool Press(KeyboardEvent data)
        {
            if (!Map.ContainsKey(data.Keycode))
                return false;
            
            Enqueue(new KeyPress
            {
                Key = Map[data.Keycode],
                Shift = data.Keymod == Keymod.Shift,
                Control = data.Keymod == Keymod.Ctrl,
                Alt = data.Keymod == Keymod.Alt
            });

            return true;
        }
    }
}