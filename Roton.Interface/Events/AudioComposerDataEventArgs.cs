using System;

namespace Roton.Interface.Events
{
    public class AudioComposerDataEventArgs : EventArgs
    {
        public int[] Data { get; set; }
    }
}