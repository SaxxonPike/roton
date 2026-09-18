using System;
using System.Collections.Generic;

namespace Torch.Gui.Components;

public class ComponentMap<TItem> : Component
{
    private readonly List<Component> _tempComponents = [];

#pragma warning disable CS8714
    private readonly Dictionary<TItem, WeakReference<Component>> _map = [];
#pragma warning restore CS8714

    public ComponentMap(Action<ComponentMap<TItem>>? config = null)
    {
        config?.Invoke(this);
    }

    public Func<TItem, int, Component>? Mapper { get; set; }
    public Func<IEnumerable<TItem>>? Items { get; set; }

    protected override void OnRender()
    {
        _tempComponents.Clear();

        var idx = -1;

        foreach (var item in Items?.Invoke() ?? [])
        {
            idx++;

            if (item == null)
                continue;

            if (_map.TryGetValue(item, out var componentRef) &&
                componentRef.TryGetTarget(out var existingComponent))
            {
                _tempComponents.Add(existingComponent);
            }
            else
            {
                if (Mapper?.Invoke(item, idx) is not { } newComponent)
                    continue;

                _map[item] = new WeakReference<Component>(newComponent);
                _tempComponents.Add(newComponent);
            }
        }

        for (var i = 0; i < _tempComponents.Count; i++)
            OnRenderChild(i, _tempComponents[i]);

        _tempComponents.Clear();
    }

    protected virtual void OnRenderChild(int index, Component component) =>
        component.Render();
}