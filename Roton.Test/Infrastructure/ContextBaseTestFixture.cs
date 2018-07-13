using Autofac;
using Lyon.App;
using Moq;
using NUnit.Framework;
using Roton.Emulation.Cheats;
using Roton.Emulation.Commands;
using Roton.Emulation.Conditions;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Directions;
using Roton.Emulation.Items;
using Roton.Emulation.Targets;
using Roton.Infrastructure;

namespace Roton.Test.Infrastructure
{
    public abstract class ContextBaseTestFixture
    {
        protected ContextBaseTestFixture(ContextEngine contextEngine)
        {
            ContextEngine = contextEngine;
        }
        
        protected IContext Context { get; private set; }
        protected TestFileSystem FileSystem { get; private set; }
        protected Config Config { get; private set; }
        protected Mock<ITerminal> TerminalMock { get; private set; }
        protected TestKeyboard Keyboard { get; private set; }
        protected Mock<ISpeaker> SpeakerMock { get; private set; }

        protected IEngine Engine => Context.Engine;
        protected IActors Actors => Engine.Actors;
        protected IAlerts Alerts => Engine.Alerts;
        protected IBoard Board => Engine.Board;
        protected ICheatList CheatList => Engine.CheatList;
        protected IColors Colors => Engine.Colors;
        protected ICommandList CommandList => Engine.CommandList;
        protected IConditionList ConditionList => Engine.ConditionList;
        protected IDirectionList DirectionList => Engine.DirectionList;
        protected IElementList ElementList => Engine.ElementList;
        protected IHud Hud => Engine.Hud;
        protected IItemList ItemList => Engine.ItemList;
        protected IParser Parser => Engine.Parser;
        protected IActor Player => Engine.Player;
        protected IRandom Random => Engine.Random;
        protected ISounds Sounds => Engine.Sounds;
        protected IState State => Engine.State;
        protected ITargetList TargetList => Engine.TargetList;
        protected ITiles Tiles => Engine.Tiles;
        protected IWorld World => Engine.World;

        protected void Step() => Engine.StepOnce();
        
        [SetUp]
        public void __SetUpContext()
        {
            // Test dependencies
            FileSystem = new TestFileSystem();
            Config = new Config();
            TerminalMock = new Mock<ITerminal>();
            Keyboard = new TestKeyboard();
            SpeakerMock = new Mock<ISpeaker>();
            
            // Outer container
            var builder = new ContainerBuilder();
            builder.Register(c => TerminalMock.Object).As<ITerminal>();
            builder.Register(c => Keyboard).As<IKeyboard>();
            builder.Register(c => SpeakerMock.Object).As<ISpeaker>();
            builder.RegisterType<AssemblyResourceService>().As<IAssemblyResourceService>();
            var container = builder.Build();
            
            // Inner container
            var contextFactory = new ContextFactory(container);
            Context = contextFactory.Create(ContextEngine, FileSystem, Config);
            
            // Preconfiguration
            Engine.ClearWorld();
            State.AboutShown = true;
            State.Init = false;
        }
        
        protected ContextEngine ContextEngine { get; }
    }
}