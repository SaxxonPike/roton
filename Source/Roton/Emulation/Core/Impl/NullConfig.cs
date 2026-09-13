using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class NullConfig : IConfig
{
    public AudioConfig Audio { get; } = new();
    public EngineConfig Engine { get; } = new();
    public JoystickConfig Joystick { get; } = new();
    public VideoConfig Video { get; } = new();
}