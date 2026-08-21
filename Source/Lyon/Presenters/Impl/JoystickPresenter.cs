using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Roton;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Lyon.Presenters.Impl;

[Context(Context.Startup)]
internal sealed class JoystickPresenter : Joystick, IJoystickPresenter, IDisposable
{
    private readonly Lock _deviceLock = new();
    private SDL_JoystickID _active;
    private readonly List<SDL_JoystickID> _precedence = [];
    private readonly HashSet<SDL_JoystickID> _connected = [];
    private readonly Dictionary<(SDL_JoystickID, JoystickAxis), float> _axes = [];
    private readonly HashSet<(SDL_JoystickID, JoystickButtons)> _buttons = [];

    public JoystickPresenter()
    {
        SDL_InitSubSystem(SDL_InitFlags.SDL_INIT_GAMEPAD);
    }
    
    public override float X =>
        _axes.GetValueOrDefault((_active, JoystickAxis.X), 0);

    public override float Y =>
        _axes.GetValueOrDefault((_active, JoystickAxis.Y), 0);

    public override JoystickButtons Buttons =>
        (_buttons.Contains((_active, JoystickButtons.Primary)) ? JoystickButtons.Primary : default) |
        (_buttons.Contains((_active, JoystickButtons.Secondary)) ? JoystickButtons.Secondary : default);

    public override bool IsConnected =>
        _active != default;

    private void UpdateActive()
    {
        foreach (var item in _precedence)
        {
            if (_connected.Contains(item))
            {
                _active = item;
                break;
            }
        }

        _active = default;
    }

    public void Connect(SDL_JoystickID id)
    {
        lock (_deviceLock)
        {
            if (!_precedence.Contains(id))
                _precedence.Add(id);
            _connected.Add(id);
            UpdateActive();
        }
    }

    public void Disconnect(SDL_JoystickID id)
    {
        lock (_deviceLock)
        {
            _connected.Remove(id);
            UpdateActive();

            var axesToRemove = _axes.Where(x => x.Key.Item1 == id).ToList();
            foreach (var item in axesToRemove)
                _axes.Remove(item.Key);

            var buttonsToRemove = _buttons.Where(x => x.Item1 == id).ToList();
            foreach (var item in buttonsToRemove)
                _buttons.Remove(item);
        }
    }

    public void UpdateAxis(SDL_JoystickID id, JoystickAxis axis, float value) =>
        _axes[(id, axis)] = value;

    public void UpdateButton(SDL_JoystickID id, JoystickButtons button, bool pressed)
    {
        if (!pressed)
            _buttons.Remove((id, button));
        else
            _buttons.Add((id, button));
    }

    public void Dispose()
    {
        SDL_QuitSubSystem(SDL_InitFlags.SDL_INIT_GAMEPAD);
    }
}