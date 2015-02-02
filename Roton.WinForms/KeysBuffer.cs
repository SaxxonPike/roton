using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Roton.WinForms
{
    public class KeysBuffer : IKeyboard
    {
        static private Encoding _enc = Encoding.GetEncoding(437);
        private Queue<int> _queue = new Queue<int>();

        public bool Alt
        {
            get;
            set;
        }

        public bool CapsLock
        {
            get;
            set;
        }

        public void Clear()
        {
            lock (_queue)
            {
                _queue.Clear();
                Shift = false;
                Control = false;
                Alt = false;
            }
        }

        public bool Control
        {
            get;
            set;
        }

        void Enqueue(int data)
        {
            lock (_queue)
            {
                if (_queue.Count(i => i == data) < 2)
                    _queue.Enqueue(data);
            }
        }

        private int GetCode(Keys data)
        {
            var code = (data & Keys.KeyCode);
            var shift = ((data & Keys.Shift) != 0) ^ CapsLock;
            var alt = (data & Keys.Alt) != 0;
            var ctrl = (data & Keys.Control) != 0;
            switch (code)
            {
                case Keys.Back: return 0x08;
                case Keys.Down: return 0xD0;
                case Keys.Enter: return 0x0D;
                case Keys.Escape: return 0x1B;
                case Keys.Left: return 0xCB;
                case Keys.PageDown: return 0xD1;
                case Keys.PageUp: return 0xC9;
                case Keys.Right: return 0xCD;
                case Keys.Space: return 0x20;
                case Keys.Tab: return 0x09;
                case Keys.Up: return 0xC8;
            }
            return -1;
        }

        public int GetKey()
        {
            lock (_queue)
            {
                if (_queue.Count > 0)
                {
                    return _queue.Dequeue();
                }
                return -1;
            }
        }

        public bool NumLock
        {
            get;
            set;
        }

        public bool Press(char data)
        {
            try
            {
                int code = _enc.GetBytes(new char[] { data })[0];
                if (code >= 0x20 && code <= 0xFF)
                {
                    Enqueue(code);
                }
                else
                {
                    code = -1;
                }
                return (code >= 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Press(Keys data)
        {
            int code = GetCode(data);
            if (code >= 0)
                Enqueue(code);
            return (code >= 0);
        }

        public bool Shift
        {
            get;
            set;
        }
    }
}
