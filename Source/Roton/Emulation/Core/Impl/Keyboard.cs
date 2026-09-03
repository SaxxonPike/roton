using System.Collections.Concurrent;
using System.Linq;

namespace Roton.Emulation.Core.Impl;

public abstract class Keyboard : IKeyboard
{
    private readonly ConcurrentQueue<KeyPress> _queue = new();
    private KeyMod _mod;

    public void Clear()
    {
        while (_queue.TryDequeue(out _))
        {
        }
    }

    public bool KeyIsAvailable
        => !_queue.IsEmpty;

    public KeyPress? GetKey()
        => _queue.TryDequeue(out var keyPress)
            ? keyPress
            : null;

    public int BufferLength
        => _queue.Count;

    public KeyMod GetMod() =>
        _mod;

    protected void SetMod(KeyMod mod) =>
        _mod = mod;

    protected void Enqueue(KeyPress keyPress)
    {
        if (_queue.Count(q => q.Key == keyPress.Key) < 2)
            _queue.Enqueue(keyPress);
    }
}