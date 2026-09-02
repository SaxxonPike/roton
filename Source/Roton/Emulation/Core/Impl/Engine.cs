using System;
using System.Threading;
using Roton.Emulation.Actions;
using Roton.Emulation.Cheats;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Interactions;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Engine : IEngine, IDisposable
{
    private readonly IClock _clock;
    private readonly IActorList _actors;
    private readonly IAlerts _alerts;
    private readonly IBoard _board;
    private readonly IElementList _elements;
    private readonly IInterpreter _interpreter;
    private readonly IRandomizer _randomizer;
    private readonly ITiles _tiles;
    private readonly ISounds _sounds;
    private readonly ITimers _timers;
    private readonly IParser _parser;
    private readonly IConfig _config;
    private readonly ICheatList _cheats;
    private readonly IHud _hud;
    private readonly IState _state;
    private readonly IWorld _world;
    private readonly IActionList _actionList;
    private readonly IInteractionList _interactions;
    private readonly IFacts _facts;
    private readonly ISpeaker _speaker;
    private readonly IObjectMover _objectMover;
    private readonly IHighScoreListFactory _highScoreListFactory;
    private readonly IConfigFileService _configFileService;
    private readonly ITracer _tracer;
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
    private readonly IScheduler _scheduler;
    private readonly IColorMatcher _colorMatcher;
    private readonly IDialogs _dialogs;
    private readonly IInputReader _inputReader;
    private readonly IPlayerEnterHandler _playerEnterHandler;
    private readonly IPlayerInputHandler _playerInputHandler;
    private readonly ITileRemover _tileRemover;
    private readonly IActorRemover _actorRemover;
    private readonly IFader _fader;

    private bool _step;

    public Engine(
        IClock clock,
        IActorList actors,
        IAlerts alerts,
        IBoard board,
        IElementList elements,
        IInterpreter interpreter,
        IRandomizer randomizer,
        ITiles tiles,
        ISounds sounds,
        ITimers timers,
        IParser parser,
        IConfig config,
        ICheatList cheats,
        IHud hud,
        IState state,
        IWorld world,
        IActionList actionList,
        IInteractionList interactions,
        IFacts facts,
        ISpeaker speaker,
        IObjectMover objectMover,
        IHighScoreListFactory highScoreListFactory,
        IConfigFileService configFileService,
        ITracer tracer,
        IEngineAccessor engineAccessor,
        ISoundUnit soundUnit,
        IWorldUnit worldUnit,
        IBoardTime boardTime,
        IBoardUpdater boardUpdater,
        IPlayField playField,
        IBroadcaster broadcaster,
        IRadiusUpdater radiusUpdater,
        IPusher pusher,
        IMessageHandler messageHandler,
        ISpawner spawner,
        IMover mover,
        IPlayerUpdater playerUpdater,
        IMessenger messenger,
        IScheduler scheduler,
        IColorMatcher colorMatcher,
        IDialogs dialogs,
        IInputReader inputReader,
        IPlayerEnterHandler playerEnterHandler,
        IPlayerInputHandler playerInputHandler,
        ITileRemover tileRemover,
        IActorRemover actorRemover,
        IFader fader)
    {
        engineAccessor.Instance = this;

        _clock = clock;
        _actors = actors;
        _alerts = alerts;
        _board = board;
        _elements = elements;
        _interpreter = interpreter;
        _randomizer = randomizer;
        _tiles = tiles;
        _sounds = sounds;
        _timers = timers;
        _parser = parser;
        _config = config;
        _cheats = cheats;
        _hud = hud;
        _state = state;
        _world = world;
        _actionList = actionList;
        _interactions = interactions;
        _facts = facts;
        _speaker = speaker;
        _objectMover = objectMover;
        _highScoreListFactory = highScoreListFactory;
        _configFileService = configFileService;
        _tracer = tracer;
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
        _scheduler = scheduler;
        _colorMatcher = colorMatcher;
        _dialogs = dialogs;
        _inputReader = inputReader;
        _playerEnterHandler = playerEnterHandler;
        _playerInputHandler = playerInputHandler;
        _tileRemover = tileRemover;
        _actorRemover = actorRemover;
        _fader = fader;
    }

    private Thread? Thread { get; set; }

    public bool ThreadActive => Thread != null || _step;

    public void Cheat()
    {
        var cheatText = _hud.EnterCheat().UpCased() ?? "";
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

    public void Attack(int index, Location location)
    {
        if (index == 0 && _world.EnergyCycles > 0)
        {
            _world.Score += _tiles.ElementAt(location).Points;
            _hud.UpdateStatus();
        }
        else
        {
            Harm(index);
        }

        if (index > 0 && index <= _state.ActIndex) _state.ActIndex--;

        if (_tiles[location].Id == _elements.PlayerId && _world.EnergyCycles > 0)
        {
            _world.Score += _tiles.ElementAt(_actors[index].Location).Points;
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
        var index = _actors.ActorIndexAt(location);
        if (index == -1)
            _tileRemover.RemoveItem(location);
        else
            Harm(index);
    }

    public void ExecuteCode(int index, ref Word instruction, string name)
    {
        var context = new OopContext(_actors)
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
            _tileRemover.RemoveActor(context.Actor.Location, context.Index, context.DeathTile);
    }

    public void StepOnce()
    {
        _step = true;
        MainLoop(true);
        _step = false;
    }

    public bool FindTile(Tile kind, Location location)
    {
        var matchColor = _colorMatcher.GetColorMatchValue(kind.Color);

        location.X++;
        while (location.Y <= _tiles.Height)
        {
            while (location.X <= _tiles.Width)
            {
                ref var tile = ref _tiles[location];
                if (tile.Id == kind.Id)
                {
                    var foundColor = _colorMatcher.GetColorMatchValue(ColorMatch(_tiles[location]));
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

    public void Harm(int index)
    {
        var actor = _actors[index];
        if (index == 0)
        {
            if (_world.Health > 0)
            {
                _world.Health -= _facts.HealthLostPerHit;
                _hud.UpdateStatus();
                _messenger.SetMessage(_facts.ShortMessageDuration, _alerts.OuchMessage);
                _tiles[actor.Location].Color = (_tiles.ElementAt(actor.Location).Color & 0x0F) | 0x70;

                if (_world.Health > 0)
                {
                    _world.TimePassed = 0;
                    if (_board.RestartOnZap)
                    {
                        _soundUnit.PlaySound(4, _sounds.TimeOut);
                        _tileRemover.RemoveItem(actor.Location);
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
            if (element == _elements.BulletId)
                _soundUnit.PlaySound(3, _sounds.BulletDie);
            else if (element != _elements.ObjectId) _soundUnit.PlaySound(3, _sounds.EnemyDie);

            _actorRemover.RemoveActor(index);
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
        if (_tiles.ElementAt(location).Id == _elements.PlayerId)
            return;

        var targetElement = _elements[tile.Id];
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
        if (!_tiles.CanPutTile(location))
            return;

        if (location.X >= 1 && location.X <= _tiles.Width && location.Y >= 1 &&
            location.Y <= _tiles.Height)
        {
            if (!_tiles.ElementAt(location).IsFloor)
                _pusher.Push(location, vector);
            PlotTile(location, kind);
        }
    }

    public void RaiseError(ref OopContext context, ReadOnlySpan<char> error)
    {
        _messenger.SetMessage(_facts.LongMessageDuration, _alerts.ErrorMessage(error));
        _soundUnit.PlaySound(5, _sounds.Error);
        _tracer.TraceError(ref context, error);
        _actors[context.Index].Instruction = -1;
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
            _scheduler.Reset();
            Thread = new Thread(StartMain);
            Thread.Start();
        }
    }

    public void Stop()
    {
        Thread = null;
    }

    public void UpdateSound()
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

    private int ColorMatch(Tile tile)
    {
        var element = _elements[tile.Id];

        if (element.Color < 0xF0)
            return element.Color & 7;
        if (element.Color == 0xFE)
            return ((tile.Color >> 4) & 0x0F) + 8;
        return tile.Color & 0x0F;
    }

    private void DrawTile(Location location, AnsiChar ac) => 
        _playField.DrawTile(location.X - 1, location.Y - 1, ac);

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

    private void InitializeElements(bool showInvisibleTiles)
    {
        _elements.Reset();
        _elements.Invisible().Character = showInvisibleTiles ? 0xB0 : 0x20;
        _elements.Invisible().Color = 0xFF;
        _elements.Player().Character = 0x02;
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
                    var actorData = _actors[_state.ActIndex];
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
                    var playerElement = _elements.Player();
                    DrawTile(_actors.Player.Location, new AnsiChar(playerElement.Character, playerElement.Color));
                }
                else
                {
                    if (_tiles[_actors.Player.Location].Id == _elements.PlayerId)
                        DrawTile(_actors.Player.Location, new AnsiChar(0x20, 0x0F));
                    else
                        _boardUpdater.UpdateBoard(_actors.Player.Location);
                }

                _hud.DrawPausing();
                _inputReader.Read(false);
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
                    var target = _actors.Player.Location + _state.KeyVector;
                    _interactions.Get(_tiles.ElementAt(target).Id)?.Interact(target, 0, ref _state.KeyVector);
                }

                if (!_state.KeyVector.IsZero())
                {
                    var target = _actors.Player.Location + _state.KeyVector;
                    if (_tiles.ElementAt(target).IsFloor)
                    {
                        if (_tiles.ElementAt(_actors.Player.Location).Id == _elements.PlayerId)
                        {
                            _mover.MoveActor(0, target);
                        }
                        else
                        {
                            _boardUpdater.UpdateBoard(_actors.Player.Location);
                            _actors.Player.Location += _state.KeyVector;
                            _playerUpdater.CleanUpPauseMovement();
                            _tiles[_actors.Player.Location] = new Tile(_elements.PlayerId, _elements.Player().Color);
                            _boardUpdater.UpdateBoard(_actors.Player.Location);
                            _radiusUpdater.UpdateRadius(_actors.Player.Location, RadiusMode.Update);
                            _radiusUpdater.UpdateRadius(_actors.Player.Location - _state.KeyVector, RadiusMode.Update);
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
                        _scheduler.WaitForTick();
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
                        _inputReader.Read(false);
                    }

                _tracer.TraceStep();
                if (_step)
                    break;

                _scheduler.WaitForTick();
            }

            if (_state.BreakGameLoop)
            {
                _soundUnit.ClearSound();
                if (_state.PlayerElement == _elements.PlayerId)
                {
                    // This game speed reset isn't here in the original code,
                    // but it solves some issues with game speed when returning
                    // to the title screen.

                    ResetGameSpeed();

                    if (_world.Health <= 0)
                        EnterHighScore(_world.Score);
                }
                else if (_state.PlayerElement == _elements.MonitorId)
                {
                    _hud.ClearTitleStatus();
                }

                var element = _elements.Player();
                _tiles[_actors.Player.Location] = new Tile(element.Id, element.Color);
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
                _dialogs.ShowAbout();

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

        var element = _elements[_state.PlayerElement];
        _tiles[_actors.Player.Location] = new Tile(element.Id, element.Color);
        if (_state.PlayerElement == _elements.MonitorId)
        {
            _messenger.SetMessage(0, new Message());
            _hud.DrawTitleStatus();
        }

        if (doFade)
            _fader.FadePurple();

        ResetGameSpeed();
        _state.GameCycle = _randomizer.GetNext(_facts.MainLoopRandomCycleRange);
        _state.ActIndex = _state.ActorCount + 1;
    }

    private void ResetGameSpeed() =>
        _state.GameWaitTime = _state.GameSpeed << 1;

    private void StartPlaying()
    {
        _worldUnit.SetBoard(_state.StartBoard);
        _playerEnterHandler.EnterBoard();
        _state.PlayerElement = _elements.PlayerId;
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
        var actor = _actors[index];
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
        _clock.OnTick += _scheduler.Advance;
        StartInit();
        TitleScreenLoop();
        _clock.OnTick -= _scheduler.Advance;
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
                _state.PlayerElement = _elements.MonitorId;
                _state.GamePaused = false;
                MainLoop(gameEnded);
                gameEnded = false;

                if (!ThreadActive)
                    break;

                var startPlaying = _playerInputHandler.HandleTitleInput();
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
            _scheduler.WaitForTick();
    }

    public int ResetBoardTimeHsec() =>
        _boardTime.Elapse();

    public void Dispose() =>
        _clock.Stop();
}