using System;
using System.Linq;
using System.Threading;
using Roton.Emulation.Actions;
using Roton.Emulation.Cheats;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Draws;
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Interactions;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class Engine : IEngine, IDisposable
{
    private readonly IClock _clock;
    private readonly IActorList _actorList;
    private readonly IAlerts _alerts;
    private readonly IBoard _board;
    private readonly IElementList _elementList;
    private readonly IInterpreter _interpreter;
    private readonly IRandomizer _randomizer;
    private readonly IKeyboard _keyboard;
    private readonly ITiles _tiles;
    private readonly ISounds _sounds;
    private readonly ITimers _timers;
    private readonly IParser _parser;
    private readonly IConfig _config;
    private readonly ICheatList _cheats;
    private readonly IFeatures _features;
    private readonly IHud _hud;
    private readonly IState _state;
    private readonly IWorld _world;
    private readonly IBoardList _boardList;
    private readonly IActionList _actionList;
    private readonly IDrawList _drawList;
    private readonly IInteractionList _interactionList;
    private readonly IFacts _facts;
    private readonly ICodeHeap _heap;
    private readonly IAnsiKeyTransformer _ansiKeyTransformer;
    private readonly ISpeaker _speaker;
    private readonly IObjectMover _objectMover;
    private readonly IHighScoreListFactory _highScoreListFactory;
    private readonly IConfigFileService _configFileService;
    private readonly ITracer _tracer;
    private readonly IJoystick _joystick;
    private readonly ISoundUnit _soundUnit;
    private readonly IWorldUnit _worldUnit;
    private readonly IBoardTime _boardTime;
    private readonly Func<bool> _waitForTickFastDelegate;
    private readonly Func<bool> _waitForTickNormalDelegate;

    private int _ticksToRun;
    private bool _step;
    private JoystickButtons _lastButtons;

    public Engine(IClock clock, IActorList actorList, IAlerts alerts, IBoard board,
        IElementList elementList,
        IInterpreter interpreter, IRandomizer randomizer, IKeyboard keyboard,
        ITiles tiles, ISounds sounds, ITimers timers, IParser parser,
        IConfig config, ICheatList cheats,
        IFeatures features, IHud hud, IState state,
        IWorld world, IBoardList boardList, IActionList actionList,
        IDrawList drawList, IInteractionList interactionList, IFacts facts,
        ICodeHeap heap, IAnsiKeyTransformer ansiKeyTransformer,
        ISpeaker speaker, IObjectMover objectMover,
        IHighScoreListFactory highScoreListFactory, IConfigFileService configFileService, ITracer tracer,
        IEngineAccessor engineAccessor, IJoystick joystick, ISoundUnit soundUnit, IWorldUnit worldUnit,
        IBoardTime boardTime)
    {
        engineAccessor.Instance = this;

        _clock = clock;
        _actorList = actorList;
        _alerts = alerts;
        _board = board;
        _elementList = elementList;
        _interpreter = interpreter;
        _randomizer = randomizer;
        _keyboard = keyboard;
        _tiles = tiles;
        _sounds = sounds;
        _timers = timers;
        _parser = parser;
        _config = config;
        _cheats = cheats;
        _features = features;
        _hud = hud;
        _state = state;
        _world = world;
        _boardList = boardList;
        _actionList = actionList;
        _drawList = drawList;
        _interactionList = interactionList;
        _facts = facts;
        _heap = heap;
        _ansiKeyTransformer = ansiKeyTransformer;
        _speaker = speaker;
        _objectMover = objectMover;
        _highScoreListFactory = highScoreListFactory;
        _configFileService = configFileService;
        _tracer = tracer;
        _joystick = joystick;
        _soundUnit = soundUnit;
        _worldUnit = worldUnit;
        _boardTime = boardTime;

        _waitForTickFastDelegate = WaitForTickFastCondition;
        _waitForTickNormalDelegate = WaitForTickNormalCondition;
    }

    private void ClockTick(object? sender, EventArgs args)
    {
        if (_ticksToRun < 3)
            _ticksToRun++;

        if (!_state.GamePaused)
            _boardTime.Advance();

        if (!ThreadActive)
            _clock.Stop();
    }

    private Thread? Thread { get; set; }

    public bool ThreadActive => Thread != null || _step;

    public int MemoryUsage => _features.BaseMemoryUsage + _heap.Size + _boardList.Sum(b => b.Data.Length);

    public void Cheat()
    {
        var cheatText = _hud.EnterCheat().UpCased();
        var clear = false;

        if (!ThreadActive)
            return;

        if (!string.IsNullOrEmpty(cheatText))
        {
            if (cheatText![0] == '-')
            {
                cheatText = cheatText.Substring(1);
                while (_world.Flags.Contains(cheatText))
                    _world.Flags.Remove(cheatText);
                clear = true;
            }
            else if (cheatText[0] == '+')
            {
                cheatText = cheatText.Substring(1);
                _world.Flags.Add(cheatText);
            }
        }

        var cheat = _cheats.Get(cheatText);
        cheat?.Execute(cheatText, clear);
        _hud.UpdateStatus();

        _soundUnit.PlaySound(10, _sounds.Cheat);
    }

    public string GetHighScoreName(string fileName) => _features.GetHighScoreName(fileName);

    public void ShowHighScores()
    {
        var list = _highScoreListFactory.Load();
        _hud.ShowHighScores(list);
    }

    public IActor ActorAt(Location location) =>
        _actorList.ActorAt(location);

    public int ActorIndexAt(Location location) =>
        _actorList.ActorIndexAt(location);

    public event EventHandler? Exited;
    public event EventHandler? Tick;

    public int Adjacent(Location location, int id) => _features.GetAdjacent(location, id);

    public void Attack(int index, Location location)
    {
        if (index == 0 && _world.EnergyCycles > 0)
        {
            _world.Score += ElementAt(location).Points;
            UpdateStatus();
        }
        else
        {
            Harm(index);
        }

        if (index > 0 && index <= _state.ActIndex) _state.ActIndex--;

        if (_tiles[location].Id == _elementList.PlayerId && _world.EnergyCycles > 0)
        {
            _world.Score += ElementAt(_actorList[index].Location).Points;
            UpdateStatus();
        }
        else
        {
            Destroy(location);
            _soundUnit.PlaySound(2, _sounds.EnemySuicide);
        }
    }

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

                _tracer.TraceBroadcast(sender, label, info.Index, ignoreLock, ignoreSelfLock);
                _actorList[info.Index].Instruction = info.Offset;
                NotifyActorSentLabel(info.Index);
            }
        }

        return success;
    }

    public void CleanUpPassageMovement() => _features.CleanUpPassageMovement();

    public void ClearForest(Location location) => _features.ClearForest(location);

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
            surrounding[i] = _tiles[center + GetConveyorVector(i)];
            var element = _elementList[surrounding[i].Id];
            if (element.Id == _elementList.EmptyId)
                pushable = true;
            else if (!element.IsPushable)
                pushable = false;
        }

        for (var i = beginIndex; i != endIndex; i += direction)
        {
            var element = _elementList[surrounding[i].Id];

            if (pushable)
            {
                if (element.IsPushable)
                {
                    var source = center + GetConveyorVector(i);
                    var target = center + GetConveyorVector((i + 8 - direction) % 8);
                    if (element.Cycle > -1)
                    {
                        ref var tile = ref _tiles[source];
                        var index = ActorIndexAt(source);
                        _tiles[source] = surrounding[i];
                        _tiles[target].Id = _elementList.EmptyId;
                        MoveActor(index, target);
                        _tiles[source] = tile;
                    }
                    else
                    {
                        _tiles[target] = surrounding[i];
                        UpdateBoard(target);
                    }

                    if (!_elementList[surrounding[(i + 8 + direction) % 8].Id].IsPushable)
                    {
                        _tiles[source].Id = _elementList.EmptyId;
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
                if (element.Id == _elementList.EmptyId)
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
        if (_board.IsDark && !ElementAt(location).IsAlwaysVisible &&
            (_world.TorchCycles <= 0 || Distance(_actorList.Player.Location, location) >= _facts.TorchRadius) &&
            !_state.EditorMode)
            return _facts.DarknessTile;

        ref var tile = ref _tiles[location];
        var element = _elementList[tile.Id];
        var elementCount = _elementList.Count;

        if (tile.Id == _elementList.EmptyId)
            return _facts.EmptyTile;

        if (element.HasDrawCode)
            return _drawList.Get(tile.Id)?.Draw(location) ?? new AnsiChar(0x4F, 0x41);

        if (tile.Id < elementCount - 7) return new AnsiChar(element.Character, tile.Color);

        return tile.Id != elementCount - 1
            ? new AnsiChar(tile.Color, ((tile.Id - (elementCount - 8)) << 4) | 0x0F)
            : new AnsiChar(tile.Color, 0x0F);
    }

    public IElement ElementAt(Location location) => _elementList[_tiles[location].Id];

    public void ExecuteCode(int index, ref Word instruction, string name)
    {
        var context = new OopContext(_actorList)
        {
            Index = index,
            Name = name,
            PreviousInstruction = instruction
        };

        while (true)
        {
            if (instruction < 0)
                break;

            _tracer?.TraceOop(ref context, ref instruction);

            context.NextLine = true;
            context.PreviousInstruction = instruction;
            context.Command = ReadActorCodeByte(index, ref instruction);

            while (context.Command == ':')
            {
                _parser.DiscardLine(index, ref instruction);
                _tracer?.TraceOop(ref context, ref instruction);
                context.Command = ReadActorCodeByte(index, ref instruction);
            }

            switch (context.Command)
            {
                case '\'':
                case '@':
                    _parser.DiscardLine(index, ref instruction);
                    break;
                case '/':
                case '?':
                    if (context.Command == '/')
                        context.Repeat = true;

                    if (!_parser.TryEvalDirection(ref context, ref instruction, out var vector))
                    {
                        RaiseError(ref context, "Bad direction");
                        break;
                    }

                    _objectMover.ExecuteDirection(ref context, vector);

                    if (ReadActorCodeByte(index, ref instruction) != '\r')
                        instruction--;
                    context.Moved = true;

                    break;
                case '#':
                    _interpreter.Execute(ref context, ref instruction);
                    break;
                case '\r':
                    if (context.HasMessage)
                        context.AddMessage(string.Empty);
                    break;
                case '\0':
                    context.Finished = true;
                    break;
                default:
                    context.AddMessage($"{context.Command}{_parser.ReadLine(context.Index, ref instruction)}");
                    break;
            }

            if (context.Finished ||
                context.Moved ||
                context.Repeat ||
                context.Died ||
                context.CommandsExecuted >= _facts.MaxOopCommands)
                break;
        }

        if (context.Repeat)
            instruction = context.PreviousInstruction;

        if (_state.OopByte == 0)
            instruction = -1;

        if (context.HasMessage)
            ExecuteMessage(ref context);

        if (context.Died)
            CleanUpOop(ref context);
    }

    public void CleanUpOop(ref OopContext context) => _features.CleanUpOop(ref context);

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
            success = _parser.TryEvalTarget(sender, ref search, target);
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
            if (label.Equals(_facts.RestartLabel, StringComparison.OrdinalIgnoreCase))
            {
                search.Offset = 0;
            }
            else
            {
                prefix.CopyTo(buffer);
                label.CopyTo(buffer.Slice(prefix.Length));
                search.Offset = _parser.Search(search.Index, buffer.Slice(0, prefix.Length + label.Length));
                if (search.Offset < 0 && split > 0)
                {
                    success = _parser.TryEvalTarget(sender, ref search, target);
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
        if (!_parser.TryEvalItem(ref context, ref instruction, out var item))
            return false;

        // Do we have a valid amount?
        var amount = _parser.ReadNumber(context.Index, ref context.Actor.Instruction);
        if (amount <= 0)
            return true;

        // Modify value if we are taking.
        if (take)
            _state.OopNumber = -_state.OopNumber;

        // Determine if the result will be in range.
        var pendingAmount = item!.Value + _state.OopNumber;
        if ((pendingAmount & 0xFFFF) >= 0x8000)
            return true;

        // Successful transaction.
        item.Value = pendingAmount;
        return false;
    }

    public void StepOnce()
    {
        _step = true;
        MainLoop(true);
        _step = false;
    }

    public string[] GetMessageLines() => _features.GetMessageLines();

    public void FadePurple()
    {
        FadeBoard(_facts.FadeTile);
        _hud.RedrawBoard();
    }

    public int GetColorMatchValue(int color) => _features.GetColorMatchValue(color);

    public bool FindTile(Tile kind, Location location)
    {
        var matchColor = GetColorMatchValue(kind.Color);

        location.X++;
        while (location.Y <= _tiles.Height)
        {
            while (location.X <= _tiles.Width)
            {
                ref var tile = ref _tiles[location];
                if (tile.Id == kind.Id)
                {
                    var foundColor = GetColorMatchValue(ColorMatch(_tiles[location]));
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

    public void ForcePlayerColor(int index) => _features.ForcePlayerColor(index);

    public Vector GetCardinalVector(int index) => new(_state.Vector4[index], _state.Vector4[index + 4]);

    public void HandlePlayerInput(IActor actor) => _features.HandlePlayerInput(actor);

    public void Harm(int index)
    {
        var actor = _actorList[index];
        if (index == 0)
        {
            if (_world.Health > 0)
            {
                _world.Health -= _facts.HealthLostPerHit;
                UpdateStatus();
                SetMessage(_facts.ShortMessageDuration, _alerts.OuchMessage);
                _tiles[actor.Location].Color = (ElementAt(actor.Location).Color & 0x0F) | 0x70;

                if (_world.Health > 0)
                {
                    _world.TimePassed = 0;
                    if (_board.RestartOnZap)
                    {
                        _soundUnit.PlaySound(4, _sounds.TimeOut);
                        RemoveItem(actor.Location);
                        var oldLocation = actor.Location;
                        actor.Location = _board.Entrance;
                        UpdateRadius(oldLocation, 0);
                        UpdateRadius(actor.Location, 0);
                        _state.GamePaused = true;
                    }

                    _soundUnit.PlaySound(4, _sounds.Ouch);
                }
                else
                {
                    _soundUnit.PlaySound(5, _sounds.GameOver);
                }
            }
        }
        else
        {
            var element = _tiles[actor.Location].Id;
            if (element == _elementList.BulletId)
                _soundUnit.PlaySound(3, _sounds.BulletDie);
            else if (element != _elementList.ObjectId) _soundUnit.PlaySound(3, _sounds.EnemyDie);

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
        Math.Max(1, hsec * (_config.MasterClockDenominator / _config.MasterClockNumerator + 50) / 100);

    public void LockActor(int index) => _features.LockActor(index);

    public void MoveActor(int index, Location target)
    {
        var actor = _actorList[index];
        var sourceLocation = actor.Location;
        ref var sourceTile = ref _tiles[actor.Location];
        ref var targetTile = ref _tiles[target];
        var underTile = actor.UnderTile;
        var nextUnderTile = targetTile;

        if (targetTile.Id == _elementList.EmptyId)
            targetTile = new Tile(sourceTile.Id, sourceTile.Color & 0x0F);
        else
            targetTile = new Tile(sourceTile.Id, (targetTile.Color & 0x70) | (sourceTile.Color & 0x0F));

        sourceTile = underTile;
        actor.Location = target;
        if (targetTile.Id == _elementList.PlayerId)
            ForcePlayerColor(index);

        UpdateBoard(target);
        UpdateBoard(sourceLocation);
        actor.UnderTile = nextUnderTile;

        if (index == 0 && _board.IsDark)
        {
            var squareDistanceX = (target.X - sourceLocation.X).Square();
            var squareDistanceY = (target.Y - sourceLocation.Y).Square();
            if (squareDistanceX + squareDistanceY == 1)
            {
                for (var x = target.X - _facts.TorchDrawBoxVerticalSize;
                     x <= target.X + _facts.TorchDrawBoxVerticalSize;
                     x++)
                for (var y = target.Y - _facts.TorchDrawBoxHorizontalSize;
                     y <= target.Y + _facts.TorchDrawBoxHorizontalSize;
                     y++)
                {
                    var glowLocation = new Location(x, y);
                    if (glowLocation.X >= 1 && glowLocation.X <= _tiles.Width && glowLocation.Y >= 1 &&
                        glowLocation.Y <= _tiles.Height)
                        if ((Distance(sourceLocation, glowLocation) < _facts.TorchRadius) ^
                            (Distance(target, glowLocation) < _facts.TorchRadius))
                            UpdateBoard(glowLocation);
                }
            }
        }

        if (index == 0)
            _hud.UpdateCamera();
    }

    public void MoveActorOnRiver(int index)
    {
        var actor = _actorList[index];
        var vector = new Vector();
        var underId = actor.UnderTile.Id;

        if (underId == _elementList.RiverNId)
            vector = Vector.North;
        else if (underId == _elementList.RiverSId)
            vector = Vector.South;
        else if (underId == _elementList.RiverWId)
            vector = Vector.West;
        else if (underId == _elementList.RiverEId)
            vector = Vector.East;

        if (vector.IsNonZero())
        {
            ref var actorTile = ref _tiles[actor.Location];
            if (actorTile.Id == _elementList.PlayerId)
            {
                var targetLocation = actor.Location + vector;
                _interactionList.Get(_tiles[targetLocation].Id)?.Interact(targetLocation, 0, ref vector);
            }
        }

        if (vector.IsNonZero())
        {
            var target = actor.Location + vector;
            if (ElementAt(target).IsFloor)
                MoveActor(index, target);
        }
    }

    public void NotifyActorSentLabel(int index) => _features.NotifyActorSentLabel(index);

    public void PlotTile(Location location, Tile tile)
    {
        if (ElementAt(location).Id == _elementList.PlayerId)
            return;

        var targetElement = _elementList[tile.Id];
        ref var existingTile = ref _tiles[location];
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
                    _state.DefaultActor);
        }

        UpdateBoard(location);
    }

    public void Push(Location location, Vector vector)
    {
        ref var tile = ref _tiles[location];
        if (tile.Id == _elementList.SliderEwId && vector.Y == 0 ||
            tile.Id == _elementList.SliderNsId && vector.X == 0 ||
            _elementList[tile.Id].IsPushable)
        {
            // this is here to prevent endless push loops
            // but doesn't exist in the original code
            if (vector.IsZero())
                throw Exceptions.PushStackOverflow;

            ref var furtherTile = ref _tiles[location + vector];
            if (furtherTile.Id == _elementList.TransporterId)
                PushThroughTransporter(location, vector);
            else if (furtherTile.Id != _elementList.EmptyId)
                Push(location + vector, vector);

            var furtherElement = _elementList[furtherTile.Id];
            if (!furtherElement.IsFloor && furtherElement.IsDestructible && furtherTile.Id != _elementList.PlayerId)
                Destroy(location + vector);

            furtherElement = _elementList[furtherTile.Id];
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
                if (element.Id == _elementList.BoardEdgeId)
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

                if (element.Id == _elementList.TransporterId)
                    if (ActorAt(search).Vector == -vector)
                        success = true;
            }

            if (target.X > 0)
            {
                MoveTile(actor.Location - vector, target);
                _soundUnit.PlaySound(3, _sounds.Transporter);
            }
        }
    }

    public void PutTile(Location location, Vector vector, Tile kind)
    {
        if (!_features.CanPutTile(location))
            return;

        if (location.X >= 1 && location.X <= _tiles.Width && location.Y >= 1 &&
            location.Y <= _tiles.Height)
        {
            if (!ElementAt(location).IsFloor) Push(location, vector);
            PlotTile(location, kind);
        }
    }

    public void RaiseError(ref OopContext context, ReadOnlySpan<char> error)
    {
        SetMessage(_facts.LongMessageDuration, _alerts.ErrorMessage(error));
        _soundUnit.PlaySound(5, _sounds.Error);
        _tracer.TraceError(ref context, error);
        _actorList[context.Index].Instruction = -1;
    }

    public void RemoveActor(int index)
    {
        var actor = _actorList[index];
        var freeCode = actor.Length > 0 && actor.Pointer != 0;

        if (index < _state.ActIndex)
            _state.ActIndex--;

        _tiles[actor.Location] = actor.UnderTile;

        if (actor.Location.Y > 0)
            UpdateBoard(actor.Location);

        var pointer = actor.Pointer;

        for (var i = 1; i <= _state.ActorCount; i++)
        {
            var a = _actorList[i];
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
            _heap.Free(pointer);
            actor.Pointer = 0;
        }

        if (index < _state.ActorCount)
            for (var i = index; i < _state.ActorCount; i++)
                _actorList[i].CopyFrom(_actorList[i + 1]);

        _state.ActorCount--;
    }

    public void RemoveItem(Location location) => _features.RemoveItem(location);

    public Vector Rnd()
    {
        var result = new Vector
        {
            X = _randomizer.GetNext(3) - 1
        };

        result.Y = result.X == 0 ? (_randomizer.GetNext(2) << 1) - 1 : 0;
        return result;
    }

    public Vector RndP(Vector vector) =>
        _randomizer.GetNext(2) == 0
            ? vector.Clockwise()
            : vector.CounterClockwise();

    public Vector Seek(Location location)
    {
        var result = new Vector();
        if (_randomizer.GetNext(2) == 0 || _actorList.Player.Location.Y == location.Y)
            result.X = (_actorList.Player.Location.X - location.X).Polarity();

        if (result.X == 0) result.Y = (_actorList.Player.Location.Y - location.Y).Polarity();

        if (_world.EnergyCycles > 0) result = -result;

        return result;
    }

    public void SetEditorMode()
    {
        InitializeElements(true);
        _state.EditorMode = true;
    }

    public void SetGameMode()
    {
        InitializeElements(false);
        _state.EditorMode = false;
    }

    public void SetMessage(int duration, IMessage message)
    {
        var index = ActorIndexAt(new Location(0, 0));
        if (index >= 0)
        {
            RemoveActor(index);
            _hud.UpdateBorder();
        }

        var topMessage = message.Text[0];
        var bottomMessage = message.Text.Count > 1 ? message.Text[1] : string.Empty;

        SpawnActor(new Location(0, 0), new Tile(_elementList.MessengerId, 0), 1, _state.DefaultActor);
        _actorList[_state.ActorCount].P2 = unchecked((byte)(duration / (_state.GameWaitTime + 1)));
        _state.Message = topMessage;
        _state.Message2 = bottomMessage;
    }

    public void ShowHelp(string title, string filename) => _hud.ShowHelp(title, filename);

    public void ShowInGameHelp() => _features.ShowInGameHelp();

    public void SpawnActor(Location location, Tile tile, int cycle, IActor? source)
    {
        // must reserve one actor for player, and one for messenger
        if (_state.ActorCount < _actorList.Capacity - 2)
        {
            _state.ActorCount++;
            var actor = _actorList[_state.ActorCount];

            source ??= _state.DefaultActor;

            actor.CopyFrom(source);
            actor.Location = location;
            actor.Cycle = cycle;
            actor.UnderTile = _tiles[location];
            actor.Instruction = 0;

            if (ElementAt(actor.Location).IsEditorFloor)
            {
                var newColor = _tiles[actor.Location].Color & 0x70;
                newColor |= tile.Color & 0x0F;
                _tiles[actor.Location].Color = newColor;
            }
            else
            {
                _tiles[actor.Location].Color = tile.Color;
            }

            _tiles[actor.Location].Id = tile.Id;
            if (actor.Location.Y > 0) UpdateBoard(actor.Location);
        }
    }

    public bool SpawnProjectile(int id, Location location, Vector vector, bool enemyOwned)
    {
        var target = location + vector;
        var element = ElementAt(target);

        if (element.IsFloor || element.Id == _elementList.WaterId)
        {
            SpawnActor(target, new Tile(id, _elementList[id].Color), 1, _state.DefaultActor);
            var actor = _actorList[_state.ActorCount];
            actor.P1 = unchecked((byte)(enemyOwned ? 1 : 0));
            actor.Vector = vector;
            actor.P2 = 0x64;
            return true;
        }

        if (element.Id != _elementList.BreakableId &&
            (!element.IsDestructible ||
             element.Id == _elementList.PlayerId != enemyOwned ||
             _world.EnergyCycles != 0))
            return false;

        Destroy(target);
        _soundUnit.PlaySound(2, _sounds.BulletDie);
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

    public void Stop()
    {
        Thread = null;
    }

    public bool TitleScreen => _state.PlayerElement != _elementList.PlayerId;

    public void UnlockActor(int index) => _features.UnlockActor(index);

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
            if (x >= 1 && x <= _tiles.Width && y >= 1 && y <= _tiles.Height)
            {
                var target = new Location(x, y);
                if (mode != RadiusMode.Update)
                    if (Distance(source, target) < _facts.TorchRadius)
                    {
                        var element = ElementAt(target);
                        if (mode == RadiusMode.Explode)
                        {
                            if (element.CanContainCode)
                            {
                                var actorIndex = ActorIndexAt(target);
                                if (actorIndex > 0) BroadcastLabel(-actorIndex, _facts.BombedLabel, false);
                            }

                            if (element.IsDestructible || element.Id == _elementList.StarId) Destroy(target);

                            if (element.Id == _elementList.EmptyId || element.Id == _elementList.BreakableId)
                                _tiles[target] = new Tile(_elementList.BreakableId, _randomizer.GetNext(7) + 9);
                        }
                        else
                        {
                            if (_tiles[target].Id == _elementList.BreakableId) _tiles[target].Id = _elementList.EmptyId;
                        }
                    }

                UpdateBoard(target);
            }
    }

    public void UpdateStatus() => _hud.UpdateStatus();

    private void UpdateSound()
    {
        if (!_state.SoundPlaying)
        {
            _state.SoundBuffer.Clear();
            return;
        }

        if (_state.SoundTicks <= 0)
        {
            if (_state.SoundBuffer.Count > 0)
            {
                var sound = _state.SoundBuffer.Dequeue();
                _state.SoundTicks = sound.Duration << 2;
                if (sound.Note >= 0xF0)
                {
                    _speaker.PlayDrum(sound.Note - 0xF0);
                }
                else if (sound.Note > 0x00)
                {
                    var actualNote = (sound.Note & 0xF) + (sound.Note >> 4) * 12;
                    _speaker.PlayNote(actualNote);
                }
                else
                {
                    _speaker.StopNote();
                }
            }
            else
            {
                _state.SoundPlaying = false;
                _state.SoundPriority = 0;
                _speaker.StopNote();
            }
        }

        if (_state.SoundPlaying)
            _state.SoundTicks--;
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
        var isFast = _state.GameWaitTime <= 0 && _config.FastMode;

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

    private bool ActorIsLocked(int index) => _features.IsActorLocked(index);

    private int ColorMatch(Tile tile)
    {
        var element = _elementList[tile.Id];

        if (element.Color < 0xF0)
            return element.Color & 7;
        if (element.Color == 0xFE)
            return ((tile.Color >> 4) & 0x0F) + 8;
        return tile.Color & 0x0F;
    }

    private static int Distance(Location a, Location b) => (a.Y - b.Y).Square() * 2 + (a.X - b.X).Square();

    private void DrawTile(Location location, AnsiChar ac) => _hud.DrawTile(location.X - 1, location.Y - 1, ac);

    private void EnterHighScore(int score)
    {
        var list = _highScoreListFactory.Load();
        var name = _hud.EnterHighScore(list, score);
        if (name == null)
            return;

        list.Add(name, score);
        _highScoreListFactory.Save(list);
        ShowHighScores();
    }

    private void ExecuteMessage(ref OopContext context)
    {
        var result = _features.ExecuteMessage(ref context);
        if (result is { Cancelled: false, Label: not null })
            context.NextLine = BroadcastLabel(context.Index, result.Label, false);
    }

    private void FadeBoard(AnsiChar ac) => _hud.FadeBoard(ac);

    public void FadeRed()
    {
        FadeBoard(_facts.ErrorFadeTile);
        _hud.RedrawBoard();
    }

    private Vector GetConveyorVector(int index) => new(_state.Vector8[index], _state.Vector8[index + 8]);

    private void InitializeElements(bool showInvisibleTiles)
    {
        _elementList.Reset();
        _elementList.Invisible().Character = showInvisibleTiles ? 0xB0 : 0x20;
        _elementList.Invisible().Color = 0xFF;
        _elementList.Player().Character = 0x02;
    }

    private void MainLoop(bool doFade)
    {
        var alternating = false;

        if (!_step)
        {
            _hud.CreateStatusText();
            _hud.UpdateStatus();
            MainLoopInit(doFade);
        }

        _state.BreakGameLoop = false;

        while (ThreadActive)
        {
            if (!_state.GamePaused)
            {
                if (_state.ActIndex <= _state.ActorCount)
                {
                    var actorData = _actorList[_state.ActIndex];
                    if (actorData.Cycle != 0)
                        if (_state.ActIndex % actorData.Cycle == _state.GameCycle % actorData.Cycle)
                            _actionList.Get(_tiles[actorData.Location].Id)?.Act(_state.ActIndex);

                    _state.ActIndex++;
                }
            }
            else
            {
                _state.ActIndex = _state.ActorCount + 1;

                if (_timers.Player.Clock(1, HsecToTicks(25)) > 0)
                    alternating = !alternating;

                if (alternating)
                {
                    var playerElement = _elementList.Player();
                    DrawTile(_actorList.Player.Location, new AnsiChar(playerElement.Character, playerElement.Color));
                }
                else
                {
                    if (_tiles[_actorList.Player.Location].Id == _elementList.PlayerId)
                        DrawTile(_actorList.Player.Location, new AnsiChar(0x20, 0x0F));
                    else
                        UpdateBoard(_actorList.Player.Location);
                }

                _hud.DrawPausing();
                ReadInput(false);
                if (_state.KeyPressed == EngineKeyCode.Escape)
                {
                    if (_world.Health > 0)
                    {
                        _state.BreakGameLoop = _hud.EndGameConfirmation();
                    }
                    else
                    {
                        _state.BreakGameLoop = true;
                        _hud.UpdateBorder();
                    }

                    _state.KeyPressed = 0;
                }

                if (!_state.KeyVector.IsZero())
                {
                    var target = _actorList.Player.Location + _state.KeyVector;
                    _interactionList.Get(ElementAt(target).Id)?.Interact(target, 0, ref _state.KeyVector);
                }

                if (!_state.KeyVector.IsZero())
                {
                    var target = _actorList.Player.Location + _state.KeyVector;
                    if (ElementAt(target).IsFloor)
                    {
                        _features.CleanUpPauseMovement();
                        _state.GamePaused = false;
                        _hud.ClearPausing();
                        _state.GameCycle = _randomizer.GetNext(_facts.MainLoopRandomCycleRange);
                        _world.IsLocked = true;
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

            if (_state.ActIndex > _state.ActorCount)
            {
                if (!_state.BreakGameLoop && !_state.GamePaused)
                    if (_state.GameWaitTime <= 0 || _timers.Player.Clock(1, _state.GameWaitTime) > 0)
                    {
                        _state.GameCycle++;
                        if (_state.GameCycle > _facts.MaxGameCycle) _state.GameCycle = 1;

                        _state.ActIndex = 0;
                        ReadInput(false);
                    }

                _tracer.TraceStep();
                if (_step)
                    break;

                WaitForTick();
            }

            if (_state.BreakGameLoop)
            {
                _soundUnit.ClearSound();
                if (_state.PlayerElement == _elementList.PlayerId)
                {
                    if (_world.Health <= 0) EnterHighScore(_world.Score);
                }
                else if (_state.PlayerElement == _elementList.MonitorId)
                {
                    _hud.ClearTitleStatus();
                }

                var element = _elementList.Player();
                _tiles[_actorList.Player.Location] = new Tile(element.Id, element.Color);
                _state.GameOver = false;
                break;
            }
        }
    }

    private void MainLoopInit(bool doFade)
    {
        if (_state.Init)
        {
            if (!_state.AboutShown)
                ShowAbout();

            if (!ThreadActive)
                return;

            if (_state.DefaultWorldName.Length > 0)
            {
                _state.AboutShown = true;
                _worldUnit.LoadWorld(_state.DefaultWorldName, false);
            }

            _state.StartBoard = _world.BoardIndex;
            _worldUnit.SetBoard(0);
            _state.Init = false;
        }

        var element = _elementList[_state.PlayerElement];
        _tiles[_actorList.Player.Location] = new Tile(element.Id, element.Color);
        if (_state.PlayerElement == _elementList.MonitorId)
        {
            SetMessage(0, new Message());
            _hud.DrawTitleStatus();
        }

        if (doFade)
            FadePurple();

        _state.GameWaitTime = _state.GameSpeed << 1;
        _state.GameCycle = _randomizer.GetNext(_facts.MainLoopRandomCycleRange);
        _state.ActIndex = _state.ActorCount + 1;
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
            _tiles[target] = _tiles[source];
            UpdateBoard(target);
            RemoveItem(source);
            UpdateBoard(source);
        }
    }

    private void StartPlaying()
    {
        _worldUnit.SetBoard(_state.StartBoard);
        _features.EnterBoard();
        _state.PlayerElement = _elementList.PlayerId;
        _state.GamePaused = true;
        MainLoop(true);
    }

    private bool PlayWorld()
    {
        var gameIsActive = false;

        if (_world.IsLocked)
        {
            _worldUnit.LoadWorld(_world.Name, false);

            if (_state.WorldLoaded)
            {
                gameIsActive = _state.WorldLoaded;
                _state.StartBoard = _world.BoardIndex;
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
        var actor = _actorList[index];
        var value = (char)0;

        if (instruction < 0 || instruction >= actor.Length)
        {
            _state.OopByte = default;
        }
        else
        {
            value = actor.Code.Span[instruction];
            _state.OopByte = value;
            instruction++;
        }

        return value;
    }

    private EngineKeyCode ConvertKey(KeyPress keyPress)
    {
        var bytes = _ansiKeyTransformer.GetBytes(keyPress);

        if (bytes.IsEmpty)
            return EngineKeyCode.None;

        if (bytes.Length > 1 && (bytes[0] == 0 || bytes[0] >= 0x80))
            return (EngineKeyCode)(bytes[1] | 0x80);

        return (EngineKeyCode)bytes[0];
    }

    private void ReadInputJoystick(bool isUiFocused)
    {
        if (_config.DisableJoystick || !_joystick.IsConnected)
            return;

        // This function does things a lot differently than the original engine,
        // mostly for convenience in controls.

        var x = 0f;
        var y = 0f;
        JoystickButtons buttons = 0;

        if (_joystick.IsConnected)
        {
            x = _joystick.X;
            y = _joystick.Y;
            buttons = _joystick.Buttons;
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

        var deadZone = _config.JoystickDeadZone;
        var maxMagnitude = 0f;
        var finalKeyCode = (EngineKeyCode)0;

        if (x <= -deadZone & x <= -maxMagnitude)
        {
            _state.KeyVector = Vector.West;
            maxMagnitude = Math.Max(maxMagnitude, Math.Abs(x));
            finalKeyCode = EngineKeyCode.Left;
        }

        if (x >= deadZone && x >= maxMagnitude)
        {
            _state.KeyVector = Vector.East;
            maxMagnitude = Math.Max(maxMagnitude, Math.Abs(x));
            finalKeyCode = EngineKeyCode.Right;
        }

        if (y <= -deadZone && y <= -maxMagnitude)
        {
            _state.KeyVector = Vector.North;
            maxMagnitude = Math.Max(maxMagnitude, Math.Abs(x));
            finalKeyCode = EngineKeyCode.Up;
        }

        if (y >= deadZone && y >= maxMagnitude)
        {
            _state.KeyVector = Vector.South;
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
            _state.KeyPressed = EngineKeyCode.Left;
        else if (singleButtons.HasFlag(JoystickButtons.Right))
            _state.KeyPressed = EngineKeyCode.Right;
        else if (singleButtons.HasFlag(JoystickButtons.Up))
            _state.KeyPressed = EngineKeyCode.Up;
        else if (singleButtons.HasFlag(JoystickButtons.Down))
            _state.KeyPressed = EngineKeyCode.Down;

        // Process button actions.

        if (buttons.HasFlag(JoystickButtons.Ok))
        {
            if (isUiFocused)
            {
                _state.KeyPressed = EngineKeyCode.Enter;
            }
            else
            {
                if (_state.KeyPressed != EngineKeyCode.None)
                    _state.KeyShift = true;
                else
                    _state.KeyPressed = EngineKeyCode.Space;
            }
        }
        else if (buttons.HasFlag(JoystickButtons.Cancel))
        {
            if (isUiFocused)
                _state.KeyPressed = EngineKeyCode.Escape;
        }
        else if (buttons.HasFlag(JoystickButtons.Shoot))
        {
            if (!isUiFocused)
                _state.KeyShift = true;
        }

        if (isUiFocused && singleButtons.HasFlag(JoystickButtons.PageUp))
        {
            _state.KeyPressed = EngineKeyCode.PageUp;
        }
        else if (isUiFocused && singleButtons.HasFlag(JoystickButtons.PageDown))
        {
            _state.KeyPressed = EngineKeyCode.PageDown;
        }
        else if (singleButtons.HasFlag(JoystickButtons.Start))
        {
            // If on the title screen, Start will begin the game.
            // Otherwise, it will pause the game.

            if (_state.PlayerElement == _elementList.MonitorId)
                _state.KeyPressed = _facts.StartGameKey;
            else
                _state.KeyPressed = EngineKeyCode.P;
        }

        _lastButtons = buttons;
    }

    private void ReadInputKeyboard()
    {
        var mod = _keyboard.GetMod();
        _state.KeyShift = mod.HasFlag(KeyMod.Shift);
        _state.KeyPressed = 0;
        _state.KeyVector = Vector.Idle;

        if (!_keyboard.KeyIsAvailable)
            return;

        var key = _keyboard.GetKey();
        if (key is not { } keyValue || keyValue.Key == AnsiKey.None)
            return;

        _state.KeyPressed = ConvertKey(keyValue);

        switch (_state.KeyPressed)
        {
            case EngineKeyCode.Left:
                _state.KeyVector = Vector.West;
                break;
            case EngineKeyCode.Right:
                _state.KeyVector = Vector.East;
                break;
            case EngineKeyCode.Up:
                _state.KeyVector = Vector.North;
                break;
            case EngineKeyCode.Down:
                _state.KeyVector = Vector.South;
                break;
        }
    }

    public void ReadInput(bool isUiFocused)
    {
        ReadInputKeyboard();
        if (_state.KeyVector.IsZero())
            ReadInputJoystick(isUiFocused);
        if (_state.KeyVector.IsNonZero())
            _state.KeyLastVector = _state.KeyVector;
    }

    private void ShowAbout() => _features.ShowAbout();

    private void StartInit()
    {
        _state.GameSpeed = _facts.DefaultGameSpeed;
        _state.GameWaitTime = 1;
        _state.DefaultSaveName = _facts.DefaultSavedGameName;
        _state.DefaultBoardName = _facts.DefaultBoardName;
        _state.DefaultWorldName = _config.DefaultWorld ?? _facts.DefaultWorldName;
        _state.ForestIndex = 2;
        _state.Init = true;

        _worldUnit.ClearWorld();

        var cfg = _configFileService.Load();
        if (_config.DefaultWorld == null && cfg != null)
        {
            if (!string.IsNullOrEmpty(cfg.WorldName))
            {
                _state.DefaultWorldName = (
                    cfg.WorldName?.StartsWith("*") ?? false
                        ? cfg.WorldName.Substring(1)
                        : cfg.WorldName
                ) ?? string.Empty;
            }
        }

        SetGameMode();
        _clock.Start();
    }

    private void StartMain()
    {
        _clock.OnTick += ClockTick;
        StartInit();
        TitleScreenLoop();
        _clock.OnTick -= ClockTick;
        Exited?.Invoke(this, EventArgs.Empty);
    }

    private void TitleScreenLoop()
    {
        _state.QuitEngine = false;
        _state.Init = true;
        _state.StartBoard = 0;
        var gameEnded = true;
        _hud.Initialize();
        while (ThreadActive)
        {
            if (!_state.Init)
                _worldUnit.SetBoard(0);

            while (ThreadActive)
            {
                _state.PlayerElement = _elementList.MonitorId;
                _state.GamePaused = false;
                MainLoop(gameEnded);
                gameEnded = false;

                if (!ThreadActive)
                    break;

                var startPlaying = _features.HandleTitleInput();
                if (startPlaying)
                    gameEnded = PlayWorld();

                if (gameEnded || _state.QuitEngine)
                    break;
            }

            if (_state.QuitEngine) break;
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
        _clock.Stop();
}