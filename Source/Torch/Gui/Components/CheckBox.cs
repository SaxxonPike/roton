using System;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public class CheckBox : Component
{
    public CheckBox(Action<CheckBox>? config = null)
    {
        config?.Invoke(this);
    }

    public Action<CheckBox>? OnSelect { get; set; }
    public Func<CheckBox, bool>? IsSelected { get; set; }
    public Func<CheckBox, string>? Label { get; set; }

    protected override void OnRender()
    {
        var label = Label?.Invoke(this) ?? "(missing label)";
        var isSelected = IsSelected?.Invoke(this) ?? false;

        if (ImGui.Checkbox(label, ref isSelected))
            OnSelect?.Invoke(this);
    }
}