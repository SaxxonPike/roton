using Roton.Emulation.Core.Impl;

namespace Roton.Emulation.Core;

public interface IKeyboard
{
    void Clear();
    bool KeyIsAvailable { get; }
    IKeyPress? GetKey();
    int BufferLength { get; }
    KeyMod GetMod();
}