using System;

namespace Roton.Emulation.Execution
{
    public class OopItem
    {
        private readonly Func<int> _read;
        private readonly Action<int> _write;

        public OopItem(Func<int> read, Action<int> write)
        {
            _read = read;
            _write = write;
        }

        public int Value
        {
            get { return _read(); }
            set { _write(value); }
        }
    }
}