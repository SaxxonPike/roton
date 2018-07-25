using Autofac;
using Lyon.App;
using Lyon.Presenters;
using Roton.Composers.Audio;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;

namespace Lyon.Autofac
{
    public class LyonModule : Module
    {
        private readonly string[] _args;

        public LyonModule(string[] args)
        {
            _args = args;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            builder.RegisterAssemblyTypes(typeof(ILauncher).Assembly)
                .Where(t => !t.IsAbstract && t.IsClass)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterInstance(new CommandLine {Args = _args})
                .As<ICommandLine>()
                .SingleInstance();
            
            builder.Register(c => c.Resolve<IFileSystemFactory>().Create(c.Resolve<IConfig>().HomePath))
                .As<IFileSystem>()
                .SingleInstance();

            builder.Register(c => c.Resolve<IAudioComposerFactory>().Get())
                .As<IAudioComposer>()
                .As<ISpeaker>()
                .SingleInstance()
                .OnActivated(x =>
                {
                    var presenter = x.Context.Resolve<IAudioPresenter>();
                    if (presenter != null)
                    {
                        x.Instance.BufferReady += (s, a) => presenter.Update(a.Data);
                        presenter.Start();

                        x.Instance.SampleRate = presenter.SampleRate;                        
                    }

                    var engine = x.Context.Resolve<IEngine>();
                    engine.Tick += (s, a) => x.Instance.Tick();
                });

            builder.Register(c => c.Resolve<ISceneComposerFactory>().Get())
                .As<ISceneComposer>()
                .As<ITerminal>()
                .SingleInstance();            
        }
    }
}