using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public class Menu : Container
{
    public Menu(Action<Menu>? config, params IEnumerable<Component> components) : base(components)
    {
        config?.Invoke(this);
    }

    public Menu(Action<Menu>? config) : this(config, [])
    {
    }

    public Menu(params IEnumerable<Component> components) : this(null, components)
    {
    }

    public Func<Menu, string>? Label { get; set; }
    public Func<Menu, bool>? IsEnabled { get; set; }

    protected override bool OnPreRender()
    {
        var label = Label?.Invoke(this) ?? "(missing label)";
        var isEnabled = IsEnabled?.Invoke(this) ?? true;

        return ImGui.BeginMenu(label, isEnabled);
    }

    protected override void OnPostRender(bool rendered)
    {
        if (rendered)
            ImGui.EndMenu();
    }
}