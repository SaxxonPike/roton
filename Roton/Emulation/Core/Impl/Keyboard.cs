using System.Collections.Generic;

namespace Roton.Emulation.Core.Impl
{
    public abstract class Keyboard : IKeyboard
    {
        private readonly Queue<IKeyPress> _queue = new Queue<IKeyPress>();

        public void Clear()
            => _queue.Clear();

        public bool KeyIsAvailable
            => _queue.Count > 0;

        public IKeyPress GetKey()
            => _queue.Count > 0
                ? _queue.Dequeue()
                : null;

        protected void Enqueue(IKeyPress keyPress)
            => _queue.Enqueue(keyPress);
    }
}