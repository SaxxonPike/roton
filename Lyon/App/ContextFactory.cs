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
                        builder.RegisterType<ZztActorList>().As<IActorList>().SingleInstance();
                        builder.RegisterType<ZztAlerts>().As<IAlerts>().SingleInstance();
                        builder.RegisterType<ZztBoard>().As<IBoard>().SingleInstance();
                        builder.RegisterType<ZztColorList>().As<IColorList>().SingleInstance();
                        builder.RegisterType<ZztDrumBank>().As<IDrumBank>().SingleInstance();
                        builder.RegisterType<ZztElementList>().As<IElementList>().SingleInstance();
                        builder.RegisterType<ZztEngine>().As<IEngine>().SingleInstance();
                        builder.RegisterType<ZztFlagList>().As<IFlagList>().SingleInstance();
                        builder.RegisterType<ZztGameSerializer>().As<IGameSerializer>().SingleInstance();
                        builder.RegisterType<ZztGrammar>().As<IGrammar>().SingleInstance();
                        builder.RegisterType<ZztHud>().As<IHud>().SingleInstance();
                        builder.RegisterType<ZztMessage>().As<IMessage>().SingleInstance();
                        builder.RegisterType<SoundSet>().As<ISoundSet>().SingleInstance();
                        builder.RegisterType<ZztState>().As<IState>().SingleInstance();
                        builder.RegisterType<ZztTileGrid>().As<ITileGrid>().SingleInstance();
                        builder.RegisterType<ZztWorld>().As<IWorld>().SingleInstance();
                        break;
                    case ContextEngine.SuperZzt:
                        builder.RegisterType<SuperZztActorList>().As<IActorList>().SingleInstance();
                        builder.RegisterType<SuperZztAlerts>().As<IAlerts>().SingleInstance();
                        builder.RegisterType<SuperZztBoard>().As<IBoard>().SingleInstance();
                        builder.RegisterType<SuperZztColorList>().As<IColorList>().SingleInstance();
                        builder.RegisterType<SuperZztDrumBank>().As<IDrumBank>().SingleInstance();
                        builder.RegisterType<SuperZztElementList>().As<IElementList>().SingleInstance();
                        builder.RegisterType<SuperZztEngine>().As<IEngine>().SingleInstance();
                        builder.RegisterType<SuperZztFlagList>().As<IFlagList>().SingleInstance();
                        builder.RegisterType<SuperZztGameSerializer>().As<IGameSerializer>().SingleInstance();
                        builder.RegisterType<SuperZztGrammar>().As<IGrammar>().SingleInstance();
                        builder.RegisterType<SuperZztHud>().As<IHud>().SingleInstance();
                        builder.RegisterType<SuperZztMessage>().As<IMessage>().SingleInstance();
                        builder.RegisterType<SuperZztSoundSet>().As<ISoundSet>().SingleInstance();
                        builder.RegisterType<SuperZztState>().As<IState>().SingleInstance();
                        builder.RegisterType<SuperZztTileGrid>().As<ITileGrid>().SingleInstance();
                        builder.RegisterType<SuperZztWorld>().As<IWorld>().SingleInstance();
                        break;
                }
            });
            
            return scope.Resolve<IContext>();
        }
    }
}