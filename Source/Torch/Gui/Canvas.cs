using System.Numerics;

namespace Torch.Gui;

public struct Canvas(ImDrawList dl, Vector2 screenPos)
{
    private ImDrawList _dl = dl;

    public void DrawRect(Vector2 min, Vector2 max, Vector4 color) => 
        _dl.AddRect(min + screenPos, max + screenPos, PackedColor.Encode(color));
}