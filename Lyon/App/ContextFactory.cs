using Autofac;
using Roton.Emulation.Cheats;
using Roton.Emulation.Commands;
using Roton.Emulation.Conditions;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Directions;
using Roton.Emulation.Items;
using Roton.Emulation.SuperZzt;
using Roton.Emulation.Targets;
using Roton.Emulation.Temp;
using Roton.Emulation.Zzt;

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
                            e.Instance.BuggyPut = true;
                        });
                        builder.RegisterType<ZztActors>().As<IActors>().SingleInstance();
                        builder.RegisterType<ZztAlerts>().As<IAlerts>().SingleInstance();
                        builder.RegisterType<ZztBoard>().As<IBoard>().SingleInstance();
                        builder.RegisterType<ZztCheatList>().As<ICheatList>().SingleInstance();
                        builder.RegisterType<ZztColors>().As<IColors>().SingleInstance();
                        builder.RegisterType<ZztCommands>().As<ICommands>().SingleInstance();
                        builder.RegisterType<ZztConditionList>().As<IConditionList>().SingleInstance();
                        builder.RegisterType<ZztDirectionList>().As<IDirectionList>().SingleInstance();
                        builder.RegisterType<ZztDrumBank>().As<IDrumBank>().SingleInstance();
                        builder.RegisterType<ZztElementList>().As<IElementList>().SingleInstance();
                        builder.Register(c => new ZztEngineResourceProvider(c.Resolve<IResourceService>()
                                .GetResource(typeof(IEngineResourceProvider).Assembly)))
                            .As<IEngineResourceProvider>()
                            .SingleInstance();
                        builder.RegisterType<ZztFeatures>().As<IFeatures>().SingleInstance();
                        builder.RegisterType<ZztFlags>().As<IFlags>().SingleInstance();
                        builder.RegisterType<ZztGameSerializer>().As<IGameSerializer>().SingleInstance();
                        builder.RegisterType<ZztHud>().As<IHud>().SingleInstance();
                        builder.RegisterType<ZztItemList>().As<IItemList>().SingleInstance();
                        builder.RegisterType<ZztKeyList>().As<IKeyList>().SingleInstance();
                        builder.RegisterType<Sounds>().As<ISounds>().SingleInstance();
                        builder.RegisterType<ZztState>().As<IState>().SingleInstance();
                        builder.RegisterType<ZztTargetList>().As<ITargetList>().SingleInstance();
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
                            e.Instance.BuggyPut = false;
                        });
                        builder.RegisterType<SuperZztActors>().As<IActors>().SingleInstance();
                        builder.RegisterType<SuperZztAlerts>().As<IAlerts>().SingleInstance();
                        builder.RegisterType<SuperZztBoard>().As<IBoard>().SingleInstance();
                        builder.RegisterType<SuperZztCheatList>().As<ICheatList>().SingleInstance();
                        builder.RegisterType<SuperZztColors>().As<IColors>().SingleInstance();
                        builder.RegisterType<SuperZztCommands>().As<ICommands>().SingleInstance();
                        builder.RegisterType<SuperZztConditionList>().As<IConditionList>().SingleInstance();
                        builder.RegisterType<SuperZztDirectionList>().As<IDirectionList>().SingleInstance();
                        builder.RegisterType<SuperZztDrumBank>().As<IDrumBank>().SingleInstance();
                        builder.RegisterType<SuperZztElementList>().As<IElementList>().SingleInstance();
                        builder.Register(c => new SuperZztEngineResourceProvider(c.Resolve<IResourceService>()
                                .GetResource(typeof(IEngineResourceProvider).Assembly)))
                            .As<IEngineResourceProvider>()
                            .SingleInstance();
                        builder.RegisterType<SuperZztFeatures>().As<IFeatures>().SingleInstance();
                        builder.RegisterType<SuperZztFlags>().As<IFlags>().SingleInstance();
                        builder.RegisterType<SuperZztGameSerializer>().As<IGameSerializer>().SingleInstance();
                        builder.RegisterType<SuperZztHud>().As<IHud>().SingleInstance();
                        builder.RegisterType<SuperZztItemList>().As<IItemList>().SingleInstance();
                        builder.RegisterType<SuperZztKeyList>().As<IKeyList>().SingleInstance();
                        builder.RegisterType<SuperZztSounds>().As<ISounds>().SingleInstance();
                        builder.RegisterType<SuperZztState>().As<IState>().SingleInstance();
                        builder.RegisterType<SuperZztTargetList>().As<ITargetList>().SingleInstance();
                        builder.RegisterType<SuperZztTiles>().As<ITiles>().SingleInstance();
                        builder.RegisterType<SuperZztWorld>().As<IWorld>().SingleInstance();
                        break;
                }
            });

            return scope.Resolve<IContext>();
        }
    }
}