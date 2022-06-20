using System;

namespace Roton.Composers.Video.Scenes.Impl;

public class ResizedEventArgs : EventArgs
{
    public int Width { get; set; }
    public int Height { get; set; }
    public bool Wide { get; set; }
}