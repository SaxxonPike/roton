using System;

namespace Roton.Events
{
    public delegate void DataEventHandler(object sender, DataEventArgs e);

    public class DataEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}
