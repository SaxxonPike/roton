using System;
using System.IO;

namespace Roton.Events
{
    public delegate void StreamEventHandler(object sender, StreamEventArgs e);

    public class StreamEventArgs : EventArgs
    {
        public Stream Stream { get; set; }
    }
}
