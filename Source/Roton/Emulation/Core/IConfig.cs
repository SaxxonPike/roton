namespace Roton.Emulation.Core;

public interface IConfig
{
    AudioConfig Audio { get; }
    ContextConfig Context { get; }
    EngineConfig Engine { get; }
    JoystickConfig Joystick { get; }
}