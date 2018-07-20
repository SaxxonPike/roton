using Roton.Emulation.Core.Impl;

namespace Roton.Emulation.Core
{
    public interface IKeyPress
    {
        AnsiKey Key { get; }
        bool Shift { get; }
        bool Control { get; }
        bool Alt { get; }
    }
}