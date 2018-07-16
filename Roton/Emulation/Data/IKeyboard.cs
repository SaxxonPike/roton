using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data
{
    public interface IKeyboard
    {
        bool Alt { get; }
        bool Control { get; }
        bool Shift { get; }
        void Clear();
        EngineKeyCode GetKey();
    }
}