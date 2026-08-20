namespace Roton.Emulation.Core;

public readonly struct KeyPress(AnsiKey key, KeyMod mod)
{
    public AnsiKey Key { get; } = key;
    public KeyMod Mod { get; } = mod;   
}