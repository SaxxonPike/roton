namespace Roton.Editors;

public struct Tile(int element, int color)
{
    public int Element { get; set; } = element;
    public int Color { get; set; } = color;
}