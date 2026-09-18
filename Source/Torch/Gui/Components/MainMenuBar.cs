using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Torch.Gui.Components;

public abstract class Component(params IEnumerable<Component> components)
{
    private bool _isRendering;
    private readonly List<Component> _tempComponents = [];

    public List<Component> Components { get; set; } = [.. components];

    public void Render()
    {
        if (_isRendering)
            return;
        _isRendering = true;
        var rendered = OnPreRender();
        if (rendered)
            OnRender();
        OnPostRender(rendered);
        _isRendering = false;
    }

    protected virtual bool OnPreRender() =>
        true;

    protected virtual void OnRender()
    {
        _tempComponents.Clear();
        _tempComponents.AddRange(Components);

        for (var i = 0; i < _tempComponents.Count; i++)
            OnRenderChild(i, _tempComponents[i]);
    }

    protected virtual void OnRenderChild(int index, Component component) =>
        component.Render();

    protected virtual void OnPostRender(bool rendered)
    {
    }
}

public class MainMenuBar(params IEnumerable<Component> components) : Component(components)
{
    public static MainMenuBar Create(params IEnumerable<Component> components) => 
        new(components);

    public static MainMenuBar Create(Action<MainMenuBar> config, params IEnumerable<Component> components)
    {
        var result = new MainMenuBar(components);
        config(result);
        return result;
    }

    protected override bool OnPreRender() =>
        ImGui.BeginMainMenuBar();

    protected override void OnPostRender(bool rendered)
    {
        if (rendered)
            ImGui.EndMainMenuBar();
    }
}

public class MenuBar(params IEnumerable<Component> components) : Component(components)
{
    public static MenuBar Create(params IEnumerable<Component> components) => 
        new(components);

    public static MenuBar Create(Action<MenuBar> config, params IEnumerable<Component> components)
    {
        var result = new MenuBar(components);
        config(result);
        return result;
    }

    protected override bool OnPreRender() =>
        ImGui.BeginMenuBar();

    protected override void OnPostRender(bool rendered)
    {
        if (rendered)
            ImGui.EndMenuBar();
    }
}

public static class Menu
{
    public static Menu<TData> Create<TData>(TData data, params IEnumerable<Component> components) => 
        new(data, components);

    public static Menu<TData> Create<TData>(TData data, Action<Menu<TData>> config,
        params IEnumerable<Component> components)
    {
        var result = new Menu<TData>(data, components);
        config(result);
        return result;
    }
}

public class Menu<TData>(TData data, params IEnumerable<Component> components) : Component(components)
{
    public Func<TData, string>? Label { get; set; }
    public Func<TData, bool>? IsEnabled { get; set; }

    protected override bool OnPreRender()
    {
        var label = Label?.Invoke(data) ?? "(missing label)";
        var isEnabled = IsEnabled?.Invoke(data) ?? true;

        return ImGui.BeginMenu(label, isEnabled);
    }

    protected override void OnPostRender(bool rendered)
    {
        if (rendered)
            ImGui.EndMenu();
    }
}

public static class MenuItem
{
    public static MenuItem<TData> Create<TData>(TData data) => 
        new(data);

    public static MenuItem<TData> Create<TData>(TData data, Action<MenuItem<TData>> config)
    {
        var result = new MenuItem<TData>(data);
        config(result);
        return result;
    }
}

public class MenuItem<TData>(TData data) : Component
{
    public Action<TData>? OnClick { get; set; }
    public Func<TData, string>? Label { get; set; }
    public Func<TData, string>? Shortcut { get; set; }
    public Func<TData, bool>? IsSelected { get; set; }
    public Func<TData, bool>? IsEnabled { get; set; }

    protected override void OnRender()
    {
        var label = Label?.Invoke(data) ?? "(missing label)";
        var shortcut = Shortcut?.Invoke(data) ?? string.Empty;
        var selected = IsSelected?.Invoke(data) ?? false;
        var enabled = IsEnabled?.Invoke(data) ?? true;

        if (ImGui.MenuItem(label, shortcut, selected, enabled))
            OnClick?.Invoke(data);
    }
}

public static class Window
{
    public static Window<TData> Create<TData>(TData data, params IEnumerable<Component> components) => 
        new(data, components);

    public static Window<TData> Create<TData>(TData data, Action<Window<TData>> config,
        params IEnumerable<Component> components)
    {
        var result = new Window<TData>(data, components);
        config(result);
        return result;
    }
}

public class Window<TData>(TData data, params IEnumerable<Component> components) : Component(components)
{
    public Action<TData>? OnClose { get; set; }
    public Func<TData, string>? Label { get; set; }
    public Func<TData, bool>? IsOpen { get; set; }
    public Func<TData, ImGuiWindowFlags>? Flags { get; set; }

    protected override bool OnPreRender()
    {
        var label = Label?.Invoke(data) ?? "(missing label)";
        var isOpen = IsOpen?.Invoke(data) ?? true;
        var flags = Flags?.Invoke(data) ?? ImGuiWindowFlags.None;

        var wasOpen = isOpen;
        var result = ImGui.Begin(label, ref isOpen, flags);

        if (wasOpen && !isOpen)
            OnClose?.Invoke(data);

        return result;
    }

    protected override void OnPostRender(bool rendered)
    {
        ImGui.End();
    }
}

public class SameLine(params IEnumerable<Component> components) : Component(components)
{
    public static SameLine Create(Action<SameLine> config, params IEnumerable<Component> components)
    {
        var result = new SameLine(components);
        config(result);
        return result;
    }

    protected override void OnRenderChild(int index, Component component)
    {
        if (index > 0)
            ImGui.SameLine();
        base.OnRenderChild(index, component);
    }
}

public static class Disabled
{
    public static Disabled<TData> Create<TData>(TData data, params IEnumerable<Component> components) => 
        new(data, components);

    public static Disabled<TData> Create<TData>(TData data, Action<Disabled<TData>> config,
        params IEnumerable<Component> components)
    {
        var result = new Disabled<TData>(data, components);
        config(result);
        return result;
    }
}

public class Disabled<TData>(TData data, params IEnumerable<Component> components) : Component(components)
{
    public Func<TData, bool>? IsDisabled { get; set; }

    protected override bool OnPreRender()
    {
        var isDisabled = IsDisabled?.Invoke(data) ?? true;
        ImGui.BeginDisabled(isDisabled);
        return true;
    }

    protected override void OnPostRender(bool rendered)
    {
        ImGui.EndDisabled();
    }
}

public static class RadioButton
{
    public static RadioButton<TData> Create<TData>(TData data) => 
        new(data);

    public static RadioButton<TData> Create<TData>(TData data, Action<RadioButton<TData>> config)
    {
        var result = new RadioButton<TData>(data);
        config(result);
        return result;
    }
}

public class RadioButton<TData>(TData data) : Component
{
    public Action<TData>? OnSelect { get; set; }
    public Func<TData, bool>? IsSelected { get; set; }
    public Func<TData, string>? Label { get; set; }

    protected override void OnRender()
    {
        var label = Label?.Invoke(data) ?? "(missing label)";
        var isSelected = IsSelected?.Invoke(data) ?? false;

        if (ImGui.RadioButton(label, isSelected))
            OnSelect?.Invoke(data);
    }
}

public static class CheckBox
{
    public static CheckBox<TData> Create<TData>(TData data) => 
        new(data);

    public static CheckBox<TData> Create<TData>(TData data, Action<CheckBox<TData>> config)
    {
        var result = new CheckBox<TData>(data);
        config(result);
        return result;
    }
}

public class CheckBox<TData>(TData data) : Component
{
    public Action<TData>? OnSelect { get; set; }
    public Func<TData, bool>? IsSelected { get; set; }
    public Func<TData, string>? Label { get; set; }

    protected override void OnRender()
    {
        var label = Label?.Invoke(data) ?? "(missing label)";
        var isSelected = IsSelected?.Invoke(data) ?? false;

        if (ImGui.Checkbox(label, ref isSelected))
            OnSelect?.Invoke(data);
    }
}

public static class Combo
{
    public static Combo<TData> Create<TData>(TData data, params IEnumerable<Component> components) => 
        new(data, components);

    public static Combo<TData> Create<TData>(TData data, Action<Combo<TData>> config,
        params IEnumerable<Component> components)
    {
        var result = new Combo<TData>(data, components);
        config(result);
        return result;
    }
}

public class Combo<TData>(TData data, params IEnumerable<Component> components) : Component(components)
{
    public Func<TData, string>? Label { get; set; }
    public Func<TData, string>? Preview { get; set; }
    public Func<TData, ImGuiComboFlags>? Flags { get; set; }

    protected override bool OnPreRender()
    {
        var label = Label?.Invoke(data) ?? "(missing label)";
        var preview = Preview?.Invoke(data) ?? string.Empty;
        var flags = Flags?.Invoke(data) ?? ImGuiComboFlags.None;

        var labelLength = Encoding.UTF8.GetByteCount(label);
        var labelBytes = (stackalloc byte[labelLength + 1]);
        var labelBytesCount = Encoding.UTF8.GetBytes(label, labelBytes);
        labelBytes[labelBytesCount] = 0;

        var previewLength = Encoding.UTF8.GetByteCount(preview);
        var previewBytes = (stackalloc byte[previewLength + 1]);
        var previewBytesCount = Encoding.UTF8.GetBytes(preview, previewBytes);
        previewBytes[previewBytesCount] = 0;

        return ImGui.BeginCombo(label, preview, flags);
    }

    protected override void OnPostRender(bool rendered)
    {
        if (rendered)
            ImGui.EndCombo();
    }
}

public static class Selectable
{
    public static Selectable<TData> Create<TData>(TData data) => 
        new(data);

    public static Selectable<TData> Create<TData>(TData data, Action<Selectable<TData>> config)
    {
        var result = new Selectable<TData>(data);
        config(result);
        return result;
    }
}

public class Selectable<TData>(TData data) : Component
{
    public Action<TData>? OnClick { get; set; }
    public Func<TData, string>? Label { get; set; }
    public Func<TData, bool>? IsSelected { get; set; }
    public Func<TData, ImGuiSelectableFlags>? Flags { get; set; }
    public Func<TData, Vector2>? Size { get; set; }

    protected override void OnRender()
    {
        var label = Label?.Invoke(data) ?? "(missing label)";
        var isSelected = IsSelected?.Invoke(data) ?? false;
        var flags = Flags?.Invoke(data) ?? ImGuiSelectableFlags.None;
        var size = Size?.Invoke(data) ?? Vector2.Zero;

        if (size != Vector2.Zero
                ? ImGui.Selectable(label, isSelected, flags, size)
                : ImGui.Selectable(label, isSelected, flags))
            OnClick?.Invoke(data);
    }
}

public class Separator : Component
{
    public static Separator Create() => new();

    protected override void OnRender() => 
        ImGui.Separator();
}