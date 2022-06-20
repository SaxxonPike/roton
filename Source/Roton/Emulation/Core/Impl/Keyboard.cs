using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Core.Impl;

public abstract class Keyboard : IKeyboard
{
    private readonly Queue<IKeyPress> _queue = new();

    public void Clear()
        => _queue.Clear();

    public bool KeyIsAvailable
        => _queue.Count > 0;

    public IKeyPress GetKey()
        => _queue.Count > 0
            ? _queue.Dequeue()
            : null;

    public int BufferLength
        => _queue.Count;

    protected void Enqueue(IKeyPress keyPress)
    {
        if (_queue.Count(q => q.Key == keyPress.Key) < 2)
            _queue.Enqueue(keyPress);
    }
}