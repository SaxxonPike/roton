using System;
using System.Numerics;

namespace Torch.Gui.Components;

public class Image : Component
{
    public Image(Action<Image>? config = null)
    {
        config?.Invoke(this);
    }

    public Func<Image, IntPtr>? Texture { get; set; }
    public Func<Image, Vector2>? Size { get; set; }
    public Func<Image, Vector2>? UvMin { get; set; }
    public Func<Image, Vector2>? UvMax { get; set; }

    protected override unsafe void OnRender()
    {
        var tex = Texture?.Invoke(this) ?? 0;
        var size = Size?.Invoke(this) ?? default;
        var uvMin = UvMin?.Invoke(this) ?? Vector2.Zero;
        var uvMax = UvMax?.Invoke(this) ?? Vector2.One;


        if (tex == IntPtr.Zero || size == Vector2.Zero)
            return;

        var texRef = new ImTextureRef(texId: new ImTextureID(tex));
        ImGui.Image(texRef, size, uvMin, uvMax);
    }
}