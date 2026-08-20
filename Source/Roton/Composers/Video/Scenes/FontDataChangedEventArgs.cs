namespace Roton.Composers.Video.Scenes;

public readonly struct FontDataChangedEventArgs(byte[]? data)
{
    public byte[]? Data => data;
}