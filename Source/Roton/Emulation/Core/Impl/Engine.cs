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
internal sealed class Engine : IEngine, IDisposable
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
    private readonly IBoardUpdater _boardUpdater;
    private readonly IPlayField _playField;
    private readonly IBroadcaster _broadcaster;
    private readonly IRadiusUpdater _radiusUpdater;
    private readonly IPusher _pusher;
    private readonly IMessageHandler _messageHandler;
    private readonly ISpawner _spawner;
    private readonly IMover _mover;
    private readonly IPlayerUpdater _playerUpdater;
    private readonly IMessenger _messenger;
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
        IBoardTime boardTime, IBoardUpdater boardUpdater, IPlayField playField, IBroadcaster broadcaster,
        IRadiusUpdater radiusUpdater, IPusher pusher, IMessageHandler messageHandler,
        ISpawner spawner, IMover mover, IPlayerUpdater playerUpdater, IMessenger messenger)
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
        _boardUpdater = boardUpdater;
        _playField = playField;
        _broadcaster = broadcaster;
        _radiusUpdater = radiusUpdater;
        _pusher = pusher;
        _messageHandler = messageHandler;
        _spawner = spawner;
        _mover = mover;
        _playerUpdater = playerUpdater;
        _messenger = messenger;

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
            switch (cheatText[0])
            {
                case '-':
                {
                    cheatText = cheatText.Substring(1);
                    while (_world.Flags.Contains(cheatText))
                        _world.Flags.Remove(cheatText);
                    clear = true;
                    break;
                }
                case '+':
                    cheatText = cheatText.Substring(1);
                    _world.Flags.Add(cheatText);
                    break;
            }
        }

        var cheat = _cheats.Get(cheatText);
        cheat?.Execute(clear);
        _hud.UpdateStatus();

        _soundUnit.PlaySound(10, _sounds.Cheat);
    }

    public void ShowHighScores()
    {
        var list = _highScoreListFactory.Load();
        _hud.ShowHighScores(list);
    }

    public event EventHandler? Exited;
    public event EventHandler? Tick;

    public void Attack(int index, Location location)
    {
        if (index == 0 && _world.EnergyCycles > 0)
        {
            _world.Score += ElementAt(location).Points;
            _hud.UpdateStatus();
        }
        else
        {
            Harm(index);
        }

        if (index > 0 && index <= _state.ActIndex) _state.ActIndex--;

        if (_tiles[location].Id == _elementList.PlayerId && _world.EnergyCycles > 0)
        {
            _world.Score += ElementAt(_actorList[index].Location).Points;
            _hud.UpdateStatus();
        }
        else
        {
            Destroy(location);
            _soundUnit.PlaySound(2, _sounds.EnemySuicide);
        }
    }

    public void Destroy(Location location)
    {
        var index = _actorList.ActorIndexAt(location);
        if (index == -1)
            _features.RemoveItem(location);
        else
            Harm(index);
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
            _features.CleanUpOop(ref context);
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

    public void FadePurple()
    {
        FadeBoard(_facts.FadeTile);
        _hud.RedrawBoard();
    }

    public bool FindTile(Tile kind, Location location)
    {
        var matchColor = _features.GetColorMatchValue(kind.Color);

        location.X++;
        while (location.Y <= _tiles.Height)
        {
            while (location.X <= _tiles.Width)
            {
                ref var tile = ref _tiles[location];
                if (tile.Id == kind.Id)
                {
                    var foundColor = _features.GetColorMatchValue(ColorMatch(_tiles[location]));
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

    public Vector GetCardinalVector(int index) => new(_state.Vector4[index], _state.Vector4[index + 4]);

    public void Harm(int index)
    {
        var actor = _actorList[index];
        if (index == 0)
        {
            if (_world.Health > 0)
            {
                _world.Health -= _facts.HealthLostPerHit;
                _hud.UpdateStatus();
                _messenger.SetMessage(_facts.ShortMessageDuration, _alerts.OuchMessage);
                _tiles[actor.Location].Color = (ElementAt(actor.Location).Color & 0x0F) | 0x70;

                if (_world.Health > 0)
                {
                    _world.TimePassed = 0;
                    if (_board.RestartOnZap)
                    {
                        _soundUnit.PlaySound(4, _sounds.TimeOut);
                        _features.RemoveItem(actor.Location);
                        var oldLocation = actor.Location;
                        actor.Location = _board.Entrance;
                        _radiusUpdater.UpdateRadius(oldLocation, 0);
                        _radiusUpdater.UpdateRadius(actor.Location, 0);
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
                _spawner.SpawnActor(location, new Tile(targetElement.Id, targetColor), targetElement.Cycle,
                    _state.DefaultActor);
        }

        _boardUpdater.UpdateBoard(location);
    }

    public void PutTile(Location location, Vector vector, Tile kind)
    {
        if (!_features.CanPutTile(location))
            return;

        if (location.X >= 1 && location.X <= _tiles.Width && location.Y >= 1 &&
            location.Y <= _tiles.Height)
        {
            if (!ElementAt(location).IsFloor)
                _pusher.Push(location, vector);
            PlotTile(location, kind);
        }
    }

    public void RaiseError(ref OopContext context, ReadOnlySpan<char> error)
    {
        _messenger.SetMessage(_facts.LongMessageDuration, _alerts.ErrorMessage(error));
        _soundUnit.PlaySound(5, _sounds.Error);
        _tracer.TraceError(ref context, error);
        _actorList[context.Index].Instruction = -1;
    }

    public void RemoveActor(int index)
    {
        if (index < 0)
        {
            _tracer.TraceCrash("Attempted to remove invalid actor index");
            return;
        }

        var actor = _actorList[index];
        var freeCode = actor.Length > 0 && actor.Pointer != 0;

        if (index < _state.ActIndex)
            _state.ActIndex--;

        _tiles[actor.Location] = actor.UnderTile;

        if (actor.Location.Y > 0)
            _boardUpdater.UpdateBoard(actor.Location);

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

    private void SetGameMode()
    {
        InitializeElements(false);
        _state.EditorMode = false;
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
                switch (sound.Note)
                {
                    case >= 0xF0:
                    {
                        _speaker.PlayDrum(sound.Note - 0xF0);
                        break;
                    }
                    case > 0x00:
                    {
                        var actualNote = (sound.Note & 0xF) + (sound.Note >> 4) * 12;
                        _speaker.PlayNote(actualNote);
                        break;
                    }
                    default:
                    {
                        _speaker.StopNote();
                        break;
                    }
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

    private int ColorMatch(Tile tile)
    {
        var element = _elementList[tile.Id];

        if (element.Color < 0xF0)
            return element.Color & 7;
        if (element.Color == 0xFE)
            return ((tile.Color >> 4) & 0x0F) + 8;
        return tile.Color & 0x0F;
    }

    private void DrawTile(Location location, AnsiChar ac) => _playField.DrawTile(location.X - 1, location.Y - 1, ac);

    private void EnterHighScore(int score)
    {
        if (score <= 0)
            return;

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
        var result = _messageHandler.ExecuteMessage(ref context);
        if (result is { Cancelled: false, Label: not null })
            context.NextLine = _broadcaster.BroadcastLabel(context.Index, result.Label, false);
    }

    private void FadeBoard(AnsiChar ac) => _hud.FadeBoard(ac);

    public void FadeRed()
    {
        FadeBoard(_facts.ErrorFadeTile);
        _hud.RedrawBoard();
    }

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
                        _boardUpdater.UpdateBoard(_actorList.Player.Location);
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
                        if (_tiles.ElementAt(_actorList.Player.Location).Id == _elementList.PlayerId)
                        {
                            _mover.MoveActor(0, target);
                        }
                        else
                        {
                            _boardUpdater.UpdateBoard(_actorList.Player.Location);
                            _actorList.Player.Location += _state.KeyVector;
                            _playerUpdater.CleanUpPauseMovement();
                            _tiles[_actorList.Player.Location] = new Tile(_elementList.PlayerId, _elementList.Player().Color);
                            _boardUpdater.UpdateBoard(_actorList.Player.Location);
                            _radiusUpdater.UpdateRadius(_actorList.Player.Location, RadiusMode.Update);
                            _radiusUpdater.UpdateRadius(_actorList.Player.Location - _state.KeyVector, RadiusMode.Update);
                        }

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
                    // This game speed reset isn't here in the original code,
                    // but it solves some issues with game speed when returning
                    // to the title screen.

                    ResetGameSpeed();

                    if (_world.Health <= 0)
                        EnterHighScore(_world.Score);
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
                _features.ShowAbout();

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
            _messenger.SetMessage(0, new Message());
            _hud.DrawTitleStatus();
        }

        if (doFade)
            FadePurple();

        ResetGameSpeed();
        _state.GameCycle = _randomizer.GetNext(_facts.MainLoopRandomCycleRange);
        _state.ActIndex = _state.ActorCount + 1;
    }

    private void ResetGameSpeed() =>
        _state.GameWaitTime = _state.GameSpeed << 1;

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
            value = actor.Code[instruction];
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

        _state.KeyVector = _state.KeyPressed switch
        {
            EngineKeyCode.Left => Vector.West,
            EngineKeyCode.Right => Vector.East,
            EngineKeyCode.Up => Vector.North,
            EngineKeyCode.Down => Vector.South,
            _ => _state.KeyVector
        };
    }

    public void ReadInput(bool isUiFocused)
    {
        ReadInputKeyboard();
        if (_state.KeyVector.IsZero())
            ReadInputJoystick(isUiFocused);
        if (_state.KeyVector.IsNonZero())
            _state.KeyLastVector = _state.KeyVector;
    }

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