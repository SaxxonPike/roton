using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public class SameLine : Container
{
    public SameLine(Action<SameLine>? config, params IEnumerable<Component> components) : base(components)
    {
        config?.Invoke(this);
    }

    public SameLine(Action<SameLine>? config) : this(config, [])
    {
    }

    public SameLine(params IEnumerable<Component> components) : this(null, components)
    {
    }

    protected override void OnRenderChild(int index, Component component)
    {
        if (index > 0)
            ImGui.SameLine();
        base.OnRenderChild(index, component);
    }
}