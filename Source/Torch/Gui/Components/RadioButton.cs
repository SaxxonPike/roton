using System;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public class RadioButton : Component
{
    public RadioButton(Action<RadioButton>? config = null)
    {
        config?.Invoke(this);
    }

    public Action<RadioButton>? OnSelect { get; set; }
    public Func<RadioButton, bool>? IsSelected { get; set; }
    public Func<RadioButton, string>? Label { get; set; }

    protected override void OnRender()
    {
        var label = Label?.Invoke(this) ?? "(missing label)";
        var isSelected = IsSelected?.Invoke(this) ?? false;

        if (ImGui.RadioButton(label, isSelected))
            OnSelect?.Invoke(this);
    }
}