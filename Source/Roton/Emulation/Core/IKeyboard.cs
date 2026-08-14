namespace Roton.Emulation.Core;

public interface IKeyboard
{
    void Clear();
    bool KeyIsAvailable { get; }
    IKeyPress GetKey();
    int BufferLength { get; }
}