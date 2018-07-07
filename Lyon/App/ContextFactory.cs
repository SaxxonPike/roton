using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Core.Registration;
using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.SuperZZT;
using Roton.Emulation.ZZT;
using Roton.FileIo;
using Roton.Interface.Infrastructure;
using Roton.Interface.Video.Scenes.Composition;

namespace Lyon.App
{
    public class ContextFactory : IContextFactory
    {
        private readonly ILifetimeScope _rootLifetimeScope;

        public ContextFactory(ILifetimeScope rootLifetimeScope)
        {
            _rootLifetimeScope = rootLifetimeScope;
        }

        public IContext Create(ContextEngine contextEngine, IFileSystem fileSystem)
        {
            var scope = _rootLifetimeScope.BeginLifetimeScope(builder =>
            {
                builder.RegisterInstance(fileSystem).As<IFileSystem>().SingleInstance();
                
                builder.RegisterType<Context>().As<IContext>().SingleInstance();
                
                builder.Register(c => c.Resolve<IComposerProxy>().SceneComposer)
                    .As<ITerminal>();

                builder.Register(c => c.Resolve<IComposerProxy>().AudioComposer)
                    .As<ISpeaker>();

                switch (contextEngine)
                {
                    case ContextEngine.Zzt:
                        builder.RegisterType<Config>().As<IConfig>().SingleInstance().OnActivated(e =>
                        {
                            e.Instance.AmmoPerPickup = 5;
                            e.Instance.BuggyPassages = false;
                            e.Instance.ForestToFloor = false;
                            e.Instance.HealthPerGem = 1;
                            e.Instance.MultiMovement = false;
                            e.Instance.ScorePerGem = 10;
                        });
                        builder.RegisterType<ZztActors>().As<IActors>().SingleInstance();
                        builder.RegisterType<ZztAlerts>().As<IAlerts>().SingleInstance();
                        builder.RegisterType<ZztBoard>().As<IBoard>().SingleInstance();
                        builder.RegisterType<ZztColors>().As<IColors>().SingleInstance();
                        builder.RegisterType<ZztDrumBank>().As<IDrumBank>().SingleInstance();
                        builder.RegisterType<ZztElements>().As<IElements>().SingleInstance();
                        builder.Register(c => new ZztEngineResourceProvider(c.Resolve<IResourceService>()
                                .GetResource(typeof(IEngineResourceProvider).Assembly)))
                            .As<IEngineResourceProvider>()
                            .SingleInstance();
                        builder.RegisterType<ZztFlags>().As<IFlags>().SingleInstance();
                        builder.RegisterType<ZztGameSerializer>().As<IGameSerializer>().SingleInstance();
                        builder.RegisterType<ZztHud>().As<IHud>().SingleInstance();
                        builder.RegisterType<ZztMessage>().As<IMessage>().SingleInstance();
                        builder.RegisterType<Sounds>().As<ISounds>().SingleInstance();
                        builder.RegisterType<ZztState>().As<IState>().SingleInstance();
                        builder.RegisterType<ZztTiles>().As<ITiles>().SingleInstance();
                        builder.RegisterType<ZztWorld>().As<IWorld>().SingleInstance();
                        break;
                    case ContextEngine.SuperZzt:
                        builder.RegisterType<Config>().As<IConfig>().SingleInstance().OnActivated(e =>
                        {
                            e.Instance.AmmoPerPickup = 20;
                            e.Instance.BuggyPassages = true;
                            e.Instance.ForestToFloor = true;
                            e.Instance.HealthPerGem = 10;
                            e.Instance.MultiMovement = true;
                            e.Instance.ScorePerGem = 10;
                        });
                        builder.RegisterType<SuperZztActors>().As<IActors>().SingleInstance();
                        builder.RegisterType<SuperZztAlerts>().As<IAlerts>().SingleInstance();
                        builder.RegisterType<SuperZztBoard>().As<IBoard>().SingleInstance();
                        builder.RegisterType<SuperZztColors>().As<IColors>().SingleInstance();
                        builder.RegisterType<SuperZztDrumBank>().As<IDrumBank>().SingleInstance();
                        builder.RegisterType<SuperZztElements>().As<IElements>().SingleInstance();
                        builder.Register(c => new SuperZztEngineResourceProvider(c.Resolve<IResourceService>()
                                .GetResource(typeof(IEngineResourceProvider).Assembly)))
                            .As<IEngineResourceProvider>()
                            .SingleInstance();
                        builder.RegisterType<SuperZztFlags>().As<IFlags>().SingleInstance();
                        builder.RegisterType<SuperZztGameSerializer>().As<IGameSerializer>().SingleInstance();
                        builder.RegisterType<SuperZztHud>().As<IHud>().SingleInstance();
                        builder.RegisterType<SuperZztMessage>().As<IMessage>().SingleInstance();
                        builder.RegisterType<SuperZztSounds>().As<ISounds>().SingleInstance();
                        builder.RegisterType<SuperZztState>().As<IState>().SingleInstance();
                        builder.RegisterType<SuperZztTiles>().As<ITiles>().SingleInstance();
                        builder.RegisterType<SuperZztWorld>().As<IWorld>().SingleInstance();
                        break;
                }
            });

            return scope.Resolve<IContext>();
        }
    }
}