namespace Roton.Emulation.Core;

public interface IKeyboard
{
    void Clear();
    bool KeyIsAvailable { get; }
    KeyPress? GetKey();
    int BufferLength { get; }
    KeyMod GetMod();
}