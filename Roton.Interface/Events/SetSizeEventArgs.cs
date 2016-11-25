using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Interface.Events
{
    public delegate void SetSizeEventHandler(object sender, SetSizeEventArgs e);

    public class SetSizeEventArgs : EventArgs
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Wide { get; set; }
    }
}
