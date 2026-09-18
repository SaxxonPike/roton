using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public class Combo : Container
{
    public Combo(Action<Combo>? config, params IEnumerable<Component> components) : base(components)
    {
        config?.Invoke(this);
    }

    public Combo(Action<Combo>? config) : this(config, [])
    {
    }

    public Combo(params IEnumerable<Component> components) : this(null, components)
    {
    }

    public Func<Combo, string>? Label { get; set; }
    public Func<Combo, string>? Preview { get; set; }
    public Func<Combo, ImGuiComboFlags>? Flags { get; set; }

    protected override bool OnPreRender()
    {
        var label = Label?.Invoke(this) ?? "(missing label)";
        var preview = Preview?.Invoke(this) ?? string.Empty;
        var flags = Flags?.Invoke(this) ?? ImGuiComboFlags.None;

        return ImGui.BeginCombo(label, preview, flags);
    }

    protected override void OnPostRender(bool rendered)
    {
        if (rendered)
            ImGui.EndCombo();
    }
}