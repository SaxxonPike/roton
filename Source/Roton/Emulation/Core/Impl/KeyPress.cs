namespace Roton.Emulation.Core.Impl;

public sealed class KeyPress : IKeyPress
{
    public AnsiKey Key { get; set; }
    public KeyMod Mod { get; set; }
}