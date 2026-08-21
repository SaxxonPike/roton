using Autofac;
using Lyon.App;
using Roton.Composers.Audio;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Module = Autofac.Module;

namespace Lyon.Autofac;

public sealed class LyonModule : Module
{
    private readonly string[] _args;

    public LyonModule(string[] args)
    {
        _args = args;
    }

    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterInstance(new CommandLine { Args = _args })
            .As<ICommandLine>()
            .SingleInstance();

        builder.Register(c => c.Resolve<IFileSystemFactory>().Create(c.Resolve<IConfig>().HomePath ?? "."))
            .As<IFileSystem>()
            .AutoActivate()
            .SingleInstance();

        builder.Register(c => c.Resolve<IAudioComposerFactory>().Get())
            .As<IAudioComposer>()
            .As<ISpeaker>()
            .AutoActivate()
            .SingleInstance();

        builder.Register(c => c.Resolve<ISceneComposerFactory>().Get())
            .As<ISceneComposer>()
            .As<ITerminal>()
            .AutoActivate()
            .SingleInstance();
    }
}