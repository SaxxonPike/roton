using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using Roton.Emulation.Infrastructure;

namespace Roton.Interface.Input
{
    public class OpenTkKeyBuffer : IOpenTkKeyBuffer
    {
        private static readonly Encoding Enc = Encoding.GetEncoding(437);
        private readonly Queue<EngineKeyCode> _queue = new Queue<EngineKeyCode>();
        private readonly object _syncObject = new object();

        public bool CapsLock { get; set; }

        public bool Alt { get; set; }

        public void Clear()
        {
            lock (_syncObject)
            {
                _queue.Clear();
                Shift = false;
                Control = false;
                Alt = false;
            }
        }

        public bool Control { get; set; }

        public EngineKeyCode GetKey()
        {
            lock (_syncObject)
            {
                if (_queue.Count > 0)
                {
                    return _queue.Dequeue();
                }
                return EngineKeyCode.None;
            }
        }

        public bool Shift { get; set; }

        private void Enqueue(EngineKeyCode data)
        {
            lock (_syncObject)
            {
                if (_queue.Count(i => i == data) < 2)
                    _queue.Enqueue(data);
            }
        }

        private EngineKeyCode GetCode(Key data)
        {
            switch (data)
            {
                case Key.Back:
                    return EngineKeyCode.Backspace;
                case Key.Down:
                    return EngineKeyCode.Down;
                case Key.Enter:
                    return EngineKeyCode.Enter;
                case Key.Escape:
                    return EngineKeyCode.Escape;
                case Key.Left:
                    return EngineKeyCode.Left;
                case Key.PageDown:
                    return EngineKeyCode.PageDown;
                case Key.PageUp:
                    return EngineKeyCode.PageUp;
                case Key.Right:
                    return EngineKeyCode.Right;
                case Key.Space:
                    return EngineKeyCode.Space;
                case Key.Tab:
                    return EngineKeyCode.Tab;
                case Key.Up:
                    return EngineKeyCode.Up;
                default:
                    return EngineKeyCode.None;
            }
        }

        public bool Press(char data)
        {
            try
            {
                int code = Enc.GetBytes(new[] {data})[0];
                if (code >= 0x20 && code <= 0xFF)
                {
                    Enqueue((EngineKeyCode)code);
                }
                else
                {
                    code = -1;
                }
                return code >= 0;
            }
            catch (Exception)
            {
                // todo: This kind of error handling is bad.
                return false;
            }
        }

        public bool Press(Key data)
        {
            var code = GetCode(data);
            if (code >= 0)
                Enqueue(code);
            return code >= 0;
        }
    }
}