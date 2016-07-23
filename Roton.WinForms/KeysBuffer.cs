using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Roton.Core;

namespace Roton.WinForms
{
    public class KeysBuffer : IKeyboard
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

        private int GetCode(Keys data)
        {
            var code = data & Keys.KeyCode;
            var shift = ((data & Keys.Shift) != 0) ^ CapsLock;
            var alt = (data & Keys.Alt) != 0;
            var ctrl = (data & Keys.Control) != 0;
            switch (code)
            {
                case Keys.Back:
                    return 0x08;
                case Keys.Down:
                    return 0xD0;
                case Keys.Enter:
                    return 0x0D;
                case Keys.Escape:
                    return 0x1B;
                case Keys.Left:
                    return 0xCB;
                case Keys.PageDown:
                    return 0xD1;
                case Keys.PageUp:
                    return 0xC9;
                case Keys.Right:
                    return 0xCD;
                case Keys.Space:
                    return 0x20;
                case Keys.Tab:
                    return 0x09;
                case Keys.Up:
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

        public bool Press(Keys data)
        {
            var code = GetCode(data);
            if (code >= 0)
                Enqueue(code);
            return code >= 0;
        }
    }
}