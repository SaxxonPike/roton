using Roton.Emulation.Core.Impl;

namespace Roton.Emulation.Core;

public interface IKeyPress
{
    AnsiKey Key { get; }
    KeyMod Mod { get; }
}