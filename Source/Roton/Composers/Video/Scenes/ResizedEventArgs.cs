namespace Roton.Composers.Video.Scenes;

public readonly struct ResizedEventArgs(int width, int height, bool wide)
{
    public int Width => width;
    public int Height => height;
    public bool Wide => wide;
}