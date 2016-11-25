using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roton.Events
{
    public delegate void StreamEventHandler(object sender, StreamEventArgs e);

    public class StreamEventArgs : EventArgs
    {
        public Stream Stream { get; set; }
    }
}
