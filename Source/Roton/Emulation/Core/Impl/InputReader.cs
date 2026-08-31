using System;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class InputReader(
    IConfig config,
    IJoystick joystick,
    IState state,
    IKeyboard keyboard,
    IElementList elements,
    IFacts facts,
    IAnsiKeyTransformer ansiKeyTransformer)
    : IInputReader
{
    private JoystickButtons _lastButtons;

    private EngineKeyCode ConvertKey(KeyPress keyPress)
    {
        var bytes = ansiKeyTransformer.GetBytes(keyPress);

        if (bytes.IsEmpty)
            return EngineKeyCode.None;

        if (bytes.Length > 1 && (bytes[0] == 0 || bytes[0] >= 0x80))
            return (EngineKeyCode)(bytes[1] | 0x80);

        return (EngineKeyCode)bytes[0];
    }

    private void ReadInputJoystick(bool isUiFocused)
    {
        if (config.DisableJoystick || !joystick.IsConnected)
            return;

        // This function does things a lot differently than the original engine,
        // mostly for convenience in controls.

        var x = 0f;
        var y = 0f;
        JoystickButtons buttons = 0;

        if (joystick.IsConnected)
        {
            x = joystick.X;
            y = joystick.Y;
            buttons = joystick.Buttons;
        }

        // Directional buttons should act like analog input for movement directions.

        if (buttons.HasFlag(JoystickButtons.Up))
            y = -1;
        else if (buttons.HasFlag(JoystickButtons.Down))
            y = 1;
        else if (buttons.HasFlag(JoystickButtons.Left))
            x = -1;
        else if (buttons.HasFlag(JoystickButtons.Right))
            x = 1;

        // Determine which direction "wins" based on how far the stick is held from center.

        var deadZone = config.JoystickDeadZone;
        var maxMagnitude = 0f;
        var finalKeyCode = (EngineKeyCode)0;

        if (x <= -deadZone & x <= -maxMagnitude)
        {
            state.KeyVector = Vector.West;
            maxMagnitude = Math.Max(maxMagnitude, Math.Abs(x));
            finalKeyCode = EngineKeyCode.Left;
        }

        if (x >= deadZone && x >= maxMagnitude)
        {
            state.KeyVector = Vector.East;
            maxMagnitude = Math.Max(maxMagnitude, Math.Abs(x));
            finalKeyCode = EngineKeyCode.Right;
        }

        if (y <= -deadZone && y <= -maxMagnitude)
        {
            state.KeyVector = Vector.North;
            maxMagnitude = Math.Max(maxMagnitude, Math.Abs(x));
            finalKeyCode = EngineKeyCode.Up;
        }

        if (y >= deadZone && y >= maxMagnitude)
        {
            state.KeyVector = Vector.South;
            maxMagnitude = Math.Max(maxMagnitude, Math.Abs(x));
            finalKeyCode = EngineKeyCode.Down;
        }

        if (finalKeyCode == EngineKeyCode.Left)
            buttons |= JoystickButtons.Left;
        else if (finalKeyCode == EngineKeyCode.Right)
            buttons |= JoystickButtons.Right;
        else if (finalKeyCode == EngineKeyCode.Up)
            buttons |= JoystickButtons.Up;
        else if (finalKeyCode == EngineKeyCode.Down)
            buttons |= JoystickButtons.Down;

        // The other buttons only activate when pressed and not every frame they're held.

        var singleButtons = buttons & ~_lastButtons;

        if (singleButtons.HasFlag(JoystickButtons.Left))
            state.KeyPressed = EngineKeyCode.Left;
        else if (singleButtons.HasFlag(JoystickButtons.Right))
            state.KeyPressed = EngineKeyCode.Right;
        else if (singleButtons.HasFlag(JoystickButtons.Up))
            state.KeyPressed = EngineKeyCode.Up;
        else if (singleButtons.HasFlag(JoystickButtons.Down))
            state.KeyPressed = EngineKeyCode.Down;

        // Process button actions.

        if (buttons.HasFlag(JoystickButtons.Ok))
        {
            if (isUiFocused)
            {
                state.KeyPressed = EngineKeyCode.Enter;
            }
            else
            {
                if (state.KeyPressed != EngineKeyCode.None)
                    state.KeyShift = true;
                else
                    state.KeyPressed = EngineKeyCode.Space;
            }
        }
        else if (buttons.HasFlag(JoystickButtons.Cancel))
        {
            if (isUiFocused)
                state.KeyPressed = EngineKeyCode.Escape;
        }
        else if (buttons.HasFlag(JoystickButtons.Shoot))
        {
            if (!isUiFocused)
                state.KeyShift = true;
        }

        if (isUiFocused && singleButtons.HasFlag(JoystickButtons.PageUp))
        {
            state.KeyPressed = EngineKeyCode.PageUp;
        }
        else if (isUiFocused && singleButtons.HasFlag(JoystickButtons.PageDown))
        {
            state.KeyPressed = EngineKeyCode.PageDown;
        }
        else if (singleButtons.HasFlag(JoystickButtons.Start))
        {
            // If on the title screen, Start will begin the game.
            // Otherwise, it will pause the game.

            if (state.PlayerElement == elements.MonitorId)
                state.KeyPressed = facts.StartGameKey;
            else
                state.KeyPressed = EngineKeyCode.P;
        }

        _lastButtons = buttons;
    }

    private void ReadInputKeyboard()
    {
        var mod = keyboard.GetMod();
        state.KeyShift = mod.HasFlag(KeyMod.Shift);
        state.KeyPressed = 0;
        state.KeyVector = Vector.Idle;

        if (!keyboard.KeyIsAvailable)
            return;

        var key = keyboard.GetKey();
        if (key is not { } keyValue || keyValue.Key == AnsiKey.None)
            return;

        state.KeyPressed = ConvertKey(keyValue);

        state.KeyVector = state.KeyPressed switch
        {
            EngineKeyCode.Left => Vector.West,
            EngineKeyCode.Right => Vector.East,
            EngineKeyCode.Up => Vector.North,
            EngineKeyCode.Down => Vector.South,
            _ => state.KeyVector
        };
    }

    public void Read(bool isUiFocused)
    {
        ReadInputKeyboard();
        if (state.KeyVector.IsZero())
            ReadInputJoystick(isUiFocused);
        if (state.KeyVector.IsNonZero())
            state.KeyLastVector = state.KeyVector;
    }
}