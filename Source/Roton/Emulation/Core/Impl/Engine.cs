using System;
using System.Linq;
using System.Threading;
using Roton.Emulation.Actions;
using Roton.Emulation.Cheats;
using Roton.Emulation.Commands;
using Roton.Emulation.Conditions;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Directions;
using Roton.Emulation.Draws;
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Interactions;
using Roton.Emulation.Items;
using Roton.Emulation.Targets;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class Engine : IEngine, IDisposable
{
    private readonly IConfigFileService _configFileService;
    private readonly ISoundUnit _soundUnit;
    private readonly IWorldUnit _worldUnit;
    private readonly IBoardTime _boardTime;
    private readonly Func<bool> _waitForTickFastDelegate;
    private readonly Func<bool> _waitForTickNormalDelegate;

    private int _ticksToRun;
    private bool _step;
    private JoystickButtons _lastButtons;

    public Engine(IClock clock, IActorList actors, IAlerts alerts, IBoard board,
        IFileSystem fileSystem, IElementList elements,
        IInterpreter interpreter, IRandomizer randomizer, IKeyboard keyboard,
        ITiles tiles, ISounds sounds, ITimers timers, IParser parser,
        IConfig config, IConditionList conditions, IDirectionList directions,
        IColorList colors, ICheatList cheats, ICommandList commands, ITargetList targets,
        IFeatures features, IGameSerializer gameSerializer, IHud hud, IState state,
        IWorld world, IItemList items, IBoardList boards, IActionList actionList,
        IDrawList drawList, IInteractionList interactionList, IFacts facts, IMemory memory,
        ICodeHeap heap, IAnsiKeyTransformer ansiKeyTransformer, IScrollFormatter scrollFormatter,
        ISpeaker speaker, IDrumSoundList drumBank, IObjectMover objectMover,
        IMusicEncoder musicEncoder,
        IHighScoreListFactory highScoreListFactory, IConfigFileService configFileService,
        IFileDialog fileDialog, ITracer tracer, IEngineAccessor engineAccessor,
        IJoystick joystick, ISoundUnit soundUnit, IWorldUnit worldUnit, IBoardTime boardTime)
    {
        engineAccessor.Instance = this;

        Actors = actors;
        Alerts = alerts;
        Board = board;
        Clock = clock;
        Elements = elements;
        Interpreter = interpreter;
        Random = randomizer;
        Keyboard = keyboard;
        Tiles = tiles;
        Sounds = sounds;
        Timers = timers;
        Parser = parser;
        Config = config;
        Cheats = cheats;
        Features = features;
        Hud = hud;
        State = state;
        World = world;
        Boards = boards;
        ActionList = actionList;
        DrawList = drawList;
        InteractionList = interactionList;
        Facts = facts;
        Heap = heap;
        AnsiKeyTransformer = ansiKeyTransformer;
        Speaker = speaker;
        DrumSounds = drumBank;
        ObjectMover = objectMover;
        MusicEncoder = musicEncoder;
        HighScoreListFactory = highScoreListFactory;
        _configFileService = configFileService;
        _soundUnit = soundUnit;
        _worldUnit = worldUnit;
        _boardTime = boardTime;
        Tracer = tracer;
        Joystick = joystick;

        _waitForTickFastDelegate = WaitForTickFastCondition;
        _waitForTickNormalDelegate = WaitForTickNormalCondition;
    }

    private IJoystick Joystick { get; }

    private void ClockTick(object? sender, EventArgs args)
    {
        if (_ticksToRun < 3)
            _ticksToRun++;

        if (!State.GamePaused)
            _boardTime.Advance();

        if (!ThreadActive)
            Clock.Stop();
    }

    private IHighScoreListFactory HighScoreListFactory { get; }

    private IObjectMover ObjectMover { get; }

    public IMusicEncoder MusicEncoder { get; }

    private IClock Clock { get; }

    private IBoardList Boards { get; }

    private IFeatures Features { get; }

    private ISpeaker Speaker { get; }

    private IInterpreter Interpreter { get; }

    private IKeyboard Keyboard { get; }

    public ITimers Timers { get; }

    public IDrumSoundList DrumSounds { get; }

    private ITracer Tracer { get; }

    private Thread? Thread { get; set; }

    public bool ThreadActive => Thread != null || _step;

    public int MemoryUsage => Features.BaseMemoryUsage + Heap.Size + Boards.Sum(b => b.Data.Length);

    public void Cheat()
    {
        var cheatText = Hud.EnterCheat().UpCased();
        var clear = false;

        if (!ThreadActive)
            return;

        if (!string.IsNullOrEmpty(cheatText))
        {
            if (cheatText![0] == '-')
            {
                cheatText = cheatText.Substring(1);
                while (World.Flags.Contains(cheatText))
                    World.Flags.Remove(cheatText);
                clear = true;
            }
            else if (cheatText[0] == '+')
            {
                cheatText = cheatText.Substring(1);
                World.Flags.Add(cheatText);
            }
        }

        var cheat = Cheats.Get(cheatText);
        cheat?.Execute(cheatText, clear);
        Hud.UpdateStatus();

        _soundUnit.PlaySound(10, Sounds.Cheat);
    }

    public string GetHighScoreName(string fileName) => Features.GetHighScoreName(fileName);

    public void ShowHighScores()
    {
        var list = HighScoreListFactory.Load();
        Hud.ShowHighScores(list);
    }

    private IActionList ActionList { get; }

    public IActor ActorAt(Location location) =>
        Actors.ActorAt(location);

    public int ActorIndexAt(Location location) =>
        Actors.ActorIndexAt(location);

    public event EventHandler? Exited;
    public event EventHandler? Tick;

    private IActorList Actors { get; }

    public int Adjacent(Location location, int id) => Features.GetAdjacent(location, id);

    private IAlerts Alerts { get; }

    public void Attack(int index, Location location)
    {
        if (index == 0 && World.EnergyCycles > 0)
        {
            World.Score += ElementAt(location).Points;
            UpdateStatus();
        }
        else
        {
            Harm(index);
        }

        if (index > 0 && index <= State.ActIndex) State.ActIndex--;

        if (Tiles[location].Id == Elements.PlayerId && World.EnergyCycles > 0)
        {
            World.Score += ElementAt(Actors[index].Location).Points;
            UpdateStatus();
        }
        else
        {
            Destroy(location);
            _soundUnit.PlaySound(2, Sounds.EnemySuicide);
        }
    }

    private IBoard Board { get; }

    public bool BroadcastLabel(int sender, ReadOnlySpan<char> label, bool ignoreLock)
    {
        var ignoreSelfLock = false;
        var success = false;

        if (sender < 0)
        {
            ignoreSelfLock = true;
            sender = -sender;
        }

        var info = new SearchContext
        {
            Index = 0,
            Offset = 0
        };

        while (ExecuteLabel(sender, ref info, label, "\r:"))
        {
            if (!ActorIsLocked(info.Index) || ignoreLock || sender == info.Index && !ignoreSelfLock)
            {
                if (sender == info.Index)
                    success = true;

                Tracer.TraceBroadcast(sender, label, info.Index, ignoreLock, ignoreSelfLock);
                Actors[info.Index].Instruction = info.Offset;
                NotifyActorSentLabel(info.Index);
            }
        }

        return success;
    }

    private ICheatList Cheats { get; }

    public void CleanUpPassageMovement() => Features.CleanUpPassageMovement();

    public void ClearForest(Location location) => Features.ClearForest(location);

    private IConfig Config { get; }

    public void Convey(Location center, int direction)
    {
        int beginIndex;
        int endIndex;

        Span<Tile> surrounding = stackalloc Tile[8];

        if (direction == 1)
        {
            beginIndex = 0;
            endIndex = 8;
        }
        else
        {
            beginIndex = 7;
            endIndex = -1;
        }

        var pushable = true;
        for (var i = beginIndex; i != endIndex; i += direction)
        {
            surrounding[i] = Tiles[center + GetConveyorVector(i)];
            var element = Elements[surrounding[i].Id];
            if (element.Id == Elements.EmptyId)
                pushable = true;
            else if (!element.IsPushable)
                pushable = false;
        }

        for (var i = beginIndex; i != endIndex; i += direction)
        {
            var element = Elements[surrounding[i].Id];

            if (pushable)
            {
                if (element.IsPushable)
                {
                    var source = center + GetConveyorVector(i);
                    var target = center + GetConveyorVector((i + 8 - direction) % 8);
                    if (element.Cycle > -1)
                    {
                        ref var tile = ref Tiles[source];
                        var index = ActorIndexAt(source);
                        Tiles[source] = surrounding[i];
                        Tiles[target].Id = Elements.EmptyId;
                        MoveActor(index, target);
                        Tiles[source] = tile;
                    }
                    else
                    {
                        Tiles[target] = surrounding[i];
                        UpdateBoard(target);
                    }

                    if (!Elements[surrounding[(i + 8 + direction) % 8].Id].IsPushable)
                    {
                        Tiles[source].Id = Elements.EmptyId;
                        UpdateBoard(source);
                    }
                }
                else
                {
                    pushable = false;
                }
            }
            else
            {
                if (element.Id == Elements.EmptyId)
                    pushable = true;
            }
        }
    }

    public void Destroy(Location location)
    {
        var index = ActorIndexAt(location);
        if (index == -1)
            RemoveItem(location);
        else
            Harm(index);
    }

    public AnsiChar Draw(Location location)
    {
        if (Board.IsDark && !ElementAt(location).IsAlwaysVisible &&
            (World.TorchCycles <= 0 || Distance(Player.Location, location) >= Facts.TorchRadius) &&
            !State.EditorMode)
            return Facts.DarknessTile;

        ref var tile = ref Tiles[location];
        var element = Elements[tile.Id];
        var elementCount = Elements.Count;

        if (tile.Id == Elements.EmptyId)
            return Facts.EmptyTile;

        if (element.HasDrawCode)
            return DrawList.Get(tile.Id)?.Draw(location) ?? new AnsiChar(0x4F, 0x41);

        if (tile.Id < elementCount - 7) return new AnsiChar(element.Character, tile.Color);

        return tile.Id != elementCount - 1
            ? new AnsiChar(tile.Color, ((tile.Id - (elementCount - 8)) << 4) | 0x0F)
            : new AnsiChar(tile.Color, 0x0F);
    }

    private IDrawList DrawList { get; }

    public IElement ElementAt(Location location) => Elements[Tiles[location].Id];

    private IElementList Elements { get; }

    public void ExecuteCode(int index, ref Word instruction, string name)
    {
        var context = new OopContext(Actors)
        {
            Index = index,
            Name = name,
            PreviousInstruction = instruction
        };

        while (true)
        {
            if (instruction < 0)
                break;

            Tracer?.TraceOop(ref context, ref instruction);

            context.NextLine = true;
            context.PreviousInstruction = instruction;
            context.Command = ReadActorCodeByte(index, ref instruction);

            while (context.Command == ':')
            {
                Parser.DiscardLine(index, ref instruction);
                Tracer?.TraceOop(ref context, ref instruction);
                context.Command = ReadActorCodeByte(index, ref instruction);
            }

            switch (context.Command)
            {
                case '\'':
                case '@':
                    Parser.DiscardLine(index, ref instruction);
                    break;
                case '/':
                case '?':
                    if (context.Command == '/')
                        context.Repeat = true;

                    if (!Parser.TryEvalDirection(ref context, ref instruction, out var vector))
                    {
                        RaiseError(ref context, "Bad direction");
                        break;
                    }

                    ObjectMover.ExecuteDirection(ref context, vector);

                    if (ReadActorCodeByte(index, ref instruction) != '\r')
                        instruction--;
                    context.Moved = true;

                    break;
                case '#':
                    Interpreter.Execute(ref context, ref instruction);
                    break;
                case '\r':
                    if (context.HasMessage)
                        context.AddMessage(string.Empty);
                    break;
                case '\0':
                    context.Finished = true;
                    break;
                default:
                    context.AddMessage($"{context.Command}{Parser.ReadLine(context.Index, ref instruction)}");
                    break;
            }

            if (context.Finished ||
                context.Moved ||
                context.Repeat ||
                context.Died ||
                context.CommandsExecuted >= Facts.MaxOopCommands)
                break;
        }

        if (context.Repeat)
            instruction = context.PreviousInstruction;

        if (State.OopByte == 0)
            instruction = -1;

        if (context.HasMessage)
            ExecuteMessage(ref context);

        if (context.Died)
            CleanUpOop(ref context);
    }

    public void CleanUpOop(ref OopContext context) => Features.CleanUpOop(ref context);

    public bool ExecuteLabel(int sender, ref SearchContext search, ReadOnlySpan<char> term, ReadOnlySpan<char> prefix)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var label = term;
        var success = false;
        var split = label.IndexOf(':');
        ReadOnlySpan<char> target = null;

        if (split > 0)
        {
            target = label.Slice(0, split);
            label = label.Slice(split + 1);
            success = Parser.TryEvalTarget(sender, ref search, target);
        }
        else if (search.Index < sender)
        {
            label = term;
            search.Index = sender;
            split = 0;
            success = true;
        }

        while (success)
        {
            if (label.Equals(Facts.RestartLabel, StringComparison.OrdinalIgnoreCase))
            {
                search.Offset = 0;
            }
            else
            {
                prefix.CopyTo(buffer);
                label.CopyTo(buffer.Slice(prefix.Length));
                search.Offset = Parser.Search(search.Index, buffer.Slice(0, prefix.Length + label.Length));
                if (search.Offset < 0 && split > 0)
                {
                    success = Parser.TryEvalTarget(sender, ref search, target);
                    continue;
                }
            }

            success = search.Offset >= 0;
            break;
        }

        return success;
    }

    public bool ExecuteTransaction(ref OopContext context, ref Word instruction, bool take)
    {
        // Does the item exist?
        if (!Parser.TryEvalItem(ref context, ref instruction, out var item))
            return false;

        // Do we have a valid amount?
        var amount = Parser.ReadNumber(context.Index, ref context.Actor.Instruction);
        if (amount <= 0)
            return true;

        // Modify value if we are taking.
        if (take)
            State.OopNumber = -State.OopNumber;

        // Determine if the result will be in range.
        var pendingAmount = item!.Value + State.OopNumber;
        if ((pendingAmount & 0xFFFF) >= 0x8000)
            return true;

        // Successful transaction.
        item.Value = pendingAmount;
        return false;
    }

    private IFacts Facts { get; }

    private ICodeHeap Heap { get; }

    public void StepOnce()
    {
        _step = true;
        MainLoop(true);
        _step = false;
    }

    public string[] GetMessageLines() => Features.GetMessageLines();

    public void FadePurple()
    {
        FadeBoard(Facts.FadeTile);
        Hud.RedrawBoard();
    }

    public int GetColorMatchValue(int color) => Features.GetColorMatchValue(color);

    public bool FindTile(Tile kind, Location location)
    {
        var matchColor = GetColorMatchValue(kind.Color);

        location.X++;
        while (location.Y <= Tiles.Height)
        {
            while (location.X <= Tiles.Width)
            {
                ref var tile = ref Tiles[location];
                if (tile.Id == kind.Id)
                {
                    var foundColor = GetColorMatchValue(ColorMatch(Tiles[location]));
                    if (kind.Color == 0 || foundColor == matchColor)
                        return true;
                }

                location.X++;
            }

            location.X = 1;
            location.Y++;
        }

        return false;
    }

    public void ForcePlayerColor(int index) => Features.ForcePlayerColor(index);

    public Vector GetCardinalVector(int index) => new(State.Vector4[index], State.Vector4[index + 4]);

    public void HandlePlayerInput(IActor actor) => Features.HandlePlayerInput(actor);

    public void Harm(int index)
    {
        var actor = Actors[index];
        if (index == 0)
        {
            if (World.Health > 0)
            {
                World.Health -= Facts.HealthLostPerHit;
                UpdateStatus();
                SetMessage(Facts.ShortMessageDuration, Alerts.OuchMessage);
                Tiles[actor.Location].Color = (ElementAt(actor.Location).Color & 0x0F) | 0x70;

                if (World.Health > 0)
                {
                    World.TimePassed = 0;
                    if (Board.RestartOnZap)
                    {
                        _soundUnit.PlaySound(4, Sounds.TimeOut);
                        RemoveItem(actor.Location);
                        var oldLocation = actor.Location;
                        actor.Location = Board.Entrance;
                        UpdateRadius(oldLocation, 0);
                        UpdateRadius(actor.Location, 0);
                        State.GamePaused = true;
                    }

                    _soundUnit.PlaySound(4, Sounds.Ouch);
                }
                else
                {
                    _soundUnit.PlaySound(5, Sounds.GameOver);
                }
            }
        }
        else
        {
            var element = Tiles[actor.Location].Id;
            if (element == Elements.BulletId)
                _soundUnit.PlaySound(3, Sounds.BulletDie);
            else if (element != Elements.ObjectId) _soundUnit.PlaySound(3, Sounds.EnemyDie);

            RemoveActor(index);
        }
    }

    /// <summary>
    /// Converts hundredths of seconds to ticks.
    /// </summary>
    /// <param name="hsec">
    /// Duration in hundredths of seconds.
    /// </param>
    /// <returns>
    /// The equivalent number of ticks.
    /// </returns>
    private int HsecToTicks(int hsec) =>
        Math.Max(1, hsec * (Config.MasterClockDenominator / Config.MasterClockNumerator + 50) / 100);

    private IHud Hud { get; }

    private IInteractionList InteractionList { get; }

    public void LockActor(int index) => Features.LockActor(index);

    public void MoveActor(int index, Location target)
    {
        var actor = Actors[index];
        var sourceLocation = actor.Location;
        ref var sourceTile = ref Tiles[actor.Location];
        ref var targetTile = ref Tiles[target];
        var underTile = actor.UnderTile;
        var nextUnderTile = targetTile;

        if (targetTile.Id == Elements.EmptyId)
            targetTile = new Tile(sourceTile.Id, sourceTile.Color & 0x0F);
        else
            targetTile = new Tile(sourceTile.Id, (targetTile.Color & 0x70) | (sourceTile.Color & 0x0F));

        sourceTile = underTile;
        actor.Location = target;
        if (targetTile.Id == Elements.PlayerId)
            ForcePlayerColor(index);

        UpdateBoard(target);
        UpdateBoard(sourceLocation);
        actor.UnderTile = nextUnderTile;

        if (index == 0 && Board.IsDark)
        {
            var squareDistanceX = (target.X - sourceLocation.X).Square();
            var squareDistanceY = (target.Y - sourceLocation.Y).Square();
            if (squareDistanceX + squareDistanceY == 1)
            {
                for (var x = target.X - Facts.TorchDrawBoxVerticalSize;
                     x <= target.X + Facts.TorchDrawBoxVerticalSize;
                     x++)
                for (var y = target.Y - Facts.TorchDrawBoxHorizontalSize;
                     y <= target.Y + Facts.TorchDrawBoxHorizontalSize;
                     y++)
                {
                    var glowLocation = new Location(x, y);
                    if (glowLocation.X >= 1 && glowLocation.X <= Tiles.Width && glowLocation.Y >= 1 &&
                        glowLocation.Y <= Tiles.Height)
                        if ((Distance(sourceLocation, glowLocation) < Facts.TorchRadius) ^
                            (Distance(target, glowLocation) < Facts.TorchRadius))
                            UpdateBoard(glowLocation);
                }
            }
        }

        if (index == 0)
            Hud.UpdateCamera();
    }

    public void MoveActorOnRiver(int index)
    {
        var actor = Actors[index];
        var vector = new Vector();
        var underId = actor.UnderTile.Id;

        if (underId == Elements.RiverNId)
            vector = Vector.North;
        else if (underId == Elements.RiverSId)
            vector = Vector.South;
        else if (underId == Elements.RiverWId)
            vector = Vector.West;
        else if (underId == Elements.RiverEId)
            vector = Vector.East;

        if (vector.IsNonZero())
        {
            ref var actorTile = ref Tiles[actor.Location];
            if (actorTile.Id == Elements.PlayerId)
            {
                var targetLocation = actor.Location + vector;
                InteractionList.Get(Tiles[targetLocation].Id)?.Interact(targetLocation, 0, ref vector);
            }
        }

        if (vector.IsNonZero())
        {
            var target = actor.Location + vector;
            if (ElementAt(target).IsFloor)
                MoveActor(index, target);
        }
    }

    public void NotifyActorSentLabel(int index) => Features.NotifyActorSentLabel(index);

    private IParser Parser { get; }

    private IActor Player => Actors[0];

    public void PlotTile(Location location, Tile tile)
    {
        if (ElementAt(location).Id == Elements.PlayerId)
            return;

        var targetElement = Elements[tile.Id];
        ref var existingTile = ref Tiles[location];
        var targetColor = tile.Color;
        if (targetElement.Color >= 0xF0)
        {
            if (targetColor == 0)
                targetColor = existingTile.Color;
            if (targetColor == 0)
                targetColor = 0x0F;
            if (targetElement.Color == 0xFE)
                targetColor = ((targetColor - 8) << 4) + 0x0F;
        }
        else
        {
            targetColor = targetElement.Color;
        }

        if (targetElement.Id == existingTile.Id)
        {
            existingTile.Color = targetColor;
        }
        else
        {
            Destroy(location);
            if (targetElement.Cycle < 0)
                existingTile = new Tile(targetElement.Id, targetColor);
            else
                SpawnActor(location, new Tile(targetElement.Id, targetColor), targetElement.Cycle,
                    State.DefaultActor);
        }

        UpdateBoard(location);
    }

    public void Push(Location location, Vector vector)
    {
        ref var tile = ref Tiles[location];
        if (tile.Id == Elements.SliderEwId && vector.Y == 0 ||
            tile.Id == Elements.SliderNsId && vector.X == 0 ||
            Elements[tile.Id].IsPushable)
        {
            // this is here to prevent endless push loops
            // but doesn't exist in the original code
            if (vector.IsZero())
                throw Exceptions.PushStackOverflow;

            ref var furtherTile = ref Tiles[location + vector];
            if (furtherTile.Id == Elements.TransporterId)
                PushThroughTransporter(location, vector);
            else if (furtherTile.Id != Elements.EmptyId)
                Push(location + vector, vector);

            var furtherElement = Elements[furtherTile.Id];
            if (!furtherElement.IsFloor && furtherElement.IsDestructible && furtherTile.Id != Elements.PlayerId)
                Destroy(location + vector);

            furtherElement = Elements[furtherTile.Id];
            if (furtherElement.IsFloor)
                MoveTile(location, location + vector);
        }
    }

    public void PushThroughTransporter(Location location, Vector vector)
    {
        var actor = ActorAt(location + vector);

        if (actor.Vector == vector)
        {
            var search = actor.Location;
            var target = new Location();
            var ended = false;
            var success = true;

            while (!ended)
            {
                search += vector;
                var element = ElementAt(search);
                if (element.Id == Elements.BoardEdgeId)
                {
                    ended = true;
                }
                else
                {
                    if (success)
                    {
                        success = false;
                        if (!element.IsFloor)
                        {
                            Push(search, vector);
                            element = ElementAt(search);
                        }

                        if (element.IsFloor)
                        {
                            ended = true;
                            target = search;
                        }
                        else
                        {
                            target.X = 0;
                        }
                    }
                }

                if (element.Id == Elements.TransporterId)
                    if (ActorAt(search).Vector == -vector)
                        success = true;
            }

            if (target.X > 0)
            {
                MoveTile(actor.Location - vector, target);
                _soundUnit.PlaySound(3, Sounds.Transporter);
            }
        }
    }

    public void PutTile(Location location, Vector vector, Tile kind)
    {
        if (!Features.CanPutTile(location))
            return;

        if (location.X >= 1 && location.X <= Tiles.Width && location.Y >= 1 &&
            location.Y <= Tiles.Height)
        {
            if (!ElementAt(location).IsFloor) Push(location, vector);
            PlotTile(location, kind);
        }
    }

    public void RaiseError(ref OopContext context, ReadOnlySpan<char> error)
    {
        SetMessage(Facts.LongMessageDuration, Alerts.ErrorMessage(error));
        _soundUnit.PlaySound(5, Sounds.Error);
        Tracer.TraceError(ref context, error);
        Actors[context.Index].Instruction = -1;
    }

    private IRandomizer Random { get; }

    public void RemoveActor(int index)
    {
        var actor = Actors[index];
        var freeCode = actor.Length > 0 && actor.Pointer != 0;

        if (index < State.ActIndex)
            State.ActIndex--;

        Tiles[actor.Location] = actor.UnderTile;

        if (actor.Location.Y > 0)
            UpdateBoard(actor.Location);

        var pointer = actor.Pointer;

        for (var i = 1; i <= State.ActorCount; i++)
        {
            var a = Actors[i];
            if (a.Follower >= index)
            {
                if (a.Follower == index)
                    a.Follower = -1;
                else
                    a.Follower--;
            }

            if (a.Leader >= index)
            {
                if (a.Leader == index)
                    a.Leader = -1;
                else
                    a.Leader--;
            }

            if (freeCode && i != index && a.Pointer == pointer)
                freeCode = false;
        }

        if (freeCode)
        {
            Heap.Free(pointer);
            actor.Pointer = 0;
        }

        if (index < State.ActorCount)
            for (var i = index; i < State.ActorCount; i++)
                Actors[i].CopyFrom(Actors[i + 1]);

        State.ActorCount--;
    }

    public void RemoveItem(Location location) => Features.RemoveItem(location);

    public Vector Rnd()
    {
        var result = new Vector
        {
            X = Random.GetNext(3) - 1
        };

        result.Y = result.X == 0 ? (Random.GetNext(2) << 1) - 1 : 0;
        return result;
    }

    public Vector RndP(Vector vector) =>
        Random.GetNext(2) == 0
            ? vector.Clockwise()
            : vector.CounterClockwise();

    public Vector Seek(Location location)
    {
        var result = new Vector();
        if (Random.GetNext(2) == 0 || Player.Location.Y == location.Y)
            result.X = (Player.Location.X - location.X).Polarity();

        if (result.X == 0) result.Y = (Player.Location.Y - location.Y).Polarity();

        if (World.EnergyCycles > 0) result = -result;

        return result;
    }

    public void SetEditorMode()
    {
        InitializeElements(true);
        State.EditorMode = true;
    }

    public void SetGameMode()
    {
        InitializeElements(false);
        State.EditorMode = false;
    }

    public void SetMessage(int duration, IMessage message)
    {
        var index = ActorIndexAt(new Location(0, 0));
        if (index >= 0)
        {
            RemoveActor(index);
            Hud.UpdateBorder();
        }

        var topMessage = message.Text[0];
        var bottomMessage = message.Text.Count > 1 ? message.Text[1] : string.Empty;

        SpawnActor(new Location(0, 0), new Tile(Elements.MessengerId, 0), 1, State.DefaultActor);
        Actors[State.ActorCount].P2 = unchecked((byte)(duration / (State.GameWaitTime + 1)));
        State.Message = topMessage;
        State.Message2 = bottomMessage;
    }

    public void ShowHelp(string title, string filename) => Hud.ShowHelp(title, filename);

    public void ShowInGameHelp() => Features.ShowInGameHelp();

    public ISounds Sounds { get; }

    public void SpawnActor(Location location, Tile tile, int cycle, IActor? source)
    {
        // must reserve one actor for player, and one for messenger
        if (State.ActorCount < Actors.Capacity - 2)
        {
            State.ActorCount++;
            var actor = Actors[State.ActorCount];

            source ??= State.DefaultActor;

            actor.CopyFrom(source);
            actor.Location = location;
            actor.Cycle = cycle;
            actor.UnderTile = Tiles[location];
            actor.Instruction = 0;

            if (ElementAt(actor.Location).IsEditorFloor)
            {
                var newColor = Tiles[actor.Location].Color & 0x70;
                newColor |= tile.Color & 0x0F;
                Tiles[actor.Location].Color = newColor;
            }
            else
            {
                Tiles[actor.Location].Color = tile.Color;
            }

            Tiles[actor.Location].Id = tile.Id;
            if (actor.Location.Y > 0) UpdateBoard(actor.Location);
        }
    }

    public bool SpawnProjectile(int id, Location location, Vector vector, bool enemyOwned)
    {
        var target = location + vector;
        var element = ElementAt(target);

        if (element.IsFloor || element.Id == Elements.WaterId)
        {
            SpawnActor(target, new Tile(id, Elements[id].Color), 1, State.DefaultActor);
            var actor = Actors[State.ActorCount];
            actor.P1 = unchecked((byte)(enemyOwned ? 1 : 0));
            actor.Vector = vector;
            actor.P2 = 0x64;
            return true;
        }

        if (element.Id != Elements.BreakableId &&
            (!element.IsDestructible ||
             element.Id == Elements.PlayerId != enemyOwned ||
             World.EnergyCycles != 0))
            return false;

        Destroy(target);
        _soundUnit.PlaySound(2, Sounds.BulletDie);
        return true;
    }

    public void Start()
    {
        if (Thread == null)
        {
            _ticksToRun = 0;
            Thread = new Thread(StartMain);
            Thread.Start();
        }
    }

    private IState State { get; }

    public void Stop()
    {
        Thread = null;
    }

    private ITiles Tiles { get; }

    public bool TitleScreen => State.PlayerElement != Elements.PlayerId;

    public void UnlockActor(int index) => Features.UnlockActor(index);

    public void UpdateBoard(Location location) => DrawTile(location, Draw(location));

    public void UpdateRadius(Location location, RadiusMode mode)
    {
        var source = location;
        var left = source.X - 9;
        var right = source.X + 9;
        var top = source.Y - 6;
        var bottom = source.Y + 6;
        for (var x = left; x <= right; x++)
        for (var y = top; y <= bottom; y++)
            if (x >= 1 && x <= Tiles.Width && y >= 1 && y <= Tiles.Height)
            {
                var target = new Location(x, y);
                if (mode != RadiusMode.Update)
                    if (Distance(source, target) < Facts.TorchRadius)
                    {
                        var element = ElementAt(target);
                        if (mode == RadiusMode.Explode)
                        {
                            if (element.CanContainCode)
                            {
                                var actorIndex = ActorIndexAt(target);
                                if (actorIndex > 0) BroadcastLabel(-actorIndex, Facts.BombedLabel, false);
                            }

                            if (element.IsDestructible || element.Id == Elements.StarId) Destroy(target);

                            if (element.Id == Elements.EmptyId || element.Id == Elements.BreakableId)
                                Tiles[target] = new Tile(Elements.BreakableId, Random.GetNext(7) + 9);
                        }
                        else
                        {
                            if (Tiles[target].Id == Elements.BreakableId) Tiles[target].Id = Elements.EmptyId;
                        }
                    }

                UpdateBoard(target);
            }
    }

    public void UpdateStatus() => Hud.UpdateStatus();

    private void UpdateSound()
    {
        if (!State.SoundPlaying)
        {
            State.SoundBuffer.Clear();
            return;
        }

        if (State.SoundTicks <= 0)
        {
            if (State.SoundBuffer.Count > 0)
            {
                var sound = State.SoundBuffer.Dequeue();
                State.SoundTicks = sound.Duration << 2;
                if (sound.Note >= 0xF0)
                {
                    Speaker.PlayDrum(sound.Note - 0xF0);
                }
                else if (sound.Note > 0x00)
                {
                    var actualNote = (sound.Note & 0xF) + (sound.Note >> 4) * 12;
                    Speaker.PlayNote(actualNote);
                }
                else
                {
                    Speaker.StopNote();
                }
            }
            else
            {
                State.SoundPlaying = false;
                State.SoundPriority = 0;
                Speaker.StopNote();
            }
        }

        if (State.SoundPlaying)
            State.SoundTicks--;
    }

    private bool WaitForTickFastCondition()
    {
        if (_ticksToRun <= 0)
            return true;

        UpdateSound();
        Tick?.Invoke(this, EventArgs.Empty);
        _ticksToRun--;

        return false;
    }

    private bool WaitForTickNormalCondition() =>
        _ticksToRun > 0 || !ThreadActive;

    public void WaitForTick()
    {
        var isFast = State.GameWaitTime <= 0 && Config.FastMode;

        if (isFast)
        {
            SpinWait.SpinUntil(_waitForTickFastDelegate);
        }
        else
        {
            UpdateSound();

            Tick?.Invoke(this, EventArgs.Empty);

            SpinWait.SpinUntil(_waitForTickNormalDelegate);

            if (_ticksToRun > 0)
                _ticksToRun--;
        }
    }

    private IWorld World { get; }

    private bool ActorIsLocked(int index) => Features.IsActorLocked(index);

    private int ColorMatch(Tile tile)
    {
        var element = Elements[tile.Id];

        if (element.Color < 0xF0)
            return element.Color & 7;
        if (element.Color == 0xFE)
            return ((tile.Color >> 4) & 0x0F) + 8;
        return tile.Color & 0x0F;
    }

    private static int Distance(Location a, Location b) => (a.Y - b.Y).Square() * 2 + (a.X - b.X).Square();

    private void DrawTile(Location location, AnsiChar ac) => Hud.DrawTile(location.X - 1, location.Y - 1, ac);

    private void EnterHighScore(int score)
    {
        var list = HighScoreListFactory.Load();
        var name = Hud.EnterHighScore(list, score);
        if (name == null)
            return;

        list.Add(name, score);
        HighScoreListFactory.Save(list);
        ShowHighScores();
    }

    private void ExecuteMessage(ref OopContext context)
    {
        var result = Features.ExecuteMessage(ref context);
        if (result is { Cancelled: false, Label: not null })
            context.NextLine = BroadcastLabel(context.Index, result.Label, false);
    }

    private void FadeBoard(AnsiChar ac) => Hud.FadeBoard(ac);

    public void FadeRed()
    {
        FadeBoard(Facts.ErrorFadeTile);
        Hud.RedrawBoard();
    }

    private Vector GetConveyorVector(int index) => new(State.Vector8[index], State.Vector8[index + 8]);

    private void InitializeElements(bool showInvisibleTiles)
    {
        Elements.Reset();
        Elements.Invisible().Character = showInvisibleTiles ? 0xB0 : 0x20;
        Elements.Invisible().Color = 0xFF;
        Elements.Player().Character = 0x02;
    }

    private void MainLoop(bool doFade)
    {
        var alternating = false;

        if (!_step)
        {
            Hud.CreateStatusText();
            Hud.UpdateStatus();
            MainLoopInit(doFade);
        }

        State.BreakGameLoop = false;

        while (ThreadActive)
        {
            if (!State.GamePaused)
            {
                if (State.ActIndex <= State.ActorCount)
                {
                    var actorData = Actors[State.ActIndex];
                    if (actorData.Cycle != 0)
                        if (State.ActIndex % actorData.Cycle == State.GameCycle % actorData.Cycle)
                            ActionList.Get(Tiles[actorData.Location].Id)?.Act(State.ActIndex);

                    State.ActIndex++;
                }
            }
            else
            {
                State.ActIndex = State.ActorCount + 1;

                if (Timers.Player.Clock(1, HsecToTicks(25)) > 0)
                    alternating = !alternating;

                if (alternating)
                {
                    var playerElement = Elements.Player();
                    DrawTile(Player.Location, new AnsiChar(playerElement.Character, playerElement.Color));
                }
                else
                {
                    if (Tiles[Player.Location].Id == Elements.PlayerId)
                        DrawTile(Player.Location, new AnsiChar(0x20, 0x0F));
                    else
                        UpdateBoard(Player.Location);
                }

                Hud.DrawPausing();
                ReadInput(false);
                if (State.KeyPressed == EngineKeyCode.Escape)
                {
                    if (World.Health > 0)
                    {
                        State.BreakGameLoop = Hud.EndGameConfirmation();
                    }
                    else
                    {
                        State.BreakGameLoop = true;
                        Hud.UpdateBorder();
                    }

                    State.KeyPressed = 0;
                }

                if (!State.KeyVector.IsZero())
                {
                    var target = Player.Location + State.KeyVector;
                    InteractionList.Get(ElementAt(target).Id)?.Interact(target, 0, ref State.KeyVector);
                }

                if (!State.KeyVector.IsZero())
                {
                    var target = Player.Location + State.KeyVector;
                    if (ElementAt(target).IsFloor)
                    {
                        Features.CleanUpPauseMovement();
                        State.GamePaused = false;
                        Hud.ClearPausing();
                        State.GameCycle = Random.GetNext(Facts.MainLoopRandomCycleRange);
                        World.IsLocked = true;
                    }
                    else
                    {
                        // Added so that attempting to run into a wall while paused using
                        // a joystick doesn't cause the game to freeze (the original engine
                        // just added delays)
                        WaitForTick();
                    }
                }
            }

            if (State.ActIndex > State.ActorCount)
            {
                if (!State.BreakGameLoop && !State.GamePaused)
                    if (State.GameWaitTime <= 0 || Timers.Player.Clock(1, State.GameWaitTime) > 0)
                    {
                        State.GameCycle++;
                        if (State.GameCycle > Facts.MaxGameCycle) State.GameCycle = 1;

                        State.ActIndex = 0;
                        ReadInput(false);
                    }

                Tracer.TraceStep();
                if (_step)
                    break;

                WaitForTick();
            }

            if (State.BreakGameLoop)
            {
                _soundUnit.ClearSound();
                if (State.PlayerElement == Elements.PlayerId)
                {
                    if (World.Health <= 0) EnterHighScore(World.Score);
                }
                else if (State.PlayerElement == Elements.MonitorId)
                {
                    Hud.ClearTitleStatus();
                }

                var element = Elements.Player();
                Tiles[Player.Location] = new Tile(element.Id, element.Color);
                State.GameOver = false;
                break;
            }
        }
    }

    private void MainLoopInit(bool doFade)
    {
        if (State.Init)
        {
            if (!State.AboutShown)
                ShowAbout();

            if (!ThreadActive)
                return;

            if (State.DefaultWorldName.Length > 0)
            {
                State.AboutShown = true;
                _worldUnit.LoadWorld(State.DefaultWorldName, false);
            }

            State.StartBoard = World.BoardIndex;
            _worldUnit.SetBoard(0);
            State.Init = false;
        }

        var element = Elements[State.PlayerElement];
        Tiles[Player.Location] = new Tile(element.Id, element.Color);
        if (State.PlayerElement == Elements.MonitorId)
        {
            SetMessage(0, new Message());
            Hud.DrawTitleStatus();
        }

        if (doFade)
            FadePurple();

        State.GameWaitTime = State.GameSpeed << 1;
        State.GameCycle = Random.GetNext(Facts.MainLoopRandomCycleRange);
        State.ActIndex = State.ActorCount + 1;
    }

    private void MoveTile(Location source, Location target)
    {
        var sourceIndex = ActorIndexAt(source);
        if (sourceIndex >= 0)
        {
            MoveActor(sourceIndex, target);
        }
        else
        {
            Tiles[target] = Tiles[source];
            UpdateBoard(target);
            RemoveItem(source);
            UpdateBoard(source);
        }
    }

    private void StartPlaying()
    {
        _worldUnit.SetBoard(State.StartBoard);
        Features.EnterBoard();
        State.PlayerElement = Elements.PlayerId;
        State.GamePaused = true;
        MainLoop(true);
    }

    private bool PlayWorld()
    {
        var gameIsActive = false;

        if (World.IsLocked)
        {
            _worldUnit.LoadWorld(World.Name, false);

            if (State.WorldLoaded)
            {
                gameIsActive = State.WorldLoaded;
                State.StartBoard = World.BoardIndex;
            }
        }
        else
        {
            gameIsActive = true;
        }

        if (gameIsActive)
            StartPlaying();

        return gameIsActive;
    }

    private char ReadActorCodeByte(int index, ref Word instruction)
    {
        var actor = Actors[index];
        var value = (char)0;

        if (instruction < 0 || instruction >= actor.Length)
        {
            State.OopByte = default;
        }
        else
        {
            value = actor.Code.Span[instruction];
            State.OopByte = value;
            instruction++;
        }

        return value;
    }

    private IAnsiKeyTransformer AnsiKeyTransformer { get; }

    private EngineKeyCode ConvertKey(KeyPress keyPress)
    {
        var bytes = AnsiKeyTransformer.GetBytes(keyPress);

        if (bytes.IsEmpty)
            return EngineKeyCode.None;

        if (bytes.Length > 1 && (bytes[0] == 0 || bytes[0] >= 0x80))
            return (EngineKeyCode)(bytes[1] | 0x80);

        return (EngineKeyCode)bytes[0];
    }

    private void ReadInputJoystick(bool isUiFocused)
    {
        // This function does things a lot differently than the original engine,
        // mostly for convenience in controls.

        var x = 0f;
        var y = 0f;
        JoystickButtons buttons = 0;

        if (Joystick.IsConnected)
        {
            x = Joystick.X;
            y = Joystick.Y;
            buttons = Joystick.Buttons;
        }

        // Directional buttons should act like analog input for movement directions.

        if (buttons.HasFlag(JoystickButtons.Up))
            y = -1;
        else if (buttons.HasFlag(JoystickButtons.Down))
            y = 1;
        else if (buttons.HasFlag(JoystickButtons.Left))
            x = -1;
        else if (buttons.HasFlag(JoystickButtons.Right))
            x = 1;

        // Determine which direction "wins" based on how far the stick is held from center.

        var deadZone = Config.JoystickDeadZone;
        var maxMagnitude = 0f;
        var finalKeyCode = (EngineKeyCode)0;

        if (x <= -deadZone & x <= -maxMagnitude)
        {
            State.KeyVector = Vector.West;
            maxMagnitude = Math.Max(maxMagnitude, Math.Abs(x));
            finalKeyCode = EngineKeyCode.Left;
        }

        if (x >= deadZone && x >= maxMagnitude)
        {
            State.KeyVector = Vector.East;
            maxMagnitude = Math.Max(maxMagnitude, Math.Abs(x));
            finalKeyCode = EngineKeyCode.Right;
        }

        if (y <= -deadZone && y <= -maxMagnitude)
        {
            State.KeyVector = Vector.North;
            maxMagnitude = Math.Max(maxMagnitude, Math.Abs(x));
            finalKeyCode = EngineKeyCode.Up;
        }

        if (y >= deadZone && y >= maxMagnitude)
        {
            State.KeyVector = Vector.South;
            maxMagnitude = Math.Max(maxMagnitude, Math.Abs(x));
            finalKeyCode = EngineKeyCode.Down;
        }

        if (finalKeyCode == EngineKeyCode.Left)
            buttons |= JoystickButtons.Left;
        else if (finalKeyCode == EngineKeyCode.Right)
            buttons |= JoystickButtons.Right;
        else if (finalKeyCode == EngineKeyCode.Up)
            buttons |= JoystickButtons.Up;
        else if (finalKeyCode == EngineKeyCode.Down)
            buttons |= JoystickButtons.Down;

        // The other buttons only activate when pressed and not every frame they're held.

        var singleButtons = buttons & ~_lastButtons;

        if (singleButtons.HasFlag(JoystickButtons.Left))
            State.KeyPressed = EngineKeyCode.Left;
        else if (singleButtons.HasFlag(JoystickButtons.Right))
            State.KeyPressed = EngineKeyCode.Right;
        else if (singleButtons.HasFlag(JoystickButtons.Up))
            State.KeyPressed = EngineKeyCode.Up;
        else if (singleButtons.HasFlag(JoystickButtons.Down))
            State.KeyPressed = EngineKeyCode.Down;

        // Process button actions.

        if (buttons.HasFlag(JoystickButtons.Ok))
        {
            if (isUiFocused)
            {
                State.KeyPressed = EngineKeyCode.Enter;
            }
            else
            {
                if (State.KeyPressed != EngineKeyCode.None)
                    State.KeyShift = true;
                else
                    State.KeyPressed = EngineKeyCode.Space;
            }
        }
        else if (buttons.HasFlag(JoystickButtons.Cancel))
        {
            if (isUiFocused)
                State.KeyPressed = EngineKeyCode.Escape;
        }
        else if (buttons.HasFlag(JoystickButtons.Shoot))
        {
            if (!isUiFocused)
                State.KeyShift = true;
        }

        if (isUiFocused && singleButtons.HasFlag(JoystickButtons.PageUp))
        {
            State.KeyPressed = EngineKeyCode.PageUp;
        }
        else if (isUiFocused && singleButtons.HasFlag(JoystickButtons.PageDown))
        {
            State.KeyPressed = EngineKeyCode.PageDown;
        }
        else if (singleButtons.HasFlag(JoystickButtons.Start))
        {
            // If on the title screen, Start will begin the game.
            // Otherwise, it will pause the game.

            if (State.PlayerElement == Elements.MonitorId)
                State.KeyPressed = Facts.StartGameKey;
            else
                State.KeyPressed = EngineKeyCode.P;
        }

        _lastButtons = buttons;
    }

    private void ReadInputKeyboard()
    {
        var mod = Keyboard.GetMod();
        State.KeyShift = mod.HasFlag(KeyMod.Shift);
        State.KeyPressed = 0;
        State.KeyVector = Vector.Idle;

        if (!Keyboard.KeyIsAvailable)
            return;

        var key = Keyboard.GetKey();
        if (key is not { } keyValue || keyValue.Key == AnsiKey.None)
            return;

        State.KeyPressed = ConvertKey(keyValue);

        switch (State.KeyPressed)
        {
            case EngineKeyCode.Left:
                State.KeyVector = Vector.West;
                break;
            case EngineKeyCode.Right:
                State.KeyVector = Vector.East;
                break;
            case EngineKeyCode.Up:
                State.KeyVector = Vector.North;
                break;
            case EngineKeyCode.Down:
                State.KeyVector = Vector.South;
                break;
        }
    }

    public void ReadInput(bool isUiFocused)
    {
        ReadInputKeyboard();
        if (State.KeyVector.IsZero())
            ReadInputJoystick(isUiFocused);
        if (State.KeyVector.IsNonZero())
            State.KeyLastVector = State.KeyVector;
    }

    private void ShowAbout() => Features.ShowAbout();

    private void StartInit()
    {
        State.GameSpeed = Facts.DefaultGameSpeed;
        State.GameWaitTime = 1;
        State.DefaultSaveName = Facts.DefaultSavedGameName;
        State.DefaultBoardName = Facts.DefaultBoardName;
        State.DefaultWorldName = Config.DefaultWorld ?? Facts.DefaultWorldName;
        State.ForestIndex = 2;
        State.Init = true;

        _worldUnit.ClearWorld();

        var cfg = _configFileService.Load();
        if (Config.DefaultWorld == null && cfg != null)
        {
            if (!string.IsNullOrEmpty(cfg.WorldName))
            {
                State.DefaultWorldName = (
                    cfg.WorldName?.StartsWith("*") ?? false
                        ? cfg.WorldName.Substring(1)
                        : cfg.WorldName
                ) ?? string.Empty;
            }
        }

        SetGameMode();
        Clock.Start();
    }

    private void StartMain()
    {
        Clock.OnTick += ClockTick;
        StartInit();
        TitleScreenLoop();
        Clock.OnTick -= ClockTick;
        Exited?.Invoke(this, EventArgs.Empty);
    }

    private void TitleScreenLoop()
    {
        State.QuitEngine = false;
        State.Init = true;
        State.StartBoard = 0;
        var gameEnded = true;
        Hud.Initialize();
        while (ThreadActive)
        {
            if (!State.Init) 
                _worldUnit.SetBoard(0);

            while (ThreadActive)
            {
                State.PlayerElement = Elements.MonitorId;
                State.GamePaused = false;
                MainLoop(gameEnded);
                gameEnded = false;

                if (!ThreadActive)
                    break;

                var startPlaying = Features.HandleTitleInput();
                if (startPlaying)
                    gameEnded = PlayWorld();

                if (gameEnded || State.QuitEngine)
                    break;
            }

            if (State.QuitEngine) break;
        }
    }

    public void Delay(int msec)
    {
        var waitUntil = DateTime.Now + TimeSpan.FromMilliseconds(msec);
        while (DateTime.Now < waitUntil)
            WaitForTick();
    }

    public int ResetBoardTimeHsec() => 
        _boardTime.Elapse();

    public void Dispose() => 
        Clock.Stop();
}