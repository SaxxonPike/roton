using System.Numerics;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public abstract class Component
{
    private bool _isRendering;

    public object? DataObject { get; set; }
    public Vector2 ScreenPosition { get; private set; }

    public T Data<T>() => (T)DataObject!;

    public void Render()
    {
        if (_isRendering)
            return;
        _isRendering = true;
        var rendered = OnPreRender();
        if (rendered)
        {
            ScreenPosition = ImGui.GetCursorScreenPos();
            OnRender();
        }
        OnPostRender(rendered);
        _isRendering = false;
    }

    protected virtual bool OnPreRender() =>
        true;

    protected virtual void OnRender()
    {
    }

    protected virtual void OnPostRender(bool rendered)
    {
    }
}