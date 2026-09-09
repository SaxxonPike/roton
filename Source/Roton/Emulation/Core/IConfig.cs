namespace Roton.Emulation.Core;

public interface IConfig
{
    AudioConfig Audio { get; }
    EngineConfig Engine { get; }
    JoystickConfig Joystick { get; }
    VideoConfig Video { get; }
}