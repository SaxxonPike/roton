using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Torch.Gui.Components;

[PublicAPI]
public class Window : Container
{
    public Window(Action<Window>? config, params IEnumerable<Component> components) : base(components)
    {
        config?.Invoke(this);
    }

    public Window(Action<Window>? config) : this(config, [])
    {
    }

    public Window(params IEnumerable<Component> components) : this(null, components)
    {
    }

    private bool _shouldEnd;

    public Action<Window>? OnRendered { get; set; }
    public Action<Window>? OnClose { get; set; }
    public Func<Window, string>? Label { get; set; }
    public Func<Window, bool>? IsOpen { get; set; }
    public Func<Window, ImGuiWindowFlags>? Flags { get; set; }

    protected override bool OnPreRender()
    {
        var isOpen = IsOpen?.Invoke(this) ?? true;
        if (!isOpen)
        {
            _shouldEnd = false;
            return false;
        }

        var label = Label?.Invoke(this) ?? "(missing label)";
        var flags = Flags?.Invoke(this) ?? ImGuiWindowFlags.None;

        var wasOpen = isOpen;
        var result = ImGui.Begin(label, ref isOpen, flags);

        _shouldEnd = true;

        if (wasOpen && !isOpen)
            OnClose?.Invoke(this);

        return result;
    }

    protected override void OnRender()
    {
        base.OnRender();
        OnRendered?.Invoke(this);
    }

    protected override void OnPostRender(bool rendered)
    {
        if (_shouldEnd)
            ImGui.End();
    }
}