namespace Roton.Emulation.Core.Impl;

public abstract class Joystick : IJoystick
{
    public virtual bool IsConnected => false;
    public virtual float X => 0;
    public virtual float Y => 0;
    public virtual JoystickButtons Buttons => 0;
}