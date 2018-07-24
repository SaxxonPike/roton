using System;

namespace Roton.Interface.Events
{
    public class AudioComposerDataEventArgs : EventArgs
    {
        public float[] Data { get; set; }
    }
}