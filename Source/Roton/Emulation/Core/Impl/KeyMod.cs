using System;

namespace Roton.Emulation.Core.Impl;

[Flags]
public enum KeyMod
{
    Shift = 1 << 0,
    Control = 1 << 1,
    Alt = 1 << 2,
}