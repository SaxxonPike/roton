using Roton.Emulation.Infrastructure;

namespace Roton.Test.Infrastructure
{
    public class KeyPress
    {
        public EngineKeyCode Code { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }
        public bool Alt { get; set; }
    }
}