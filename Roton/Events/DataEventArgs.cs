using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Events
{
    public delegate void DataEventHandler(object sender, DataEventArgs e);

    public class DataEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}
