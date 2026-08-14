using System.Collections.Generic;
using Roton.Emulation.Core;

namespace Roton.Test.Infrastructure;

public class TestKeyboard : IKeyboard
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

    public void Press(IKeyPress keyPress)
        => _queue.Enqueue(keyPress);
}