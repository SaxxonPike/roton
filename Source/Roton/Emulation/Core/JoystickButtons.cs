using System;

namespace Roton.Emulation.Core;

[Flags]
public enum JoystickButtons
{
    Primary = 1 << 0,
    Secondary = 1 << 1
}