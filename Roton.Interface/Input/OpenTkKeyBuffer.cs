using System.Collections.Generic;
using OpenTK.Input;
using Roton.Emulation.Core.Impl;
using Keyboard = Roton.Emulation.Core.Impl.Keyboard;

namespace Roton.Interface.Input
{
    public class OpenTkKeyBuffer : Keyboard, IOpenTkKeyBuffer
    {
        private static readonly IDictionary<Key, AnsiKey> Map = new Dictionary<Key, AnsiKey>
        {
            {Key.A, AnsiKey.A},
            {Key.B, AnsiKey.B},
            {Key.C, AnsiKey.C},
            {Key.D, AnsiKey.D},
            {Key.E, AnsiKey.E},
            {Key.F, AnsiKey.F},
            {Key.G, AnsiKey.G},
            {Key.H, AnsiKey.H},
            {Key.I, AnsiKey.I},
            {Key.J, AnsiKey.J},
            {Key.K, AnsiKey.K},
            {Key.L, AnsiKey.L},
            {Key.M, AnsiKey.M},
            {Key.N, AnsiKey.N},
            {Key.O, AnsiKey.O},
            {Key.P, AnsiKey.P},
            {Key.Q, AnsiKey.Q},
            {Key.R, AnsiKey.R},
            {Key.S, AnsiKey.S},
            {Key.T, AnsiKey.T},
            {Key.U, AnsiKey.U},
            {Key.V, AnsiKey.V},
            {Key.W, AnsiKey.W},
            {Key.X, AnsiKey.X},
            {Key.Y, AnsiKey.Y},
            {Key.Z, AnsiKey.Z},
            {Key.Quote, AnsiKey.Apostophe},
            {Key.BackSlash, AnsiKey.Backslash},
            {Key.BackSpace, AnsiKey.Backspace},
            {Key.Comma, AnsiKey.Comma},
            {Key.Number0, AnsiKey.D0},
            {Key.Number1, AnsiKey.D1},
            {Key.Number2, AnsiKey.D2},
            {Key.Number3, AnsiKey.D3},
            {Key.Number4, AnsiKey.D4},
            {Key.Number5, AnsiKey.D5},
            {Key.Number6, AnsiKey.D6},
            {Key.Number7, AnsiKey.D7},
            {Key.Number8, AnsiKey.D8},
            {Key.Number9, AnsiKey.D9},
            {Key.Delete, AnsiKey.Delete},
            {Key.Down, AnsiKey.Down},
            {Key.End, AnsiKey.End},
            {Key.Enter, AnsiKey.Enter},
            {Key.Plus, AnsiKey.Equals},
            {Key.Escape, AnsiKey.Escape},
            {Key.F1, AnsiKey.F1},
            {Key.F2, AnsiKey.F2},
            {Key.F3, AnsiKey.F3},
            {Key.F4, AnsiKey.F4},
            {Key.F5, AnsiKey.F5},
            {Key.F6, AnsiKey.F6},
            {Key.F7, AnsiKey.F7},
            {Key.F8, AnsiKey.F8},
            {Key.F9, AnsiKey.F9},
            {Key.F10, AnsiKey.F10},
            {Key.F11, AnsiKey.F11},
            {Key.F12, AnsiKey.F12},
            {Key.Grave, AnsiKey.Grave},
            {Key.Home, AnsiKey.Home},
            {Key.Insert, AnsiKey.Insert},
            {Key.Left, AnsiKey.Left},
            {Key.BracketLeft, AnsiKey.LeftSquare},
            {Key.Minus, AnsiKey.Minus},
            {Key.Keypad0, AnsiKey.Num0},
            {Key.Keypad1, AnsiKey.Num1},
            {Key.Keypad2, AnsiKey.Num2},
            {Key.Keypad3, AnsiKey.Num3},
            {Key.Keypad4, AnsiKey.Num4},
            {Key.Keypad5, AnsiKey.Num5},
            {Key.Keypad6, AnsiKey.Num6},
            {Key.Keypad7, AnsiKey.Num7},
            {Key.Keypad8, AnsiKey.Num8},
            {Key.Keypad9, AnsiKey.Num9},
            {Key.KeypadEnter, AnsiKey.NumEnter},
            {Key.KeypadMinus, AnsiKey.NumMinus},
            {Key.KeypadPeriod, AnsiKey.NumPeriod},
            {Key.KeypadPlus, AnsiKey.NumPlus},
            {Key.KeypadDivide, AnsiKey.NumSlash},
            {Key.KeypadMultiply, AnsiKey.NumStar},
            {Key.PageDown, AnsiKey.PageDown},
            {Key.PageUp, AnsiKey.PageUp},
            {Key.Pause, AnsiKey.Pause},
            {Key.Period, AnsiKey.Period},
            {Key.PrintScreen, AnsiKey.PrintScreen},
            {Key.Right, AnsiKey.Right},
            {Key.BracketRight, AnsiKey.RightSquare},
            {Key.Semicolon, AnsiKey.Semicolon},
            {Key.Slash, AnsiKey.Slash},
            {Key.Space, AnsiKey.Space},
            {Key.Tab, AnsiKey.Tab},
            {Key.Up, AnsiKey.Up}
        };

        public bool Press(KeyboardKeyEventArgs data)
        {
            if (!Map.ContainsKey(data.Key))
                return false;
            
            Enqueue(new KeyPress
            {
                Key = Map[data.Key],
                Shift = data.Shift,
                Control = data.Control,
                Alt = data.Alt
            });

            return true;
        }
    }
}