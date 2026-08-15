using System.Collections.Generic;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;

namespace Roton.Test.Infrastructure;

public class TestKeyboard : IKeyboard
{
    private readonly Queue<IKeyPress> _queue = new();
    private KeyMod _mod;

    public void Clear() =>
        _queue.Clear();

    public bool KeyIsAvailable =>
        _queue.Count > 0;

    public IKeyPress GetKey() =>
        _queue.Count > 0
            ? _queue.Dequeue()
            : null;

    public int BufferLength =>
        _queue.Count;

    public KeyMod GetMod() =>
        _mod;

    public void Press(IKeyPress keyPress)
    {
        SetMod(keyPress.Mod);
        _queue.Enqueue(keyPress);
    }

    public void SetMod(KeyMod mod) =>
        _mod = mod;
}