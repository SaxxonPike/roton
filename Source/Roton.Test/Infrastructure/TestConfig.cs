using Roton.Emulation.Core;

namespace Roton.Test.Infrastructure;

public class TestConfig : IConfig
{
    public AudioConfig Audio { get; set; } = new();
    public EngineConfig Engine { get; set; } = new();
    public JoystickConfig Joystick { get; set; } = new();
    public VideoConfig Video { get; set; } = new();
}