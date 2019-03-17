using System;

namespace Roton.Composers.Video.Scenes.Impl
{
    public class FontDataChangedEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}