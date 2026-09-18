using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public class Disabled : Container
{
    public Disabled(Action<Disabled>? config, params IEnumerable<Component> components) : base(components)
    {
        config?.Invoke(this);
    }

    public Disabled(Action<Disabled>? config) : this(config, [])
    {
    }

    public Disabled(params IEnumerable<Component> components) : this(null, components)
    {
    }

    public Func<Disabled, bool>? IsDisabled { get; set; }

    protected override bool OnPreRender()
    {
        var isDisabled = IsDisabled?.Invoke(this) ?? true;
        ImGui.BeginDisabled(isDisabled);
        return true;
    }

    protected override void OnPostRender(bool rendered)
    {
        ImGui.EndDisabled();
    }
}