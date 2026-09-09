using Microsoft.Extensions.Options;
using Roton.Emulation.Core;

namespace Lyon.Common.App.Impl;

internal sealed class Config(
    IOptions<AudioConfig> audioConfig,
    IOptions<EngineConfig> engineConfig,
    IOptions<JoystickConfig> joystickConfig,
    IOptions<VideoConfig> videoConfig)
    : IConfig
{
    public AudioConfig Audio => audioConfig.Value;
    public EngineConfig Engine => engineConfig.Value;
    public JoystickConfig Joystick => joystickConfig.Value;
    public VideoConfig Video => videoConfig.Value;
}