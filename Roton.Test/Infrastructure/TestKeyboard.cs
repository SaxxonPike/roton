using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Test.Infrastructure
{
    public class TestKeyboard : IKeyboard
    {
        private readonly Queue<KeyPress> _queue = new Queue<KeyPress>();
        
        public bool Alt { get; private set; }
        public bool Control { get; private set;}
        public bool Shift { get; private set;}

        public void Type(KeyPress keyPress)
        {
            _queue.Enqueue(new KeyPress
            {
                Code = keyPress.Code,
                Shift = keyPress.Shift,
                Alt = keyPress.Alt,
                Control = keyPress.Control
            });
        }
        
        public void Clear()
        {
            _queue.Clear();
        }

        public int GetKey()
        {
            if (_queue.Count <= 0) 
                return 0;
            
            var key = _queue.Dequeue();
            Shift = key.Shift;
            Alt = key.Alt;
            Control = key.Control;
            return key.Code;
        }
    }
}