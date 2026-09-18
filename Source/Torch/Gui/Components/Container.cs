using System.Collections.Generic;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public abstract class Container(params IEnumerable<Component> components) : Component
{
    private readonly List<Component> _tempComponents = [];

    public List<Component> Components { get; set; } = [.. components];

    protected override void OnRender()
    {
        _tempComponents.Clear();
        _tempComponents.AddRange(Components);

        for (var i = 0; i < _tempComponents.Count; i++)
            OnRenderChild(i, _tempComponents[i]);
    }

    protected virtual void OnRenderChild(int index, Component component) =>
        component.Render();
}