using System;
using System.Threading;
using Roton.Core;
using Roton.Emulation.Behavior;
using Roton.Emulation.Mapping;
using Roton.Emulation.SuperZZT;
using Roton.Events;
using Roton.Extensions;
using Roton.FileIo;

namespace Roton.Emulation.Execution
{
    public sealed class Engine : IEngine
    {
        public event EventHandler Terminated;
        public event DataEventHandler RequestReplaceContext;

        private int _timerTick;
        private readonly IKeyboard _keyboard;
        private readonly IFileSystem _fileSystem;
        private readonly IState _state;
        private readonly IOopContextFactory _oopContextFactory;
        private readonly IActors _actors;
        private readonly ITiles _tiles;
        private readonly IMisc _misc;
        private readonly IRandom _random;
        private readonly IBoard _board;
        private readonly IWorld _world;
        private readonly ITimers _timers;
        private readonly IElements _elements;
        private readonly IGameSerializer _gameSerializer;
        private readonly IAlerts _alerts;
        private readonly IHud _hud;
        private readonly IInterpreter _interpreter;
        private readonly IParser _parser;
        private readonly ISounder _sounder;
        private readonly IPlotter _plotter;
        private readonly IDrawer _drawer;
        private readonly IMessager _messager;
        private readonly IMover _mover;
        private readonly IRadius _radius;
        private readonly IBoards _boards;

        public Engine(
            IKeyboard keyboard,
            IBoards boards,
            IFileSystem fileSystem,
            IState state,
            IOopContextFactory oopContextFactory,
            IActors actors,
            IRandom random,
            IBoard board,
            IWorld world,
            ITimers timers,
            IElements elements,
            IGameSerializer gameSerializer,
            IAlerts alerts,
            IHud hud,
            IInterpreter interpreter,
            IParser parser,
            ISounder sounder,
            IPlotter plotter,
            IDrawer drawer,
            IMessager messager,
            IMover mover,
            IRadius radius,
            ITiles tiles,
            IMisc misc)
        {
            _keyboard = keyboard;
            _fileSystem = fileSystem;
            _state = state;
            _oopContextFactory = oopContextFactory;
            _actors = actors;
            _tiles = tiles;
            _misc = misc;
            _random = random;
            _board = board;
            _world = world;
            _timers = timers;
            _elements = elements;
            _gameSerializer = gameSerializer;
            _alerts = alerts;
            _hud = hud;
            _interpreter = interpreter;
            _parser = parser;
            _sounder = sounder;
            _plotter = plotter;
            _drawer = drawer;
            _messager = messager;
            _mover = mover;
            _radius = radius;
            _boards = boards;
        }

        private int TimerBase => _timers.Player.Ticks & 0x7FFF;

        private Thread Thread { get; set; }

        private bool ThreadActive { get; set; }

        private int TimerTick
        {
            get { return _timerTick; }
            set { _timerTick = value & 0x7FFF; }
        }

        public void ClearWorld()
        {
            _state.BoardCount = 0;
            _boards.Clear();
            _alerts.Reset();
            ClearBoard();
            _boards.Add(new PackedBoard(_gameSerializer.PackBoard(_tiles)));
            _world.BoardIndex = 0;
            _world.Ammo = 0;
            _world.Gems = 0;
            _world.Health = 100;
            _world.EnergyCycles = 0;
            _world.Torches = 0;
            _world.TorchCycles = 0;
            _world.Score = 0;
            _world.TimePassed = 0;
            _world.Stones = -1;
            _world.Keys.Clear();
            _world.Flags.Clear();
            SetBoard(0);
            _board.Name = "Introduction screen";
            _world.Name = string.Empty;
            _state.WorldFileName = string.Empty;
        }

        public void EnterBoard()
        {
            _board.Entrance.CopyFrom(_actors.Player.Location);
            if (_board.IsDark && _alerts.Dark)
            {
                _messager.SetMessage(0xC8, _alerts.DarkMessage);
                _alerts.Dark = false;
            }

            _world.TimePassed = 0;
            _hud.UpdateStatus();
        }

        public void ExecuteCode(int index, IExecutable instructionSource, string name)
        {
            var context = _oopContextFactory.Create(index, instructionSource, name);

            context.PreviousInstruction = context.Instruction;
            context.Moved = false;
            context.Repeat = false;
            context.Died = false;
            context.Finished = false;
            context.CommandsExecuted = 0;

            while (true)
            {
                if (context.Instruction < 0)
                    break;

                context.NextLine = true;
                context.PreviousInstruction = context.Instruction;

                var command = _parser.ReadByte(index, context);
                switch (command)
                {
                    case 0x3A: // :
                    case 0x27: // '
                    case 0x40: // @
                        _parser.ReadLine(index, context);
                        break;
                    case 0x2F: // /
                    case 0x3F: // ?
                        if (command == 0x2F)
                            context.Repeat = true;

                        var vector = _parser.GetDirection(context);
                        if (vector == null)
                        {
                            _messager.RaiseError("Bad direction");
                        }
                        else
                        {
                            ExecuteDirection(context, vector);
                            _parser.ReadByte(index, context);
                            if (_state.OopByte != 0x0D)
                                context.Instruction--;
                            context.Moved = true;
                        }

                        break;
                    case 0x23: // #
                        _interpreter.Execute(context);
                        break;
                    case 0x0D: // enter
                        if (context.Message.Count > 0)
                            context.Message.Add(string.Empty);
                        break;
                    case 0x00:
                        context.Finished = true;
                        break;
                    default:
                        context.Message.Add(command.ToStringValue() + _parser.ReadLine(context.Index, context));
                        break;
                }

                if (context.Finished ||
                    context.Moved ||
                    context.Repeat ||
                    context.Died ||
                    context.CommandsExecuted > 32)
                    break;
            }

            if (context.Repeat)
                context.Instruction = context.PreviousInstruction;

            if (_state.OopByte == 0)
                context.Instruction = -1;

            if (context.Message.Count > 0)
                ExecuteMessage(context);

            if (context.Died)
                ExecuteDeath(context);
        }

        public void FadePurple()
        {
            _hud.FadeBoard(new AnsiChar(0xDB, 0x05));
            _hud.RedrawBoard();
        }

        public bool GetPlayerTimeElapsed(int interval)
        {
            var result = false;
            while (GetTimeDifference(TimerTick, _state.PlayerTime) > 0)
            {
                result = true;
                _state.PlayerTime = (_state.PlayerTime + interval) & 0x7FFF;
            }

            return result;
        }

        public void PackBoard()
        {
            var board = new PackedBoard(_gameSerializer.PackBoard(_tiles));
            _boards[_world.BoardIndex] = board;
        }

        public int ReadKey()
        {
            var key = _keyboard.GetKey();
            _state.KeyPressed = key > 0 ? key : 0;
            return _state.KeyPressed;
        }

        public void SetBoard(int boardIndex)
        {
            var element = _elements[_elements.PlayerId];
            _tiles[_actors.Player.Location].SetTo(element.Id, element.Color);
            PackBoard();
            UnpackBoard(boardIndex);
        }

        public void ShowInGameHelp()
        {
            ShowHelp("GAME");
        }

        public void Start()
        {
            if (!ThreadActive)
            {
                ThreadActive = true;
                Thread = new Thread(StartMain);
                TimerTick = _timers.Player.Ticks;
                Thread.Start();
            }
        }

        public void Stop()
        {
            if (ThreadActive)
            {
                ThreadActive = false;
            }
        }

        public bool TitleScreen => _state.PlayerElement != _elements.PlayerId;

        public void UnpackBoard(int boardIndex)
        {
            _gameSerializer.UnpackBoard(_tiles, _boards[boardIndex].Data);
            _world.BoardIndex = boardIndex;
        }

        public void WaitForTick()
        {
            while (TimerTick == TimerBase && ThreadActive)
            {
                Thread.Sleep(1);
            }

            TimerTick++;
        }

        private void ClearBoard()
        {
            var emptyId = _elements.EmptyId;
            var boardEdgeId = _state.EdgeTile.Id;
            var boardBorderId = _state.BorderTile.Id;
            var boardBorderColor = _state.BorderTile.Color;

            // board properties
            _board.Name = string.Empty;
            _state.Message = string.Empty;
            _board.MaximumShots = 0xFF;
            _board.IsDark = false;
            _board.RestartOnZap = false;
            _board.TimeLimit = 0;
            _board.ExitEast = 0;
            _board.ExitNorth = 0;
            _board.ExitSouth = 0;
            _board.ExitWest = 0;

            // build board edges
            for (var y = 0; y <= _tiles.Height + 1; y++)
            {
                _tiles[new Location(0, y)].Id = boardEdgeId;
                _tiles[new Location(_tiles.Width + 1, y)].Id = boardEdgeId;
            }

            for (var x = 0; x <= _tiles.Width + 1; x++)
            {
                _tiles[new Location(x, 0)].Id = boardEdgeId;
                _tiles[new Location(x, _tiles.Height + 1)].Id = boardEdgeId;
            }

            // clear out board
            for (var x = 1; x <= _tiles.Width; x++)
            {
                for (var y = 1; y <= _tiles.Height; y++)
                {
                    _tiles[new Location(x, y)].SetTo(emptyId, 0);
                }
            }

            // build border
            for (var y = 1; y <= _tiles.Height; y++)
            {
                _tiles[new Location(1, y)].SetTo(boardBorderId, boardBorderColor);
                _tiles[new Location(_tiles.Width, y)].SetTo(boardBorderId, boardBorderColor);
            }

            for (var x = 1; x <= _tiles.Width; x++)
            {
                _tiles[new Location(x, 1)].SetTo(boardBorderId, boardBorderColor);
                _tiles[new Location(x, _tiles.Height)].SetTo(boardBorderId, boardBorderColor);
            }

            // generate player actor
            var element = _elements[_elements.PlayerId];
            _state.ActorCount = 0;
            _actors.Player.Location.SetTo(_tiles.Width / 2, _tiles.Height / 2);
            _tiles[_actors.Player.Location].SetTo(element.Id, element.Color);
            _actors.Player.Cycle = 1;
            _actors.Player.UnderTile.SetTo(0, 0);
            _actors.Player.Pointer = 0;
            _actors.Player.Length = 0;
        }

        private void DrawTile(IXyPair location, AnsiChar ac)
        {
            _hud.DrawTile(location.X - 1, location.Y - 1, ac);
        }

        private void EnterHighScore(int score)
        {
        }

        protected void ExecuteDeath(IOopContext context)
        {
            var location = context.Actor.Location.Clone();
            _mover.Harm(context.Index);
            _plotter.PlotTile(location, context.DeathTile);
        }

        protected void ExecuteDirection(IOopContext context, IXyPair vector)
        {
            if (vector.IsZero())
            {
                context.Repeat = false;
            }
            else
            {
                var target = context.Actor.Location.Sum(vector);
                if (!_tiles.ElementAt(target).IsFloor)
                {
                    _mover.Push(target, vector);
                }

                if (_tiles.ElementAt(target).IsFloor)
                {
                    _mover.MoveActor(context.Index, target);
                    context.Repeat = false;
                }
            }
        }

        protected void ExecuteMessage(IOopContext context)
        {
            if (context.Message.Count == 1)
            {
                _messager.SetMessage(0xC8, new Message(context.Message));
            }
            else
            {
                _state.KeyVector.SetTo(0, 0);
                _hud.ShowScroll(context.Message);
            }
        }

        public void FadeRed()
        {
            _hud.FadeBoard(new AnsiChar(0xDB, 0x04));
            _hud.RedrawBoard();
        }

        private int GetTimeDifference(int now, int then)
        {
            now &= 0x7FFF;
            then &= 0x7FFF;
            if (now < 0x4000 && then >= 0x4000)
            {
                now += 0x8000;
            }

            return now - then;
        }

        protected void InitializeElements(bool showInvisibles)
        {
            // this isn't all the initializations.
            // todo: replace this with the ability to completely reinitialize engine default memory
            _elements[_elements.InvisibleId].Character = showInvisibles ? 0xB0 : 0x20;
            _elements[_elements.InvisibleId].Color = 0xFF;
            _elements[_elements.PlayerId].Character = 0x02;
        }

        protected byte[] LoadFile(string filename)
        {
            try
            {
                return _fileSystem.GetFile(filename);
            }
            catch (Exception)
            {
                // TODO: This kind of error handling is bad.
                return null;
            }
        }

        private void MainLoop(bool gameIsActive)
        {
            var alternating = false;

            _hud.CreateStatusText();
            _hud.UpdateStatus();

            if (_state.Init)
            {
                if (!_state.AboutShown)
                {
                    ShowAbout();
                }

                if (_state.DefaultWorldName.Length <= 0)
                {
                    // normally we would load the world here,
                    // however it will have already been loaded in the context
                }

                _state.StartBoard = _world.BoardIndex;
                SetBoard(0);
                _state.Init = false;
            }

            var element = _elements[_state.PlayerElement];
            _tiles[_actors.Player.Location].SetTo(element.Id, element.Color);
            if (_state.PlayerElement == _elements.MonitorId)
            {
                _messager.SetMessage(0, new Message());
                _hud.DrawTitleStatus();
            }

            if (gameIsActive)
            {
                FadePurple();
            }

            _state.GameWaitTime = _state.GameSpeed << 1;
            _state.BreakGameLoop = false;
            _state.GameCycle = _random.Synced(0x64);
            _state.ActIndex = _state.ActorCount + 1;

            while (ThreadActive)
            {
                if (!_state.GamePaused)
                {
                    if (_state.ActIndex <= _state.ActorCount)
                    {
                        var actorData = _actors[_state.ActIndex];
                        if (actorData.Cycle != 0)
                        {
                            if (_state.ActIndex % actorData.Cycle == _state.GameCycle % actorData.Cycle)
                            {
                                _elements[_tiles[actorData.Location].Id].Act(_state.ActIndex);
                            }
                        }

                        _state.ActIndex++;
                    }
                }
                else
                {
                    _state.ActIndex = _state.ActorCount + 1;
                    if (_timers.Player.Clock(25))
                    {
                        alternating = !alternating;
                    }

                    if (alternating)
                    {
                        var playerElement = _elements[_elements.PlayerId];
                        DrawTile(_actors.Player.Location, new AnsiChar(playerElement.Character, playerElement.Color));
                    }
                    else
                    {
                        if (_tiles[_actors.Player.Location].Id == _elements.PlayerId)
                        {
                            DrawTile(_actors.Player.Location, new AnsiChar(0x20, 0x0F));
                        }
                        else
                        {
                            _drawer.UpdateBoard(_actors.Player.Location);
                        }
                    }

                    _hud.DrawPausing();
                    ReadInput();
                    if (_state.KeyPressed == 0x1B)
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
                        var target = _actors.Player.Location.Sum(_state.KeyVector);
                        _tiles.ElementAt(target).Interact(target, 0, _state.KeyVector);
                    }

                    if (!_state.KeyVector.IsZero())
                    {
                        var target = _actors.Player.Location.Sum(_state.KeyVector);
                        if (_tiles.ElementAt(target).IsFloor)
                        {
                            if (_tiles.ElementAt(_actors.Player.Location).Id == _elements.PlayerId)
                            {
                                _mover.MoveActor(0, target);
                            }
                            else
                            {
                                _drawer.UpdateBoard(_actors.Player.Location);
                                _actors.Player.Location.Add(_state.KeyVector);
                                _tiles[_actors.Player.Location]
                                    .SetTo(_elements.PlayerId, _elements[_elements.PlayerId].Color);
                                _drawer.UpdateBoard(_actors.Player.Location);
                                _radius.Update(_actors.Player.Location, RadiusMode.Update);
                                _radius.Update(_actors.Player.Location.Difference(_state.KeyVector), RadiusMode.Update);
                            }

                            _state.GamePaused = false;
                            _hud.ClearPausing();
                            _state.GameCycle = _random.Synced(100);
                            _world.IsLocked = true;
                        }
                    }
                }

                ExecuteOnce();

                if (_state.BreakGameLoop)
                {
                    _sounder.Clear();
                    if (_state.PlayerElement == _elements.PlayerId)
                    {
                        if (_world.Health <= 0)
                        {
                            EnterHighScore(_world.Score);
                        }
                    }
                    else if (_state.PlayerElement == _elements.MonitorId)
                    {
                        _hud.ClearTitleStatus();
                    }

                    element = _elements[_elements.PlayerId];
                    _tiles[_actors.Player.Location].SetTo(element.Id, element.Color);
                    _state.GameOver = false;
                    break;
                }
            }
        }

        private void ExecuteOnce()
        {
            if (_state.ActIndex > _state.ActorCount)
            {
                if (!_state.BreakGameLoop && !_state.GamePaused)
                {
                    if (_state.GameWaitTime <= 0 || _timers.Player.Clock(_state.GameWaitTime))
                    {
                        _state.GameCycle++;
                        if (_state.GameCycle > 420)
                        {
                            _state.GameCycle = 1;
                        }

                        _state.ActIndex = 0;
                        ReadInput();
                    }
                }

                WaitForTick();
            }
        }

        public bool PlayWorld()
        {
            var gameIsActive = false;

            if (_world.IsLocked)
            {
                var file = LoadFile(_misc.GetWorldName(string.IsNullOrWhiteSpace(_world.Name)
                    ? _state.WorldFileName
                    : _world.Name));
                if (file != null)
                {
                    RequestReplaceContext?.Invoke(this, new DataEventArgs {Data = file});
                    gameIsActive = _state.WorldLoaded;
                    _state.StartBoard = _world.BoardIndex;
                }
            }
            else
            {
                gameIsActive = true;
            }

            if (gameIsActive)
            {
                SetBoard(_state.StartBoard);
                EnterBoard();
                _state.PlayerElement = _elements.PlayerId;
                _state.GamePaused = true;
                MainLoop(true);
            }

            return gameIsActive;
        }

        protected void ReadInput()
        {
            _state.KeyShift = false;
            _state.KeyArrow = false;
            _state.KeyPressed = 0;
            _state.KeyVector.SetTo(0, 0);

            var key = _keyboard.GetKey();
            if (key >= 0)
            {
                _state.KeyPressed = key;
                _state.KeyShift = _keyboard.Shift;
                switch (key)
                {
                    case 0xCB:
                        _state.KeyVector.CopyFrom(Vector.West);
                        _state.KeyArrow = true;
                        break;
                    case 0xCD:
                        _state.KeyVector.CopyFrom(Vector.East);
                        _state.KeyArrow = true;
                        break;
                    case 0xC8:
                        _state.KeyVector.CopyFrom(Vector.North);
                        _state.KeyArrow = true;
                        break;
                    case 0xD0:
                        _state.KeyVector.CopyFrom(Vector.South);
                        _state.KeyArrow = true;
                        break;
                }
            }
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

        private void ShowAbout()
        {
            ShowHelp("ABOUT");
        }

        private void ShowHelp(string filename)
        {
        }

        protected void StartInit()
        {
            _state.GameSpeed = 4;
            _state.DefaultSaveName = "SAVED";
            _state.DefaultBoardName = "TEMP";
            _state.DefaultWorldName = "TOWN";
            if (!_state.WorldLoaded)
            {
                ClearWorld();
            }

            if (_state.EditorMode)
                SetEditorMode();
            else
                SetGameMode();
        }

        protected void StartMain()
        {
            StartInit();
            TitleScreenLoop();
            Terminated?.Invoke(this, EventArgs.Empty);
        }

        protected void TitleScreenLoop()
        {
            _state.QuitZzt = false;
            _state.Init = true;
            _state.StartBoard = 0;
            var gameEnded = true;
            _hud.Initialize();
            while (ThreadActive)
            {
                if (!_state.Init)
                {
                    SetBoard(0);
                }

                while (ThreadActive)
                {
                    _state.PlayerElement = _elements.MonitorId;
                    _state.GamePaused = false;
                    MainLoop(gameEnded);
                    if (!ThreadActive)
                    {
                        // escape if the thread is supposed to shut down
                        break;
                    }

                    var hotkey = _state.KeyPressed.ToUpperCase();
                    var startPlaying = _misc.HandleTitleInput(hotkey);
                    if (startPlaying)
                    {
                        gameEnded = PlayWorld();
                    }

                    if (gameEnded || _state.QuitZzt)
                    {
                        break;
                    }
                }

                if (_state.QuitZzt)
                {
                    break;
                }
            }
        }
    }
}