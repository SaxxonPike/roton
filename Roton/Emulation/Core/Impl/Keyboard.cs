using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Core.Impl
{
    public abstract class Keyboard : IKeyboard
    {
        private readonly Queue<IKeyPress> _queue = new Queue<IKeyPress>();
        private readonly object _syncObject = new object();

        public void Clear()
        {
            lock (_syncObject)
            {
                _queue.Clear();
            }
        }

        public bool KeyIsAvailable
        {
            get
            {
                lock (_syncObject)
                    return _queue.Count > 0;
            }
        }

        public IKeyPress GetKey()
        {
            lock (_syncObject)
            {
                if (_queue.Count > 0)
                {
                    return _queue.Dequeue();
                }

                return null;
            }
        }

        protected void Enqueue(IKeyPress keyPress)
        {
            lock (_syncObject)
            {
                _queue.Enqueue(keyPress);
            }
        }
    }
}