using System;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public class MenuItem : Component
{
    public MenuItem(Action<MenuItem>? config = null)
    {
        config?.Invoke(this);
    }

    public Action<MenuItem>? OnClick { get; set; }
    public Func<MenuItem, string>? Label { get; set; }
    public Func<MenuItem, string>? Shortcut { get; set; }
    public Func<MenuItem, bool>? IsSelected { get; set; }
    public Func<MenuItem, bool>? IsEnabled { get; set; }

    protected override void OnRender()
    {
        var label = Label?.Invoke(this) ?? "(missing label)";
        var shortcut = Shortcut?.Invoke(this) ?? string.Empty;
        var selected = IsSelected?.Invoke(this) ?? false;
        var enabled = IsEnabled?.Invoke(this) ?? true;

        if (ImGui.MenuItem(label, shortcut, selected, enabled))
            OnClick?.Invoke(this);
    }
}