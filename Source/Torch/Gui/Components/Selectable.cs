using System;
using System.Numerics;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public class Selectable : Component
{
    public Selectable(Action<Selectable>? config = null)
    {
        config?.Invoke(this);
    }

    public Action<Selectable>? OnClick { get; set; }
    public Func<Selectable, string>? Label { get; set; }
    public Func<Selectable, bool>? IsSelected { get; set; }
    public Func<Selectable, ImGuiSelectableFlags>? Flags { get; set; }
    public Func<Selectable, Vector2>? Size { get; set; }

    protected override void OnRender()
    {
        var label = Label?.Invoke(this) ?? "(missing label)";
        var isSelected = IsSelected?.Invoke(this) ?? false;
        var flags = Flags?.Invoke(this) ?? ImGuiSelectableFlags.None;
        var size = Size?.Invoke(this) ?? Vector2.Zero;

        if (size != Vector2.Zero
                ? ImGui.Selectable(label, isSelected, flags, size)
                : ImGui.Selectable(label, isSelected, flags))
            OnClick?.Invoke(this);
    }
}