using System;
using System.Diagnostics;
using System.IO;
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
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class Engine : IEngine, IDisposable
{
    private readonly IConfigFileService _configFileService;
    private readonly IFileDialog _fileDialog;

    private int _ticksToRun;
    private float _boardTimeHsec;
    private bool _step;
    private readonly IEngineAccessor _engineAccessor;

    public Engine(IClock clock, IActors actors, IAlerts alerts, IBoard board,
        IFileSystem fileSystem, IElementList elements,
        IInterpreter interpreter, IRandomizer randomizer, IKeyboard keyboard,
        ITiles tiles, ISounds sounds, ITimers timers, IParser parser,
        IConfig config, IConditionList conditions, IDirectionList directions,
        IColors colors, ICheatList cheats, ICommandList commands, ITargetList targets,
        IFeatures features, IGameSerializer gameSerializer, IHud hud, IState state,
        IWorld world, IItemList items, IBoards boards, IActionList actionList,
        IDrawList drawList, IInteractionList interactionList, IFacts facts, IMemory memory,
        IHeap heap, IAnsiKeyTransformer ansiKeyTransformer, IScrollFormatter scrollFormatter,
        ISpeaker speaker, IDrumBank drumBank, IObjectMover objectMover,
        IMusicEncoder musicEncoder,
        IHighScoreListFactory highScoreListFactory, IConfigFileService configFileService,
        IFileDialog fileDialog, ITracer tracer, IEngineAccessor engineAccessor)
    {
        engineAccessor.Instance = this;

        Actors = actors;
        Alerts = alerts;
        Board = board;
        Clock = clock;
        Disk = fileSystem;
        ElementList = elements;
        Interpreter = interpreter;
        Random = randomizer;
        Keyboard = keyboard;
        Tiles = tiles;
        Sounds = sounds;
        Timers = timers;
        Parser = parser;
        Config = config;
        ConditionList = conditions;
        DirectionList = directions;
        Colors = colors;
        CheatList = cheats;
        CommandList = commands;
        TargetList = targets;
        Features = features;
        GameSerializer = gameSerializer;
        Hud = hud;
        State = state;
        World = world;
        ItemList = items;
        Boards = boards;
        ActionList = actionList;
        DrawList = drawList;
        InteractionList = interactionList;
        Facts = facts;
        Memory = memory;
        Heap = heap;
        AnsiKeyTransformer = ansiKeyTransformer;
        ScrollFormatter = scrollFormatter;
        Speaker = speaker;
        DrumBank = drumBank;
        ObjectMover = objectMover;
        MusicEncoder = musicEncoder;
        HighScoreListFactory = highScoreListFactory;
        _configFileService = configFileService;
        _fileDialog = fileDialog;
        Tracer = tracer;
        _engineAccessor = engineAccessor;
    }

    private void ClockTick(object? sender, EventArgs args)
    {
        if (_ticksToRun < 3)
            _ticksToRun++;

        if (!State.GamePaused)
            _boardTimeHsec += Config.MasterClockNumerator * 100f / Config.MasterClockDenominator;

        if (!ThreadActive)
            Clock.Stop();
    }

    private IHighScoreListFactory HighScoreListFactory { get; }

    private IObjectMover ObjectMover { get; }

    public IMusicEncoder MusicEncoder { get; }

    private IClock Clock { get; }

    private IBoards Boards { get; }

    private Tile BorderTile => State.BorderTile;

    public IFileSystem Disk { get; }

    private IFeatures Features { get; }

    private ISpeaker Speaker { get; }

    public IGameSerializer GameSerializer { get; }

    private IInterpreter Interpreter { get; }

    private IKeyboard Keyboard { get; }

    private IScrollFormatter ScrollFormatter { get; }

    public ITimers Timers { get; }

    public IDrumBank DrumBank { get; }

    public ITracer Tracer { get; }

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

        var cheat = CheatList.Get(cheatText);
        cheat?.Execute(cheatText, clear);
        Hud.UpdateStatus();

        PlaySound(10, Sounds.Cheat);
    }

    public void PlayStep()
    {
        if (State.GameOver || State.GameQuiet || State.SoundPlaying)
            return;

        Speaker.PlayStep();
    }

    public string GetHighScoreName(string fileName) => Features.GetHighScoreName(fileName);

    public void ShowHighScores()
    {
        var list = HighScoreListFactory.Load();
        if (list == null)
            return;

        Hud.ShowHighScores(list);
    }

    public IActionList ActionList { get; }

    public IActor ActorAt(Location location) =>
        Actors.ActorAt(location);

    public int ActorIndexAt(Location location) =>
        Actors.ActorIndexAt(location);

    public event EventHandler? Exited;
    public event EventHandler? Tick;

    public IActors Actors { get; }

    public int Adjacent(Location location, int id) => Features.GetAdjacent(location, id);

    public IAlerts Alerts { get; }

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

        if (Tiles[location].Id == ElementList.PlayerId && World.EnergyCycles > 0)
        {
            World.Score += ElementAt(Actors[index].Location).Points;
            UpdateStatus();
        }
        else
        {
            Destroy(location);
            PlaySound(2, Sounds.EnemySuicide);
        }
    }

    public IBoard Board { get; }

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

    public ICheatList CheatList { get; }

    public void CleanUpPassageMovement() => Features.CleanUpPassageMovement();

    public void ClearForest(Location location) => Features.ClearForest(location);

    public void ClearSound()
    {
        State.SoundPlaying = false;
        Speaker.StopNote();
    }

    public void ClearWorld()
    {
        State.BoardCount = 0;
        Boards.Clear();

        if (Config.NoPesterMode)
            Alerts.SetAll();
        else
            Alerts.Reset();

        ClearBoard();
        Boards.Add(new PackedBoard(GameSerializer.PackBoard(Tiles)));
        World.BoardIndex = 0;
        World.Ammo = Facts.DefaultAmmo;
        World.Gems = Facts.DefaultGems;
        World.Health = Facts.DefaultHealth;
        World.EnergyCycles = Facts.DefaultEnergyCycles;
        World.Torches = Facts.DefaultTorches;
        World.TorchCycles = Facts.DefaultTorchCycles;
        World.Score = Facts.DefaultScore;
        World.TimePassed = Facts.DefaultTimePassed;
        World.Stones = Facts.DefaultStones;
        World.Keys.Clear();
        World.Flags.Clear();
        SetBoard(0);
        Board.Name = Facts.DefaultBoardTitle;
        World.Name = Facts.DefaultWorldTitle;
        State.WorldFileName = string.Empty;
    }

    public IColors Colors { get; }

    public ICommandList CommandList { get; }

    public IConditionList ConditionList { get; }

    public IConfig Config { get; }

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
            var element = ElementList[surrounding[i].Id];
            if (element.Id == ElementList.EmptyId)
                pushable = true;
            else if (!element.IsPushable)
                pushable = false;
        }

        for (var i = beginIndex; i != endIndex; i += direction)
        {
            var element = ElementList[surrounding[i].Id];

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
                        Tiles[target].Id = ElementList.EmptyId;
                        MoveActor(index, target);
                        Tiles[source] = tile;
                    }
                    else
                    {
                        Tiles[target] = surrounding[i];
                        UpdateBoard(target);
                    }

                    if (!ElementList[surrounding[(i + 8 + direction) % 8].Id].IsPushable)
                    {
                        Tiles[source].Id = ElementList.EmptyId;
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
                if (element.Id == ElementList.EmptyId)
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

    public IDirectionList DirectionList { get; }

    public AnsiChar Draw(Location location)
    {
        if (Board.IsDark && !ElementAt(location).IsAlwaysVisible &&
            (World.TorchCycles <= 0 || Distance(Player.Location, location) >= Facts.TorchRadius) &&
            !State.EditorMode)
            return Facts.DarknessTile;

        ref var tile = ref Tiles[location];
        var element = ElementList[tile.Id];
        var elementCount = ElementList.Count;

        if (tile.Id == ElementList.EmptyId)
            return Facts.EmptyTile;

        if (element.HasDrawCode)
            return DrawList.Get(tile.Id).Draw(location);

        if (tile.Id < elementCount - 7) return new AnsiChar(element.Character, tile.Color);

        return tile.Id != elementCount - 1
            ? new AnsiChar(tile.Color, ((tile.Id - (elementCount - 8)) << 4) | 0x0F)
            : new AnsiChar(tile.Color, 0x0F);
    }

    public IDrawList DrawList { get; }

    public IElement ElementAt(Location location) => ElementList[Tiles[location].Id];

    public IElementList ElementList { get; }

    public void EnterBoard()
    {
        _boardTimeHsec = 0;
        Features.EnterBoard();
    }

    public void ExecuteCode(int index, ref Word instruction, string name)
    {
        var context = new OopContext(_engineAccessor)
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
                case 0x27: // '
                case 0x40: // @
                    Parser.DiscardLine(index, ref instruction);
                    break;
                case 0x2F: // /
                case 0x3F: // ?
                    if (context.Command == 0x2F)
                        context.Repeat = true;

                    if (!Parser.TryEvalDirection(ref context, ref instruction, out var vector))
                    {
                        RaiseError(ref context, "Bad direction");
                        break;
                    }

                    ObjectMover.ExecuteDirection(ref context, vector);

                    ReadActorCodeByte(index, ref instruction);
                    if (State.OopByte != 0x0D)
                        instruction--;
                    context.Moved = true;

                    break;
                case 0x23: // #
                    Interpreter.Execute(ref context, ref instruction);
                    break;
                case 0x0D: // enter
                    if (context.HasMessage)
                        context.AddMessage(string.Empty);
                    break;
                case 0x00:
                    context.Finished = true;
                    break;
                default:
                    context.AddMessage($"{context.Command.ToChar()}{Parser.ReadLine(context.Index, ref instruction)}");
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

    public IFacts Facts { get; }

    public IHeap Heap { get; }

    public IMemory Memory { get; }

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
                        PlaySound(4, Sounds.TimeOut);
                        RemoveItem(actor.Location);
                        var oldLocation = actor.Location;
                        actor.Location = Board.Entrance;
                        UpdateRadius(oldLocation, 0);
                        UpdateRadius(actor.Location, 0);
                        State.GamePaused = true;
                    }

                    PlaySound(4, Sounds.Ouch);
                }
                else
                {
                    PlaySound(5, Sounds.GameOver);
                }
            }
        }
        else
        {
            var element = Tiles[actor.Location].Id;
            if (element == ElementList.BulletId)
                PlaySound(3, Sounds.BulletDie);
            else if (element != ElementList.ObjectId) PlaySound(3, Sounds.EnemyDie);

            RemoveActor(index);
        }
    }

    public IHud Hud { get; }

    public IInteractionList InteractionList { get; }

    public IItemList ItemList { get; }

    private void ShowFormattedScroll(string error) =>
        Hud.ShowScroll(false, "Roton Error", ScrollFormatter.Format(error));

    public bool LoadWorld(string name, bool savedGame)
    {
        var worldData = TryLoadWorld();

        if (worldData == null || worldData.Length == 0)
        {
            ShowDosError();
            return false;
        }

        using (var stream = new MemoryStream(worldData))
        {
            if (stream.Length == 0)
                return false;

            using var reader = new BinaryReader(stream);
            var type = reader.ReadInt16();
            if (type != World.WorldType)
            {
                Hud.FailToLoadWorld();
                return false;
            }

            var numBoards = reader.ReadInt16();
            if (numBoards < 0)
                throw new Exception("Board count must be zero or greater.");

            State.BoardCount = numBoards;
            GameSerializer.LoadWorld(stream);

            var newBoards = Enumerable
                .Range(0, numBoards + 1)
                .Select(_ => new PackedBoard(GameSerializer.LoadBoardData(stream)))
                .ToList();

            Boards.Clear();

            foreach (var rawBoard in newBoards)
                Boards.Add(rawBoard);
        }

        Hud.CreateStatusWorld();
        UnpackBoard(World.BoardIndex);
        State.WorldLoaded = true;
        return true;

        byte[]? TryLoadWorld()
        {
            try
            {
                return Disk.GetFile(savedGame ? Features.GetSaveName(name) : Features.GetWorldName(name));
            }
            catch (IOException e)
            {
                ShowFormattedScroll(e.ToString());
                return [];
            }
        }
    }

    private void ShowDosError()
    {
        Hud.ShowScroll(false, "Error",
            [
                "$DOS Error:",
                string.Empty,
                "This may be caused by missing",
                "files or a bad disk. If you",
                "are trying to save a game,",
                "your disk may be full -- try",
                "using a blank, formatted disk",
                "for saving the game!"
            ]
        );
    }

    public void LockActor(int index) => Features.LockActor(index);

    public void MoveActor(int index, Location target)
    {
        var actor = Actors[index];
        var sourceLocation = actor.Location;
        ref var sourceTile = ref Tiles[actor.Location];
        ref var targetTile = ref Tiles[target];
        var underTile = actor.UnderTile;
        var nextUnderTile = targetTile;

        if (targetTile.Id == ElementList.EmptyId)
            targetTile = new Tile(sourceTile.Id, sourceTile.Color & 0x0F);
        else
            targetTile = new Tile(sourceTile.Id, (targetTile.Color & 0x70) | (sourceTile.Color & 0x0F));

        sourceTile = underTile;
        actor.Location = target;
        if (targetTile.Id == ElementList.PlayerId)
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
                var glowLocation = new Location();
                for (var x = target.X - Facts.TorchDrawBoxVerticalSize;
                     x <= target.X + Facts.TorchDrawBoxVerticalSize;
                     x++)
                for (var y = target.Y - Facts.TorchDrawBoxHorizontalSize;
                     y <= target.Y + Facts.TorchDrawBoxHorizontalSize;
                     y++)
                {
                    glowLocation = new Location(x, y);
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

        if (underId == ElementList.RiverNId)
            vector = Vector.North;
        else if (underId == ElementList.RiverSId)
            vector = Vector.South;
        else if (underId == ElementList.RiverWId)
            vector = Vector.West;
        else if (underId == ElementList.RiverEId)
            vector = Vector.East;

        if (vector.IsNonZero())
        {
            ref var actorTile = ref Tiles[actor.Location];
            if (actorTile.Id == ElementList.PlayerId)
            {
                var targetLocation = actor.Location + vector;
                InteractionList.Get(Tiles[targetLocation].Id).Interact(targetLocation, 0, ref vector);
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

    public IParser Parser { get; }

    public IActor Player => Actors[0];

    public void PlaySound(int priority, ISound sound, int? offset = null, int? length = null)
    {
        if (State.GameOver || State.GameQuiet)
            return;

        var soundIsNotPlaying = !State.SoundPlaying;
        var soundIsMusic = priority == -1;
        var soundIsHigherPriority = State.SoundPriority != -1 && priority >= State.SoundPriority;

        if (!(soundIsNotPlaying || soundIsMusic || soundIsHigherPriority))
            return;

        if (!soundIsMusic)
            State.SoundBuffer.Clear();

        State.SoundBuffer.Enqueue(sound, offset, length);
        State.SoundPlaying = true;
        State.SoundPriority = priority;
    }

    public void PlotTile(Location location, Tile tile)
    {
        if (ElementAt(location).Id == ElementList.PlayerId)
            return;

        var targetElement = ElementList[tile.Id];
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
        // this is here to prevent endless push loops
        // but doesn't exist in the original code
        if (vector.IsZero())
            throw Exceptions.PushStackOverflow;

        ref var tile = ref Tiles[location];
        if (tile.Id == ElementList.SliderEwId && vector.Y == 0 ||
            tile.Id == ElementList.SliderNsId && vector.X == 0 ||
            ElementList[tile.Id].IsPushable)
        {
            ref var furtherTile = ref Tiles[location + vector];
            if (furtherTile.Id == ElementList.TransporterId)
                PushThroughTransporter(location, vector);
            else if (furtherTile.Id != ElementList.EmptyId)
                Push(location + vector, vector);

            var furtherElement = ElementList[furtherTile.Id];
            if (!furtherElement.IsFloor && furtherElement.IsDestructible && furtherTile.Id != ElementList.PlayerId)
                Destroy(location + vector);

            furtherElement = ElementList[furtherTile.Id];
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
                if (element.Id == ElementList.BoardEdgeId)
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

                if (element.Id == ElementList.TransporterId)
                    if (ActorAt(search).Vector == -vector)
                        success = true;
            }

            if (target.X > 0)
            {
                MoveTile(actor.Location - vector, target);
                PlaySound(3, Sounds.Transporter);
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
        PlaySound(5, Sounds.Error);
        Tracer.TraceError(ref context, error);
    }

    public IRandomizer Random { get; }

    public void RemoveActor(int index)
    {
        var actor = Actors[index];
        if (index < State.ActIndex) State.ActIndex--;

        Tiles[actor.Location] = actor.UnderTile;
        if (actor.Location.Y > 0) UpdateBoard(actor.Location);

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

    public Vector RndP(Vector vector)
    {
        var result = new Vector();
        result = Random.GetNext(2) == 0
            ? vector.Clockwise()
            : vector.CounterClockwise();
        return result;
    }

    public void SaveWorld(string name)
    {
        // Make sure the packed board data is up to date.

        PackBoard();

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // Write common world header.

        var type = (short)World.WorldType;
        var numBoards = (short)(Boards.Count - 1);

        writer.Write(type);
        writer.Write(numBoards);

        // Write world data.

        GameSerializer.SaveWorld(stream);

        // Write each packed board.

        foreach (var board in Boards)
            GameSerializer.SaveBoardData(stream, board.Data);

        stream.Flush();

        // Save to disk. Extension depends on whether the game world has been
        // modified in-game.

        var fileName = World.IsLocked ? Features.GetSaveName(name) : Features.GetWorldName(name);
        Disk.PutFile(fileName, stream.ToArray());
    }

    public Vector Seek(Location location)
    {
        var result = new Vector();
        if (Random.GetNext(2) == 0 || Player.Location.Y == location.Y)
            result.X = (Player.Location.X - location.X).Polarity();

        if (result.X == 0) result.Y = (Player.Location.Y - location.Y).Polarity();

        if (World.EnergyCycles > 0) result = -result;

        return result;
    }

    public void SetBoard(int boardIndex)
    {
        var element = ElementList.Player();
        Tiles[Player.Location] = new Tile(element.Id, element.Color);
        PackBoard();
        UnpackBoard(boardIndex);
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

        SpawnActor(new Location(0, 0), new Tile(ElementList.MessengerId, 0), 1, State.DefaultActor);
        Actors[State.ActorCount].P2 = unchecked((byte)(duration / (State.GameWaitTime + 1)));
        State.Message = topMessage;
        State.Message2 = bottomMessage;
    }

    public void ShowHelp(string title, string filename) => Hud.ShowHelp(title, filename);

    public void ShowInGameHelp() => Features.ShowInGameHelp();

    public void OpenWorld()
    {
        var name = Features.OpenWorld();
        if (string.IsNullOrEmpty(name))
            return;

        LoadWorld(name!, false);
        State.StartBoard = World.BoardIndex;
        SetBoard(0);

        var element = ElementList[State.PlayerElement];
        Tiles[Player.Location] = new Tile(element.Id, element.Color);

        FadePurple();
    }

    public bool RestoreWorld()
    {
        var name = Features.RestoreWorld();
        if (string.IsNullOrEmpty(name))
            return false;

        if (!LoadWorld(name!, true))
            return false;

        State.StartBoard = World.BoardIndex;
        World.IsLocked = false;
        SetBoard(State.StartBoard);
        return true;
    }

    public string? ShowLoad(string title, string extension)
    {
        return _fileDialog.Open(title, extension);
    }

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

        if (element.IsFloor || element.Id == ElementList.WaterId)
        {
            SpawnActor(target, new Tile(id, ElementList[id].Color), 1, State.DefaultActor);
            var actor = Actors[State.ActorCount];
            actor.P1 = unchecked((byte)(enemyOwned ? 1 : 0));
            actor.Vector = vector;
            actor.P2 = 0x64;
            return true;
        }

        if (element.Id != ElementList.BreakableId &&
            (!element.IsDestructible ||
             element.Id == ElementList.PlayerId != enemyOwned ||
             World.EnergyCycles != 0))
            return false;

        Destroy(target);
        PlaySound(2, Sounds.BulletDie);
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

    public IState State { get; }

    public void Stop()
    {
        Thread = null;
    }

    public ITargetList TargetList { get; }

    public ITiles Tiles { get; }

    public bool TitleScreen => State.PlayerElement != ElementList.PlayerId;

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

                            if (element.IsDestructible || element.Id == ElementList.StarId) Destroy(target);

                            if (element.Id == ElementList.EmptyId || element.Id == ElementList.BreakableId)
                                Tiles[target] = new Tile(ElementList.BreakableId, Random.GetNext(7) + 9);
                        }
                        else
                        {
                            if (Tiles[target].Id == ElementList.BreakableId) Tiles[target].Id = ElementList.EmptyId;
                        }
                    }

                UpdateBoard(target);
            }
    }

    public void UpdateStatus() => Hud.UpdateStatus();

    private void UpdateSound()
    {
        if (!State.SoundPlaying)
            return;

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

    public void WaitForTick()
    {
        var isFast = State.GameWaitTime <= 0 && Config.FastMode;

        if (isFast)
        {
            SpinWait.SpinUntil(() =>
            {
                if (_ticksToRun <= 0)
                    return true;

                UpdateSound();
                if (Clock != null)
                    Tick?.Invoke(this, EventArgs.Empty);
                _ticksToRun--;

                return false;
            });
        }
        else
        {
            UpdateSound();

            if (Clock == null)
                return;

            Tick?.Invoke(this, EventArgs.Empty);

            SpinWait.SpinUntil(() => _ticksToRun > 0 || !ThreadActive);

            if (_ticksToRun > 0)
                _ticksToRun--;
        }
    }

    public IWorld World { get; }

    private bool ActorIsLocked(int index) => Features.IsActorLocked(index);

    public void ClearBoard()
    {
        var emptyId = ElementList.EmptyId;
        var boardEdgeId = State.EdgeTile.Id;
        var boardBorderId = BorderTile.Id;
        var boardBorderColor = BorderTile.Color;

        // board properties
        Board.Name = string.Empty;
        State.Message = string.Empty;
        Board.MaximumShots = Facts.DefaultMaximumShots;
        Board.IsDark = false;
        Board.RestartOnZap = false;
        Board.TimeLimit = 0;
        Board.Exits.East = 0;
        Board.Exits.North = 0;
        Board.Exits.South = 0;
        Board.Exits.West = 0;

        // build board edges
        for (var y = 0; y <= Tiles.Height + 1; y++)
        {
            Tiles[new Location(0, y)].Id = boardEdgeId;
            Tiles[new Location(Tiles.Width + 1, y)].Id = boardEdgeId;
        }

        for (var x = 0; x <= Tiles.Width + 1; x++)
        {
            Tiles[new Location(x, 0)].Id = boardEdgeId;
            Tiles[new Location(x, Tiles.Height + 1)].Id = boardEdgeId;
        }

        // clear out board
        for (var x = 1; x <= Tiles.Width; x++)
        for (var y = 1; y <= Tiles.Height; y++)
            Tiles[new Location(x, y)] = new Tile(emptyId, 0);

        // build border
        for (var y = 1; y <= Tiles.Height; y++)
        {
            Tiles[new Location(1, y)] = new Tile(boardBorderId, boardBorderColor);
            Tiles[new Location(Tiles.Width, y)] = new Tile(boardBorderId, boardBorderColor);
        }

        for (var x = 1; x <= Tiles.Width; x++)
        {
            Tiles[new Location(x, 1)] = new Tile(boardBorderId, boardBorderColor);
            Tiles[new Location(x, Tiles.Height)] = new Tile(boardBorderId, boardBorderColor);
        }

        // generate player actor
        var element = ElementList.Player();
        State.ActorCount = 0;
        Player.Location = new Location(Tiles.Width / 2, Tiles.Height / 2);
        Tiles[Player.Location] = new Tile(element.Id, element.Color);
        Player.Cycle = 1;
        Player.UnderTile = new Tile(0, 0);
        Player.Pointer = 0;
        Player.Length = 0;
    }

    private int ColorMatch(Tile tile)
    {
        var element = ElementList[tile.Id];

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
        if (list == null)
            return;

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

    private void InitializeElements(bool showInvisibles)
    {
        ElementList.Reset();

        // this isn't all the initializations.
        // todo: replace this with the ability to completely reinitialize engine default memory
        ElementList.Invisible().Character = showInvisibles ? 0xB0 : 0x20;
        ElementList.Invisible().Color = 0xFF;
        ElementList.Player().Character = 0x02;
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
                            ActionList.Get(Tiles[actorData.Location].Id).Act(State.ActIndex);

                    State.ActIndex++;
                }
            }
            else
            {
                State.ActIndex = State.ActorCount + 1;

                if (Timers.Player.Clock(1, Facts.PauseFlashInterval) > 0)
                    alternating = !alternating;

                if (alternating)
                {
                    var playerElement = ElementList.Player();
                    DrawTile(Player.Location, new AnsiChar(playerElement.Character, playerElement.Color));
                }
                else
                {
                    if (Tiles[Player.Location].Id == ElementList.PlayerId)
                        DrawTile(Player.Location, new AnsiChar(0x20, 0x0F));
                    else
                        UpdateBoard(Player.Location);
                }

                Hud.DrawPausing();
                ReadInput();
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

                if (!State.KeyVector.IsZero() && State.KeyArrow)
                {
                    var target = Player.Location + State.KeyVector;
                    InteractionList.Get(ElementAt(target).Id).Interact(target, 0, ref State.KeyVector);
                }

                if (!State.KeyVector.IsZero() && State.KeyArrow)
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
                        ReadInput();
                    }

                Tracer.TraceStep();
                if (_step)
                    break;

                WaitForTick();
            }

            if (State.BreakGameLoop)
            {
                ClearSound();
                if (State.PlayerElement == ElementList.PlayerId)
                {
                    if (World.Health <= 0) EnterHighScore(World.Score);
                }
                else if (State.PlayerElement == ElementList.MonitorId)
                {
                    Hud.ClearTitleStatus();
                }

                var element = ElementList.Player();
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
                LoadWorld(State.DefaultWorldName, false);
            }

            State.StartBoard = World.BoardIndex;
            SetBoard(0);
            State.Init = false;
        }

        var element = ElementList[State.PlayerElement];
        Tiles[Player.Location] = new Tile(element.Id, element.Color);
        if (State.PlayerElement == ElementList.MonitorId)
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

    public void PackBoard()
    {
        var board = new PackedBoard(GameSerializer.PackBoard(Tiles));
        PackBoard(World.BoardIndex, board);
    }

    private void PackBoard(int boardIndex, IPackedBoard board)
    {
        // bit of a hack to make sure we don't go out of bounds
        while (Boards.Count <= boardIndex)
            Boards.Add(new PackedBoard([]));

        State.BoardCount = Boards.Count - 1;
        Boards[World.BoardIndex] = board;
    }

    private void StartPlaying()
    {
        SetBoard(State.StartBoard);
        EnterBoard();
        State.PlayerElement = ElementList.PlayerId;
        State.GamePaused = true;
        MainLoop(true);
    }

    private bool PlayWorld()
    {
        var gameIsActive = false;

        if (World.IsLocked)
        {
            LoadWorld(World.Name, false);

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

    private int ReadActorCodeByte(int index, ref Word instruction)
    {
        var actor = Actors[index];
        var value = 0;

        if (instruction < 0 || instruction >= actor.Length)
        {
            State.OopByte = 0;
        }
        else
        {
            value = actor.Code?[instruction] ?? 0;
            State.OopByte = value;
            instruction++;
        }

        return value;
    }

    private IAnsiKeyTransformer AnsiKeyTransformer { get; }

    private EngineKeyCode ConvertKey(IKeyPress keyPress)
    {
        var bytes = AnsiKeyTransformer.GetBytes(keyPress)?.ToList();

        if (bytes == null || bytes.Count == 0)
            return EngineKeyCode.None;

        if (bytes.Count > 1 && (bytes[0] == 0 || bytes[0] >= 0x80))
            return (EngineKeyCode)(bytes[1] | 0x80);

        return (EngineKeyCode)bytes[0];
    }

    public void ReadInput()
    {
        var mod = Keyboard.GetMod();
        State.KeyShift = mod.HasFlag(KeyMod.Shift);
        State.KeyArrow = false;
        State.KeyPressed = 0;
        State.KeyVector = new Vector(0, 0);

        if (!Keyboard.KeyIsAvailable)
            return;

        var key = Keyboard.GetKey();
        if (key == null || key.Key == AnsiKey.None)
            return;

        State.KeyPressed = ConvertKey(key);

        switch (State.KeyPressed)
        {
            case EngineKeyCode.Left:
                State.KeyVector = Vector.West;
                State.KeyArrow = true;
                break;
            case EngineKeyCode.Right:
                State.KeyVector = Vector.East;
                State.KeyArrow = true;
                break;
            case EngineKeyCode.Up:
                State.KeyVector = Vector.North;
                State.KeyArrow = true;
                break;
            case EngineKeyCode.Down:
                State.KeyVector = Vector.South;
                State.KeyArrow = true;
                break;
        }
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

        ClearWorld();

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
            if (!State.Init) SetBoard(0);

            while (ThreadActive)
            {
                State.PlayerElement = ElementList.MonitorId;
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

    public void UnpackBoard(int boardIndex)
    {
        GameSerializer.UnpackBoard(Tiles, Boards[boardIndex].Data);
        World.BoardIndex = boardIndex;
    }

    public void Delay(int msec)
    {
        var waitUntil = DateTime.Now + TimeSpan.FromMilliseconds(msec);
        while (DateTime.Now < waitUntil)
            WaitForTick();
    }

    public void PlayErrorSound()
    {
        ClearSound();
        PlaySound(1, MusicEncoder.Encode("s004x114x9"));
    }

    public int ResetBoardTimeHsec()
    {
        var result = (int)Math.Truncate(_boardTimeHsec);
        _boardTimeHsec -= result;
        return result;
    }

    public void Dispose()
    {
        Clock?.Stop();
    }
}