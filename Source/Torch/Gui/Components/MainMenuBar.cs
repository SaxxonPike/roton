using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public class MainMenuBar : Container
{
    public MainMenuBar(Action<MainMenuBar>? config, params IEnumerable<Component> components) : base(components)
    {
        config?.Invoke(this);
    }

    public MainMenuBar(Action<MainMenuBar>? config) : this(config, [])
    {
    }

    public MainMenuBar(params IEnumerable<Component> components) : this(null, components)
    {
    }

    protected override bool OnPreRender() =>
        ImGui.BeginMainMenuBar();

    protected override void OnPostRender(bool rendered)
    {
        if (rendered)
            ImGui.EndMainMenuBar();
    }
}