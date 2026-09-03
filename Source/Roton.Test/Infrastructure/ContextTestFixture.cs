using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Lyon;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Roton.Emulation.Cheats;
using Roton.Emulation.Colors;
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
using Roton.Infrastructure.Impl;
using Random = System.Random;

namespace Roton.Test.Infrastructure;

public abstract class ContextTestFixture(Context context) : BaseTestFixture
{
    protected Mock<IClock> ClockMock { get; private set; } = null!;
    protected IFileSystem FileSystem { get; private set; } = null!;
    protected Config Config { get; private set; } = null!;
    protected TestTerminal Terminal { get; private set; } = null!;
    protected TestKeyboard Keyboard { get; private set; } = null!;
    protected TestJoystick Joystick { get; private set; } = null!;
    protected Mock<ISpeaker> SpeakerMock { get; private set; } = null!;
    protected ITracer Tracer { get; private set; } = null!;

    private Random Rand { get; } = new();

    protected IEngine Engine { get; private set; } = null!;
    protected IActorList Actors { get; private set; } = null!;
    protected IAlerts Alerts { get; private set; } = null!;
    protected IBoard Board { get; private set; } = null!;
    protected IBroadcaster Broadcaster { get; private set; } = null!;
    protected ICheatList Cheats { get; private set; } = null!;
    protected IColorList Colors { get; private set; } = null!;
    protected ICommandList Commands { get; private set; } = null!;
    protected IConditionList Conditions { get; private set; } = null!;
    protected IDirectionList Directions { get; private set; } = null!;
    protected IElementList Elements { get; private set; } = null!;
    protected IExits Exits { get; private set; } = null!;
    protected IFacts Facts { get; private set; } = null!;
    protected ICodeHeap Heap { get; private set; } = null!;
    protected IHud Hud { get; private set; } = null!;
    protected IItemList Items { get; private set; } = null!;
    protected IMemory Memory { get; private set; } = null!;
    protected IMessageHandler MessageHandler { get; private set; } = null!;
    protected IMover Mover { get; private set; } = null!;
    protected IParser Parser { get; private set; } = null!;
    protected IActor Player => Actors[0];
    protected IRandomizer Random { get; private set; } = null!;
    protected ISpawner Spawner { get; private set; } = null!;
    protected ISounds Sounds { get; private set; } = null!;
    protected IState State { get; private set; } = null!;
    protected ITargetList Targets { get; private set; } = null!;
    protected ITiles Tiles { get; private set; } = null!;
    protected IWorld World { get; private set; } = null!;
    protected IGameSerializer GameSerializer { get; private set; } = null!;
    protected IWorldManager WorldManager { get; private set; } = null!;
    protected ISoundPlayer SoundPlayer { get; private set; } = null!;

    protected IEnumerable<string> FullMessage => MessageHandler.GetMessageLines();
    protected IEnumerable<string> Message => [.. FullMessage.Where(m => m != string.Empty)];

    protected void TouchActor(int actorIndex)
    {
        Broadcaster.BroadcastLabel(-actorIndex, Facts.TouchLabel, false);
    }

    protected void UnpackBoardResource(string path)
    {
        GameSerializer.UnpackBoard(Tiles, GameSerializer.LoadBoardData(GetResource(path)));
    }

    protected void Step()
    {
        Engine.StepOnce();
    }

    protected void Step(int count)
    {
        for (var i = 0; i < count; i++)
            Engine.StepOnce();
    }

    protected void DumpActorCode()
    {
        for (var i = 0; i < Actors.Count; i++)
        {
            var actor = Actors[i];
            if (actor.Pointer == 0)
                continue;

            TestContext.Out.WriteLine($"Actor {i} code:");
            var code = actor.Code.ToString();
            var reader = new StringReader(code);
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                    break;
                TestContext.Out.WriteLine($"   |{line}");
            }
        }
    }

    protected void StepAllKeys()
    {
        while (State.KeyPressed != 0 || Keyboard.KeyIsAvailable)
            Step();
    }

    protected void DisableTracer()
    {
        Tracer.Detach(TestContext.Out);
    }

    protected void EnableTracer()
    {
        Tracer.Attach(TestContext.Out);
    }

    [SetUp]
    public void __SetUpContext()
    {
        // Test dependencies
        FileSystem = new FixedFileSystem(true);
        Config = new Config
        {
            // Fast mode is needed because otherwise WaitForTick will wait for a message
            // from a thread that doesn't run during testing, leading to infinite loops.
            FastMode = true,
            // These need to be nonzero to prevent division by zero in HsecToTicks.
            MasterClockNumerator = 1,
            MasterClockDenominator = 1
        };
        Terminal = (TestTerminal)Inject<ITerminal>(new TestTerminal());
        Keyboard = (TestKeyboard)Inject<IKeyboard>(new TestKeyboard());
        Joystick = (TestJoystick)Inject<IJoystick>(new TestJoystick());
        SpeakerMock = Freeze<ISpeaker>();
        ClockMock = Freeze<IClock>();
        Tracer = new Tracer();
        EnableTracer();

        var services = new ServiceCollection();
        Assembly[] additionalAssemblies = [typeof(ContextTestFixture).Assembly];
        services.AddRoton(Context, additionalAssemblies);
        services.AddSingleton<IFileSystem>(FileSystem);
        services.AddSingleton<ITerminal>(Terminal);
        services.AddSingleton<IKeyboard>(Keyboard);
        services.AddSingleton<IJoystick>(Joystick);
        services.AddSingleton(SpeakerMock.Object);
        services.AddSingleton(ClockMock.Object);
        services.AddSingleton<IAssemblyResourceService, AssemblyResourceService>();
        services.AddSingleton<IConfig>(Config);
        services.AddSingleton(Tracer);

        var container = services.BuildServiceProvider();
        Engine = container.GetRequiredService<IEngine>();
        Actors = container.GetRequiredService<IActorList>();
        Alerts = container.GetRequiredService<IAlerts>();
        Board = container.GetRequiredService<IBoard>();
        Cheats = container.GetRequiredService<ICheatList>();
        Colors = container.GetRequiredService<IColorList>();
        Commands = container.GetRequiredService<ICommandList>();
        Conditions = container.GetRequiredService<IConditionList>();
        Directions = container.GetRequiredService<IDirectionList>();
        Elements = container.GetRequiredService<IElementList>();
        Exits = container.GetRequiredService<IExits>();
        Facts = container.GetRequiredService<IFacts>();
        Heap = container.GetRequiredService<ICodeHeap>();
        Hud = container.GetRequiredService<IHud>();
        Items = container.GetRequiredService<IItemList>();
        Memory = container.GetRequiredService<IMemory>();
        MessageHandler = container.GetRequiredService<IMessageHandler>();
        Mover = container.GetRequiredService<IMover>();
        Parser = container.GetRequiredService<IParser>();
        Random = container.GetRequiredService<IRandomizer>();
        Sounds = container.GetRequiredService<ISounds>();
        Spawner = container.GetRequiredService<ISpawner>();
        State = container.GetRequiredService<IState>();
        Targets = container.GetRequiredService<ITargetList>();
        Tiles = container.GetRequiredService<ITiles>();
        World = container.GetRequiredService<IWorld>();
        GameSerializer = container.GetRequiredService<IGameSerializer>();
        WorldManager = container.GetRequiredService<IWorldManager>();
        SoundPlayer = container.GetRequiredService<ISoundPlayer>();

        // Preconfiguration
        WorldManager.ClearWorld();
        State.AboutShown = true;
        State.Init = false;
        State.PlayerElement = Elements.PlayerId;
    }

    [TearDown]
    public void __TearDownContext()
    {
    }

    protected Context Context { get; } = context;

    protected void MovePlayerTo(int x, int y) =>
        MoveActorTo(0, x, y);

    protected void MoveActorTo(int index, int x, int y) => 
        Mover.MoveActor(index, new Location(x, y));

    protected void FaceActor(int index, Vector vector) =>
        Actors[index].Vector = vector;

    protected void PlotTo(int x, int y, int id, int? color = null) =>
        Tiles[new Location(x, y)] = (new Tile(id, color ?? RandomInt(0x00, 0xFF)));

    protected int SpawnTo(int x, int y, int id, int? color = null)
    {
        Spawner.SpawnActor(new Location(x, y), new Tile(id, color ?? Elements[id].Color), Elements[id].Cycle,
            State.DefaultActor);
        return ActorIndexAt(x, y);
    }

    protected void SetActorCode(int index, params string[] code)
    {
        var codeBlock = string.Join(new string('\xD', 1), code);
        var pointer = Heap.Allocate(codeBlock);
        Actors[index].Pointer = pointer;
        Actors[index].Length = codeBlock.Length;
    }

    protected ref Tile TileAt(int x, int y) =>
        ref Tiles[new Location(x, y)];

    protected ref Tile TileAt(Location xy) =>
        ref Tiles[xy];

    protected void Type(AnsiKey key, KeyMod mod = 0) =>
        Keyboard.Press(new KeyPress
        (
            key: key,
            mod: mod
        ));

    protected void Type(string text)
    {
        foreach (var c in text)
        {
            switch (c)
            {
                case 'A': Type(AnsiKey.A, KeyMod.Shift); break;
                case 'B': Type(AnsiKey.B, KeyMod.Shift); break;
                case 'C': Type(AnsiKey.C, KeyMod.Shift); break;
                case 'D': Type(AnsiKey.D, KeyMod.Shift); break;
                case 'E': Type(AnsiKey.E, KeyMod.Shift); break;
                case 'F': Type(AnsiKey.F, KeyMod.Shift); break;
                case 'G': Type(AnsiKey.G, KeyMod.Shift); break;
                case 'H': Type(AnsiKey.H, KeyMod.Shift); break;
                case 'I': Type(AnsiKey.I, KeyMod.Shift); break;
                case 'J': Type(AnsiKey.J, KeyMod.Shift); break;
                case 'K': Type(AnsiKey.K, KeyMod.Shift); break;
                case 'L': Type(AnsiKey.L, KeyMod.Shift); break;
                case 'M': Type(AnsiKey.M, KeyMod.Shift); break;
                case 'N': Type(AnsiKey.N, KeyMod.Shift); break;
                case 'O': Type(AnsiKey.O, KeyMod.Shift); break;
                case 'P': Type(AnsiKey.P, KeyMod.Shift); break;
                case 'Q': Type(AnsiKey.Q, KeyMod.Shift); break;
                case 'R': Type(AnsiKey.R, KeyMod.Shift); break;
                case 'S': Type(AnsiKey.S, KeyMod.Shift); break;
                case 'T': Type(AnsiKey.T, KeyMod.Shift); break;
                case 'U': Type(AnsiKey.U, KeyMod.Shift); break;
                case 'V': Type(AnsiKey.V, KeyMod.Shift); break;
                case 'W': Type(AnsiKey.W, KeyMod.Shift); break;
                case 'X': Type(AnsiKey.X, KeyMod.Shift); break;
                case 'Y': Type(AnsiKey.Y, KeyMod.Shift); break;
                case 'Z': Type(AnsiKey.Z, KeyMod.Shift); break;
                case 'a': Type(AnsiKey.A); break;
                case 'b': Type(AnsiKey.B); break;
                case 'c': Type(AnsiKey.C); break;
                case 'd': Type(AnsiKey.D); break;
                case 'e': Type(AnsiKey.E); break;
                case 'f': Type(AnsiKey.F); break;
                case 'g': Type(AnsiKey.G); break;
                case 'h': Type(AnsiKey.H); break;
                case 'i': Type(AnsiKey.I); break;
                case 'j': Type(AnsiKey.J); break;
                case 'k': Type(AnsiKey.K); break;
                case 'l': Type(AnsiKey.L); break;
                case 'm': Type(AnsiKey.M); break;
                case 'n': Type(AnsiKey.N); break;
                case 'o': Type(AnsiKey.O); break;
                case 'p': Type(AnsiKey.P); break;
                case 'q': Type(AnsiKey.Q); break;
                case 'r': Type(AnsiKey.R); break;
                case 's': Type(AnsiKey.S); break;
                case 't': Type(AnsiKey.T); break;
                case 'u': Type(AnsiKey.U); break;
                case 'v': Type(AnsiKey.V); break;
                case 'w': Type(AnsiKey.W); break;
                case 'x': Type(AnsiKey.X); break;
                case 'y': Type(AnsiKey.Y); break;
                case 'z': Type(AnsiKey.Z); break;
                case '0': Type(AnsiKey.D0); break;
                case '1': Type(AnsiKey.D1); break;
                case '2': Type(AnsiKey.D2); break;
                case '3': Type(AnsiKey.D3); break;
                case '4': Type(AnsiKey.D4); break;
                case '5': Type(AnsiKey.D5); break;
                case '6': Type(AnsiKey.D6); break;
                case '7': Type(AnsiKey.D7); break;
                case '8': Type(AnsiKey.D8); break;
                case '9': Type(AnsiKey.D9); break;
                case '!': Type(AnsiKey.D0, KeyMod.Shift); break;
                case '@': Type(AnsiKey.D1, KeyMod.Shift); break;
                case '#': Type(AnsiKey.D2, KeyMod.Shift); break;
                case '$': Type(AnsiKey.D3, KeyMod.Shift); break;
                case '%': Type(AnsiKey.D4, KeyMod.Shift); break;
                case '^': Type(AnsiKey.D5, KeyMod.Shift); break;
                case '&': Type(AnsiKey.D6, KeyMod.Shift); break;
                case '*': Type(AnsiKey.D7, KeyMod.Shift); break;
                case '(': Type(AnsiKey.D8, KeyMod.Shift); break;
                case ')': Type(AnsiKey.D9, KeyMod.Shift); break;
                case '-': Type(AnsiKey.Minus); break;
                case '_': Type(AnsiKey.Minus, KeyMod.Shift); break;
                case '=': Type(AnsiKey.Equals); break;
                case '+': Type(AnsiKey.Equals, KeyMod.Shift); break;
                case '?': Type(AnsiKey.Slash, KeyMod.Shift); break;
            }
        }
    }

    protected int ActorIndexAt(int x, int y) =>
        Actors.ActorIndexAt(new Location(x, y));

    protected IActor ActorAt(int x, int y) =>
        Actors.ActorAt(new Location(x, y));

    protected int RandomInt(int min, int max) =>
        Rand.Next(min, max + 1);

    protected void GoToBoard(int index)
    {
        while (State.BoardCount < index)
        {
            WorldManager.PackBoard();
            BoardIndex = State.BoardCount + 1;
            WorldManager.ClearBoard();
        }

        if (BoardIndex != index)
        {
            WorldManager.PackBoard();
            WorldManager.UnpackBoard(index);
        }
    }

    protected int BoardIndex
    {
        get => World.BoardIndex;
        set => World.BoardIndex = value;
    }

    protected int Ammo
    {
        get => World.Ammo;
        set => World.Ammo = value;
    }

    protected int Torches
    {
        get => World.Torches;
        set => World.Torches = value;
    }

    protected int TorchCycles
    {
        get => World.TorchCycles;
        set => World.Torches = value;
    }

    protected int EnergyCycles
    {
        get => World.EnergyCycles;
        set => World.EnergyCycles = value;
    }

    protected int Gems
    {
        get => World.Gems;
        set => World.Gems = value;
    }

    protected int Health
    {
        get => World.Health;
        set => World.Health = value;
    }

    protected int Score
    {
        get => World.Score;
        set => World.Score = value;
    }

    protected int Stones
    {
        get => World.Stones;
        set => World.Stones = value;
    }

    protected int TimePassed
    {
        get => World.TimePassed;
        set => World.TimePassed = value;
    }
    
    protected bool IsDark
    {
        get => Board.IsDark;
        set => Board.IsDark = value;
    }

    protected IFlags Flags => World.Flags;


    protected IKeyList Keys => World.Keys;

    protected bool GamePaused => State.GamePaused;

    /// <summary>
    /// Pass in ElementList IDs here. If the element is not present, the test will immediately
    /// be considered a pass (as it cannot be tested but all is expected.)
    /// </summary>
    protected void RequireElement(int elementId)
    {
        if (elementId < 0)
            Assert.Pass("Element does not exist in this context.");
    }

    protected void TypeCheat(string cheat)
    {
        Type(AnsiKey.Slash, KeyMod.Shift);
        Type(cheat);
        Type(AnsiKey.Enter);
        StepAllKeys();
    }

    protected void ClearBoard() =>
        WorldManager.ClearBoard();
    
    protected void PackBoard() =>
        WorldManager.PackBoard();
    
    protected void UnpackBoard(int index) =>
        WorldManager.UnpackBoard(index);
}