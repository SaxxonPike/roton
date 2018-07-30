using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using AutoFixture;
using AutoFixture.Dsl;
using FluentAssertions;
using Lyon.Autofac;
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
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Items;
using Roton.Emulation.Targets;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;
using Random = System.Random;

namespace Roton.Test.Infrastructure
{
    public abstract class ContextBaseTestFixture
    {
        protected ContextBaseTestFixture(Context context)
        {
            Context = context;
        }

        protected Mock<IClockFactory> ClockFactoryMock { get; private set; }
        protected FixedFileSystem FileSystem { get; private set; }
        protected Config Config { get; private set; }
        protected Mock<ITerminal> TerminalMock { get; private set; }
        protected TestKeyboard Keyboard { get; private set; }
        protected Mock<ISpeaker> SpeakerMock { get; private set; }

        private Random Rand { get; } = new Random();
        private Fixture Fixture { get; } = new Fixture();

        protected IEngine Engine { get; private set; }
        protected IActors Actors => Engine.Actors;
        protected IAlerts Alerts => Engine.Alerts;
        protected IBoard Board => Engine.Board;
        protected ICheatList CheatList => Engine.CheatList;
        protected IColors Colors => Engine.Colors;
        protected ICommandList CommandList => Engine.CommandList;
        protected IConditionList ConditionList => Engine.ConditionList;
        protected IDirectionList DirectionList => Engine.DirectionList;
        protected IElementList ElementList => Engine.ElementList;
        protected IFacts Facts => Engine.Facts;
        protected IHeap Heap => Engine.Heap;
        protected IHud Hud => Engine.Hud;
        protected IItemList ItemList => Engine.ItemList;
        protected IMemory Memory => Engine.Memory;
        protected IParser Parser => Engine.Parser;
        protected IActor Player => Engine.Player;
        protected IRandomizer Random => Engine.Random;
        protected ISounds Sounds => Engine.Sounds;
        protected IState State => Engine.State;
        protected ITargetList TargetList => Engine.TargetList;
        protected ITiles Tiles => Engine.Tiles;
        protected IWorld World => Engine.World;

        protected IEnumerable<string> FullMessage => Engine.GetMessageLines();
        protected IEnumerable<string> Message => FullMessage.Where(m => m != string.Empty).ToArray();

        protected void Step() => Engine.StepOnce();

        protected void StepAllKeys()
        {
            while (State.KeyPressed != 0 || Keyboard.KeyIsAvailable)
                Step();
        }
        
        [SetUp]
        public void __SetUpContext()
        {
            // Test dependencies
            FileSystem = new FixedFileSystem(true);
            Config = new Config();
            TerminalMock = new Mock<ITerminal>();
            Keyboard = new TestKeyboard();
            SpeakerMock = new Mock<ISpeaker>();
            ClockFactoryMock = new Mock<IClockFactory>();

            // Outer container
            var builder = new ContainerBuilder();
            builder.RegisterModule(new RotonModule(Context));
            builder.Register(c => FileSystem)
                .As<IFileSystem>()
                .SingleInstance();
            builder.Register(c => TerminalMock.Object)
                .As<ITerminal>()
                .SingleInstance();
            builder.Register(c => Keyboard)
                .As<IKeyboard>()
                .SingleInstance();
            builder.Register(c => SpeakerMock.Object)
                .As<ISpeaker>()
                .SingleInstance();
            builder.RegisterType<AssemblyResourceService>()
                .As<IAssemblyResourceService>()
                .SingleInstance();
            builder.Register(c => ClockFactoryMock.Object)
                .As<IClockFactory>()
                .SingleInstance();
            builder.Register(c => Config)
                .As<IConfig>()
                .SingleInstance();
            var container = builder.Build();

            // Inner container
            Engine = container.Resolve<IEngine>();

            // Preconfiguration
            Engine.ClearWorld();
            State.AboutShown = true;
            State.Init = false;
        }

        protected Context Context { get; }

        protected void MovePlayerTo(int x, int y) => MoveActorTo(0, x, y);

        protected void MoveActorTo(int index, int x, int y) => Engine.MoveActor(index, new Location(x, y));

        protected void PlotTo(int x, int y, int id, int? color = null) =>
            Tiles[new Location(x, y)].CopyFrom(new Tile(id, color ?? RandomInt(0x00, 0xFF)));
        
        protected int SpawnTo(int x, int y, int id, int? color = null)
        {
            Engine.SpawnActor(new Location(x, y), new Tile(id, color ?? ElementList[id].Color), ElementList[id].Cycle,
                State.DefaultActor);
            return ActorIndexAt(x, y);
        }

        protected void SetActorCode(int index, params string[] code)
        {
            var codeBytes = string.Join(new string('\xD', 1), code).ToBytes();
            var pointer = Heap.Allocate(codeBytes);
            Actors[index].Pointer = pointer;
            Actors[index].Length = codeBytes.Length;
        }

        protected ITile TileAt(int x, int y) => Tiles[new Location(x, y)];

        protected void Type(AnsiKey ekc) => Keyboard.Press(new KeyPress {Key = ekc});

        protected int ActorIndexAt(int x, int y) => Engine.ActorIndexAt(new Location(x, y));

        protected IActor ActorAt(int x, int y) => Engine.ActorAt(new Location(x, y));

        protected int RandomInt(int min, int max) => Rand.Next(min, max + 1);

        protected T Create<T>() => Fixture.Create<T>();

        protected IEnumerable<T> CreateMany<T>() => Fixture.CreateMany<T>();
        
        protected IEnumerable<T> CreateMany<T>(int count) => Fixture.CreateMany<T>(count);

        protected ICustomizationComposer<T> Build<T>() => Fixture.Build<T>();
    }
}