using System;

namespace Roton.Composers.Video
{
    public delegate void SetSizeEventHandler(object sender, SetSizeEventArgs e);

    public class SetSizeEventArgs : EventArgs
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Wide { get; set; }
    }
}
