using Microsoft.Extensions.Options;
using Roton.Emulation.Core;

namespace Lyon.Common.App.Impl;

internal sealed class Config(
    IOptions<AudioConfig> audioConfig,
    IOptions<ContextConfig> contextConfig,
    IOptions<EngineConfig> engineConfig,
    IOptions<JoystickConfig> joystickConfig)
    : IConfig
{
    public AudioConfig Audio => audioConfig.Value;
    public ContextConfig Context => contextConfig.Value;
    public EngineConfig Engine => engineConfig.Value;
    public JoystickConfig Joystick => joystickConfig.Value;
}