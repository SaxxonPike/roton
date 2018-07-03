using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using Roton.Core;

namespace Roton.Interface.Input
{
    public class OpenTkKeyBuffer : IKeyboard
    {
        private static readonly Encoding Enc = Encoding.GetEncoding(437);
        private readonly Queue<int> _queue = new Queue<int>();
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

        public int GetKey()
        {
            lock (_syncObject)
            {
                if (_queue.Count > 0)
                {
                    return _queue.Dequeue();
                }
                return -1;
            }
        }

        public bool Shift { get; set; }

        private void Enqueue(int data)
        {
            lock (_syncObject)
            {
                if (_queue.Count(i => i == data) < 2)
                    _queue.Enqueue(data);
            }
        }

        private int GetCode(Key data)
        {
            switch (data)
            {
                case Key.Back:
                    return 0x08;
                case Key.Down:
                    return 0xD0;
                case Key.Enter:
                    return 0x0D;
                case Key.Escape:
                    return 0x1B;
                case Key.Left:
                    return 0xCB;
                case Key.PageDown:
                    return 0xD1;
                case Key.PageUp:
                    return 0xC9;
                case Key.Right:
                    return 0xCD;
                case Key.Space:
                    return 0x20;
                case Key.Tab:
                    return 0x09;
                case Key.Up:
                    return 0xC8;
            }
            return -1;
        }

        public bool Press(char data)
        {
            try
            {
                int code = Enc.GetBytes(new[] {data})[0];
                if (code >= 0x20 && code <= 0xFF)
                {
                    Enqueue(code);
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