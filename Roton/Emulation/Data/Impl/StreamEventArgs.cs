using System;
using System.IO;

namespace Roton.Emulation.Data.Impl
{
    public delegate void StreamEventHandler(object sender, StreamEventArgs e);

    public class StreamEventArgs : EventArgs
    {
        public Stream Stream { get; set; }
    }
}
