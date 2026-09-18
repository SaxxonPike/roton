using System;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public class Separator : Component
{
    public Separator(Action<Separator>? config = null)
    {
        config?.Invoke(this);
    }

    protected override void OnRender() => 
        ImGui.Separator();
}