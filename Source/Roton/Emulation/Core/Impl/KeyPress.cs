namespace Roton.Emulation.Core.Impl
{
    public sealed class KeyPress : IKeyPress
    {
        public AnsiKey Key { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }
        public bool Alt { get; set; }
    }
}