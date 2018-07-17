using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutoFixture;
using AutoFixture.Dsl;
using Lyon.App;
using Moq;
using NUnit.Framework;
using OpenTK.Input;
using Roton.Emulation.Cheats;
using Roton.Emulation.Commands;
using Roton.Emulation.Conditions;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Directions;
using Roton.Emulation.Infrastructure;
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

        private Random Rand { get; } = new Random();
        private Fixture Fixture { get; } = new Fixture();

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
        protected IFacts Facts => Engine.Facts;
        protected IHeap Heap => Engine.Heap;
        protected IHud Hud => Engine.Hud;
        protected IItemList ItemList => Engine.ItemList;
        protected IMemory Memory => Engine.Memory;
        protected IParser Parser => Engine.Parser;
        protected IActor Player => Engine.Player;
        protected IRandom Random => Engine.Random;
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
            while (State.KeyPressed != 0 || Keyboard.HasKey)
                Step();
        }
        
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

        protected void Type(EngineKeyCode ekc) => Keyboard.Type(new KeyPress {Code = ekc});

        protected int ActorIndexAt(int x, int y) => Engine.ActorIndexAt(new Location(x, y));

        protected IActor ActorAt(int x, int y) => Engine.ActorAt(new Location(x, y));

        protected int RandomInt(int min, int max) => Rand.Next(min, max + 1);

        protected T Create<T>() => Fixture.Create<T>();

        protected IEnumerable<T> CreateMany<T>() => Fixture.CreateMany<T>();
        
        protected IEnumerable<T> CreateMany<T>(int count) => Fixture.CreateMany<T>(count);

        protected ICustomizationComposer<T> Build<T>() => Fixture.Build<T>();
    }
}