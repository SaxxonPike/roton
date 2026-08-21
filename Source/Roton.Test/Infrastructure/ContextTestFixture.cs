using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lyon;
using Microsoft.Extensions.DependencyInjection;
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
using Roton.Infrastructure.Impl;
using Random = System.Random;

namespace Roton.Test.Infrastructure;

public abstract class ContextTestFixture(Context context) : BaseTestFixture
{
    protected Mock<IClock> ClockMock { get; private set; } = null!;
    protected FixedFileSystem FileSystem { get; private set; } = null!;
    protected Config Config { get; private set; } = null!;
    protected TestTerminal Terminal { get; private set; } = null!;
    protected TestKeyboard Keyboard { get; private set; } = null!;
    protected TestJoystick Joystick { get; private set; } = null!;
    protected Mock<ISpeaker> SpeakerMock { get; private set; } = null!;
    protected ITracer Tracer { get; private set; } = null!;

    private Random Rand { get; } = new();

    protected IEngine Engine { get; private set; } = null!;
    protected IActorList Actors => Engine.Actors;
    protected IAlerts Alerts => Engine.Alerts;
    protected IBoard Board => Engine.Board;
    protected ICheatList Cheats => Engine.Cheats;
    protected IColorList Colors => Engine.Colors;
    protected ICommandList Commands => Engine.CommandList;
    protected IConditionList Conditions => Engine.Conditions;
    protected IDirectionList Directions => Engine.Directions;
    protected IElementList Elements => Engine.Elements;
    protected IFacts Facts => Engine.Facts;
    protected ICodeHeap Heap => Engine.Heap;
    protected IHud Hud => Engine.Hud;
    protected IItemList Items => Engine.ItemList;
    protected IMemory Memory => Engine.Memory;
    protected IParser Parser => Engine.Parser;
    protected IActor Player => Engine.Player;
    protected IRandomizer Random => Engine.Random;
    protected ISounds Sounds => Engine.Sounds;
    protected IState State => Engine.State;
    protected ITargetList Targets => Engine.TargetList;
    protected ITiles Tiles => Engine.Tiles;
    protected IGameSerializer GameSerializer => Engine.GameSerializer;

    protected IEnumerable<string> FullMessage => Engine.GetMessageLines();
    protected IEnumerable<string> Message => [.. FullMessage.Where(m => m != string.Empty)];

    protected void TouchActor(int actorIndex)
    {
        Engine.BroadcastLabel(-actorIndex, Facts.TouchLabel, false);
    }

    protected void UnpackBoardResource(string path)
    {
        GameSerializer.UnpackBoard(Engine.Tiles, GameSerializer.LoadBoardData(GetResource(path)));
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
        services.AddRoton(Context, typeof(ContextTestFixture).Assembly);
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

        // Preconfiguration
        Engine.ClearWorld();
        State.AboutShown = true;
        State.Init = false;
        State.PlayerElement = Elements.PlayerId;
    }

    [TearDown]
    public void __TearDownContext()
    {
    }

    protected Context Context { get; } = context;

    protected void MovePlayerTo(int x, int y) => MoveActorTo(0, x, y);

    protected void MoveActorTo(int index, int x, int y) => Engine.MoveActor(index, new Location(x, y));

    protected void FaceActor(int index, Vector vector) => Actors[index].Vector = vector;

    protected void PlotTo(int x, int y, int id, int? color = null) =>
        Tiles[new Location(x, y)] = (new Tile(id, color ?? RandomInt(0x00, 0xFF)));

    protected int SpawnTo(int x, int y, int id, int? color = null)
    {
        Engine.SpawnActor(new Location(x, y), new Tile(id, color ?? Elements[id].Color), Elements[id].Cycle,
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
        Engine.ActorIndexAt(new Location(x, y));

    protected IActor ActorAt(int x, int y) =>
        Engine.ActorAt(new Location(x, y));

    protected int RandomInt(int min, int max) =>
        Rand.Next(min, max + 1);

    protected void GoToBoard(int index)
    {
        while (State.BoardCount < index)
        {
            Engine.PackBoard();
            BoardIndex = State.BoardCount + 1;
            Engine.ClearBoard();
        }

        if (BoardIndex != index)
        {
            Engine.PackBoard();
            Engine.UnpackBoard(index);
        }
    }

    protected int BoardIndex
    {
        get => Engine.World.BoardIndex;
        set => Engine.World.BoardIndex = value;
    }

    protected int Ammo
    {
        get => Engine.World.Ammo;
        set => Engine.World.Ammo = value;
    }

    protected int Torches
    {
        get => Engine.World.Torches;
        set => Engine.World.Torches = value;
    }

    protected int TorchCycles
    {
        get => Engine.World.TorchCycles;
        set => Engine.World.Torches = value;
    }

    protected int EnergyCycles
    {
        get => Engine.World.EnergyCycles;
        set => Engine.World.EnergyCycles = value;
    }

    protected int Gems
    {
        get => Engine.World.Gems;
        set => Engine.World.Gems = value;
    }

    protected int Health
    {
        get => Engine.World.Health;
        set => Engine.World.Health = value;
    }

    protected int Score
    {
        get => Engine.World.Score;
        set => Engine.World.Score = value;
    }

    protected int Stones
    {
        get => Engine.World.Stones;
        set => Engine.World.Stones = value;
    }

    protected int TimePassed
    {
        get => Engine.World.TimePassed;
        set => Engine.World.TimePassed = value;
    }
    
    protected bool IsDark
    {
        get => Engine.Board.IsDark;
        set => Engine.Board.IsDark = value;
    }

    protected IFlags Flags => Engine.World.Flags;


    protected IKeyList Keys => Engine.World.Keys;

    protected bool GamePaused => Engine.State.GamePaused;

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
}