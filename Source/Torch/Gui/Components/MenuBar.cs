using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public class MenuBar : Container
{
    public MenuBar(Action<MenuBar>? config, params IEnumerable<Component> components) : base(components)
    {
        config?.Invoke(this);
    }

    public MenuBar(Action<MenuBar>? config) : this(config, [])
    {
    }

    public MenuBar(params IEnumerable<Component> components) : this(null, components)
    {
    }

    public Action<MenuBar>? OnRendering { get; set; }
    public Action<MenuBar>? OnRendered { get; set; }

    protected override bool OnPreRender() =>
        ImGui.BeginMenuBar();

    protected override void OnRender()
    {
        OnRendering?.Invoke(this);
        base.OnRender();
        OnRendered?.Invoke(this);
    }

    protected override void OnPostRender(bool rendered)
    {
        if (rendered)
            ImGui.EndMenuBar();
    }
}