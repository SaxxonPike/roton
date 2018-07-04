using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Roton.Core;
using Roton.Emulation.Behavior;
using Roton.Emulation.Mapping;
using Roton.Emulation.Timing;
using Roton.Events;
using Roton.Extensions;
using Roton.FileIo;

namespace Roton.Emulation.Execution
{
    public abstract class Engine : IEngine
    {
        public event EventHandler Terminated;
        public event DataEventHandler RequestReplaceContext;

        private int _timerTick;
        private readonly IKeyboard _keyboard;
        private readonly IFileSystem _fileSystem;
        private readonly IState _state;
        private readonly IOopContextFactory _oopContextFactory;
        private readonly IActors _actors;
        private readonly IGrid _grid;
        private readonly IRandom _random;
        private readonly IBoard _board;
        private readonly IWorld _world;
        private readonly ITimers _timers;
        private readonly IElements _elements;
        private readonly ISounds _sounds;
        private readonly IGameSerializer _gameSerializer;
        private readonly IAlerts _alerts;
        private readonly IHud _hud;
        private readonly IGrammar _grammar;
        private readonly IBoards _boards;

        protected Engine(
            IKeyboard keyboard,
            IBoards boards,
            IFileSystem fileSystem,
            IState state,
            IOopContextFactory oopContextFactory,
            IActors actors,
            IGrid grid,
            IRandom random,
            IBoard board,
            IWorld world,
            ITimers timers,
            IElements elements,
            ISounds sounds,
            IGameSerializer gameSerializer,
            IAlerts alerts,
            IHud hud,
            IGrammar grammar)
        {
            _keyboard = keyboard;
            _fileSystem = fileSystem;
            _state = state;
            _oopContextFactory = oopContextFactory;
            _actors = actors;
            _grid = grid;
            _random = random;
            _board = board;
            _world = world;
            _timers = timers;
            _elements = elements;
            _sounds = sounds;
            _gameSerializer = gameSerializer;
            _alerts = alerts;
            _hud = hud;
            _grammar = grammar;
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

        public void Attack(int index, IXyPair location)
        {
            if (index == 0 && _world.EnergyCycles > 0)
            {
                _world.Score += _grid.ElementAt(location).Points;
                UpdateStatus();
            }
            else
            {
                Harm(index);
            }

            if (index > 0 && index <= _state.ActIndex)
            {
                _state.ActIndex--;
            }

            if (_grid[location].Id == _elements.PlayerId && _world.EnergyCycles > 0)
            {
                _world.Score += _grid.ElementAt(_actors[index].Location).Points;
                UpdateStatus();
            }
            else
            {
                Destroy(location);
                this.PlaySound(2, _sounds.EnemySuicide);
            }
        }

        public virtual bool BroadcastLabel(int sender, string label, bool force)
        {
            var external = false;
            var success = false;

            if (sender < 0)
            {
                external = true;
                sender = -sender;
            }

            var info = new SearchContext(this)
            {
                SearchIndex = 0,
                SearchOffset = 0,
                SearchTarget = label
            };

            while (ExecuteLabel(sender, info, "\x000D:"))
            {
                if (!ActorIsLocked(info.SearchIndex) || force || (sender == info.SearchIndex && !external))
                {
                    if (sender == info.SearchIndex)
                    {
                        success = true;
                    }

                    _actors[info.SearchIndex].Instruction = info.SearchOffset;
                }

                info.SearchTarget = label;
            }

            return success;
        }

        public void ClearSound()
        {
            _state.SoundPlaying = false;
            StopSound();
        }

        public void ClearWorld()
        {
            _state.BoardCount = 0;
            _boards.Clear();
            ResetAlerts();
            ClearBoard();
            _boards.Add(new PackedBoard(_gameSerializer.PackBoard(_grid)));
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

        public void Convey(IXyPair center, int direction)
        {
            int beginIndex;
            int endIndex;

            var surrounding = new ITile[8];

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
                surrounding[i] = _grid.TileAt(center.Sum(GetConveyorVector(i))).Clone();
                var element = _elements[surrounding[i].Id];
                if (element.Id == _elements.EmptyId)
                    pushable = true;
                else if (!element.IsPushable)
                    pushable = false;
            }

            for (var i = beginIndex; i != endIndex; i += direction)
            {
                var element = _elements[surrounding[i].Id];

                if (pushable)
                {
                    if (element.IsPushable)
                    {
                        var source = center.Sum(GetConveyorVector(i));
                        var target = center.Sum(GetConveyorVector((i + 8 - direction) % 8));
                        if (element.Cycle > -1)
                        {
                            var tile = _grid.TileAt(source);
                            var index = _actors.ActorIndexAt(source);
                            _grid.TileAt(source).CopyFrom(surrounding[i]);
                            _grid.TileAt(target).Id = _elements.EmptyId;
                            MoveActor(index, target);
                            _grid.TileAt(source).CopyFrom(tile);
                        }
                        else
                        {
                            _grid.TileAt(target).CopyFrom(surrounding[i]);
                            UpdateBoard(target);
                        }

                        if (!_elements[surrounding[(i + 8 + direction) % 8].Id].IsPushable)
                        {
                            _grid.TileAt(source).Id = _elements.EmptyId;
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
                    if (element.Id == _elements.EmptyId)
                        pushable = true;
                }
            }
        }

        public void Destroy(IXyPair location)
        {
            var index = _actors.ActorIndexAt(location);
            if (index == -1)
            {
                RemoveItem(location);
            }
            else
            {
                Harm(index);
            }
        }

        public virtual AnsiChar Draw(IXyPair location)
        {
            if (_board.IsDark && !_grid.ElementAt(location).IsAlwaysVisible &&
                (_world.TorchCycles <= 0 || Distance(_actors.Player.Location, location) >= 50) && !_state.EditorMode)
            {
                return new AnsiChar(0xB0, 0x07);
            }

            var tile = _grid[location];
            var element = _elements[tile.Id];
            var elementCount = _elements.Count;

            if (tile.Id == _elements.EmptyId)
            {
                return new AnsiChar(0x20, 0x0F);
            }

            if (element.HasDrawCode)
            {
                return element.Draw(location);
            }

            if (tile.Id < elementCount - 7)
            {
                return new AnsiChar(element.Character, tile.Color);
            }

            if (tile.Id != elementCount - 1)
            {
                return new AnsiChar(tile.Color, ((tile.Id - (elementCount - 8)) << 4) | 0x0F);
            }

            return new AnsiChar(tile.Color, 0x0F);
        }

        public ISound EncodeMusic(string music)
        {
            return new Sound();
        }

        public virtual void EnterBoard()
        {
            _board.Entrance.CopyFrom(_actors.Player.Location);
            if (_board.IsDark && _alerts.Dark)
            {
                SetMessage(0xC8, _alerts.DarkMessage);
                _alerts.Dark = false;
            }

            _world.TimePassed = 0;
            UpdateStatus();
        }

        public virtual void ExecuteCode(int index, IExecutable instructionSource, string name)
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

                var command = ReadActorCodeByte(index, context);
                switch (command)
                {
                    case 0x3A: // :
                    case 0x27: // '
                    case 0x40: // @
                        ReadActorCodeLine(index, context);
                        break;
                    case 0x2F: // /
                    case 0x3F: // ?
                        if (command == 0x2F)
                            context.Repeat = true;

                        var vector = _grammar.GetDirection(context);
                        if (vector == null)
                        {
                            RaiseError("Bad direction");
                        }
                        else
                        {
                            ExecuteDirection(context, vector);
                            ReadActorCodeByte(index, context);
                            if (_state.OopByte != 0x0D)
                                context.Instruction--;
                            context.Moved = true;
                        }

                        break;
                    case 0x23: // #
                        _grammar.Execute(context);
                        break;
                    case 0x0D: // enter
                        if (context.Message.Count > 0)
                            context.Message.Add(string.Empty);
                        break;
                    case 0x00:
                        context.Finished = true;
                        break;
                    default:
                        context.Message.Add(command.ToStringValue() + ReadActorCodeLine(context.Index, context));
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

        public bool ExecuteLabel(int sender, ISearchContext context, string prefix)
        {
            var label = context.SearchTarget;
            var success = false;
            var split = label.IndexOf(':');

            if (split > 0)
            {
                var target = label.Substring(0, split);
                label = label.Substring(split + 1);
                context.SearchTarget = target;
                success = _grammar.GetTarget(context);
            }
            else if (context.SearchIndex < sender)
            {
                context.SearchIndex = sender;
                split = 0;
                success = true;
            }

            while (true)
            {
                if (!success)
                {
                    break;
                }

                if (label.ToUpper() == @"RESTART")
                {
                    context.SearchOffset = 0;
                }
                else
                {
                    context.SearchOffset = SearchActorCode(context.SearchIndex, prefix + label);
                    if (context.SearchOffset < 0 && split > 0)
                    {
                        success = _grammar.GetTarget(context);
                        continue;
                    }
                }

                success = context.SearchOffset >= 0;
                break;
            }

            return success;
        }

        public void FadePurple()
        {
            FadeBoard(new AnsiChar(0xDB, 0x05));
            RedrawBoard();
        }

        public bool FindTile(ITile kind, IXyPair location)
        {
            location.X++;
            while (location.Y <= _grid.Height)
            {
                while (location.X <= _grid.Width)
                {
                    var tile = _grid.TileAt(location);
                    if (tile.Id == kind.Id)
                    {
                        if (kind.Color == 0 || ColorMatch(_grid.TileAt(location)) == kind.Color)
                        {
                            return true;
                        }
                    }

                    location.X++;
                }

                location.X = 1;
                location.Y++;
            }

            return false;
        }

        public virtual void ForcePlayerColor(int index)
        {
            var actor = _actors[index];
            var playerElement = _elements[_elements.PlayerId];
            if (_grid.TileAt(actor.Location).Color != playerElement.Color ||
                playerElement.Character != 0x02)
            {
                playerElement.Character = 2;
                _grid.TileAt(actor.Location).Color = playerElement.Color;
                UpdateBoard(actor.Location);
            }
        }

        public IXyPair GetCardinalVector(int index)
        {
            return new Vector(_state.Vector4[index], _state.Vector4[index + 4]);
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

        public abstract void HandlePlayerInput(IActor actor, int hotkey);

        public void Harm(int index)
        {
            var actor = _actors[index];
            if (index == 0)
            {
                if (_world.Health > 0)
                {
                    _world.Health -= 10;
                    UpdateStatus();
                    SetMessage(0x64, _alerts.OuchMessage);
                    var color = _grid.TileAt(actor.Location).Color;
                    color &= 0x0F;
                    color |= 0x70;
                    _grid.TileAt(actor.Location).Color = color;
                    if (_world.Health > 0)
                    {
                        _world.TimePassed = 0;
                        if (_board.RestartOnZap)
                        {
                            this.PlaySound(4, _sounds.TimeOut);
                            _grid.TileAt(actor.Location).Id = _elements.EmptyId;
                            UpdateBoard(actor.Location);
                            var oldLocation = actor.Location.Clone();
                            actor.Location.CopyFrom(_board.Entrance);
                            UpdateRadius(oldLocation, 0);
                            UpdateRadius(actor.Location, 0);
                            _state.GamePaused = true;
                        }

                        this.PlaySound(4, _sounds.Ouch);
                    }
                    else
                    {
                        this.PlaySound(5, _sounds.GameOver);
                    }
                }
            }
            else
            {
                var element = _grid.TileAt(actor.Location).Id;
                if (element == _elements.BulletId)
                {
                    this.PlaySound(3, _sounds.BulletDie);
                }
                else if (element != _elements.ObjectId)
                {
                    this.PlaySound(3, _sounds.EnemyDie);
                }

                RemoveActor(index);
            }
        }

        public void MoveActor(int index, IXyPair target)
        {
            var actor = _actors[index];
            var sourceLocation = actor.Location.Clone();
            var sourceTile = _grid.TileAt(actor.Location);
            var targetTile = _grid.TileAt(target);
            var underTile = actor.UnderTile.Clone();

            actor.UnderTile.CopyFrom(targetTile);
            if (targetTile.Id == _elements.EmptyId)
            {
                targetTile.SetTo(sourceTile.Id, sourceTile.Color & 0x0F);
            }
            else
            {
                targetTile.SetTo(sourceTile.Id, (targetTile.Color & 0x70) | (sourceTile.Color & 0x0F));
            }

            sourceTile.CopyFrom(underTile);
            actor.Location.CopyFrom(target);
            if (targetTile.Id == _elements.PlayerId)
            {
                ForcePlayerColor(index);
            }

            UpdateBoard(target);
            UpdateBoard(sourceLocation);
            if (index == 0 && _board.IsDark)
            {
                var squareDistanceX = (target.X - sourceLocation.X).Square();
                var squareDistanceY = (target.Y - sourceLocation.Y).Square();
                if (squareDistanceX + squareDistanceY == 1)
                {
                    var glowLocation = new Location();
                    for (var x = target.X - 11; x <= target.X + 11; x++)
                    {
                        for (var y = target.Y - 8; y <= target.Y + 8; y++)
                        {
                            glowLocation.SetTo(x, y);
                            if (glowLocation.X >= 1 && glowLocation.X <= _grid.Width && glowLocation.Y >= 1 &&
                                glowLocation.Y <= _grid.Height)
                            {
                                if ((Distance(sourceLocation, glowLocation) < 50) ^
                                    (Distance(target, glowLocation) < 50))
                                {
                                    UpdateBoard(glowLocation);
                                }
                            }
                        }
                    }
                }
            }

            if (index == 0)
            {
                _hud.UpdateCamera();
            }
        }

        public void MoveActorOnRiver(int index)
        {
            var actor = _actors[index];
            var vector = new Vector();
            var underId = actor.UnderTile.Id;

            if (underId == _elements.RiverNId)
            {
                vector.SetTo(0, -1);
            }
            else if (underId == _elements.RiverSId)
            {
                vector.SetTo(0, 1);
            }
            else if (underId == _elements.RiverWId)
            {
                vector.SetTo(-1, 0);
            }
            else if (underId == _elements.RiverEId)
            {
                vector.SetTo(1, 0);
            }

            if (_grid.ElementAt(actor.Location).Id == _elements.PlayerId)
            {
                _grid.ElementAt(actor.Location.Sum(vector)).Interact(actor.Location.Sum(vector), 0, vector);
            }

            if (vector.IsNonZero())
            {
                var target = actor.Location.Sum(vector);
                if (_grid.ElementAt(target).IsFloor)
                {
                    MoveActor(index, target);
                }
            }
        }

        public void PackBoard()
        {
            var board = new PackedBoard(_gameSerializer.PackBoard(_grid));
            _boards[_world.BoardIndex] = board;
        }

        public void PlaySound(int priority, ISound sound, int offset, int length)
        {
        }

        public void PlotTile(IXyPair location, ITile tile)
        {
            if (_grid.ElementAt(location).Id == _elements.PlayerId)
                return;

            var targetElement = _elements[tile.Id];
            var existingTile = _grid[location];
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
                {
                    existingTile.SetTo(targetElement.Id, targetColor);
                }
                else
                {
                    SpawnActor(location, new Tile(targetElement.Id, targetColor), targetElement.Cycle,
                        _state.DefaultActor);
                }
            }

            UpdateBoard(location);
        }

        public void Push(IXyPair location, IXyPair vector)
        {
            // this is here to prevent endless push loops
            // but doesn't exist in the original code
            if (vector.IsZero())
            {
                throw Exceptions.PushStackOverflow;
            }

            var tile = _grid.TileAt(location);
            if ((tile.Id == _elements.SliderEwId && vector.Y == 0) ||
                (tile.Id == _elements.SliderNsId && vector.X == 0) ||
                _elements[tile.Id].IsPushable)
            {
                var furtherTile = _grid.TileAt(location.Sum(vector));
                if (furtherTile.Id == _elements.TransporterId)
                {
                    PushThroughTransporter(location, vector);
                }
                else if (furtherTile.Id != _elements.EmptyId)
                {
                    Push(location.Sum(vector), vector);
                }

                var furtherElement = _elements[furtherTile.Id];
                if (!furtherElement.IsFloor && furtherElement.IsDestructible && furtherTile.Id != _elements.PlayerId)
                {
                    Destroy(location.Sum(vector));
                }

                furtherElement = _elements[furtherTile.Id];
                if (furtherElement.IsFloor)
                {
                    MoveTile(location, location.Sum(vector));
                }
            }
        }

        public void PushThroughTransporter(IXyPair location, IXyPair vector)
        {
            var actor = _actors.ActorAt(location.Sum(vector));

            if (actor.Vector.Matches(vector))
            {
                var search = actor.Location.Clone();
                var target = new Location();
                var ended = false;
                var success = true;

                while (!ended)
                {
                    search.Add(vector);
                    var element = _grid.ElementAt(search);
                    if (element.Id == _elements.BoardEdgeId)
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
                                element = _grid.ElementAt(search);
                            }

                            if (element.IsFloor)
                            {
                                ended = true;
                                target.CopyFrom(search);
                            }
                            else
                            {
                                target.X = 0;
                            }
                        }
                    }

                    if (element.Id == _elements.TransporterId)
                    {
                        if (_actors.ActorAt(search).Vector.Matches(vector.Opposite()))
                        {
                            success = true;
                        }
                    }
                }

                if (target.X > 0)
                {
                    MoveTile(actor.Location.Difference(vector), target);
                    this.PlaySound(3, _sounds.Transporter);
                }
            }
        }

        public void RaiseError(string error)
        {
            SetMessage(0xC8, _alerts.ErrorMessage(error));
            this.PlaySound(5, _sounds.Error);
        }

        public int ReadActorCodeByte(int index, IExecutable instructionSource)
        {
            var actor = _actors[index];
            var value = 0;

            if (instructionSource.Instruction < 0 || instructionSource.Instruction >= actor.Length)
            {
                _state.OopByte = 0;
            }
            else
            {
                Debug.Assert(actor.Length == actor.Code.Length, @"Actor length and actual code length mismatch.");
                value = actor.Code[instructionSource.Instruction];
                _state.OopByte = value;
                instructionSource.Instruction++;
            }

            return value;
        }

        public string ReadActorCodeLine(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();
            ReadActorCodeByte(index, instructionSource);
            while (_state.OopByte != 0x00 && _state.OopByte != 0x0D)
            {
                result.Append(_state.OopByte.ToChar());
                ReadActorCodeByte(index, instructionSource);
            }

            return result.ToString();
        }

        public int ReadActorCodeNumber(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();
            var success = false;

            while (ReadActorCodeByte(index, instructionSource) == 0x20)
            {
            }

            _state.OopByte = _state.OopByte.ToUpperCase();
            while (_state.OopByte >= 0x30 && _state.OopByte <= 0x39)
            {
                success = true;
                result.Append(_state.OopByte.ToChar());
                ReadActorCodeByte(index, instructionSource);
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            if (!success)
            {
                _state.OopNumber = -1;
            }
            else
            {
                int resultInt;
                int.TryParse(result.ToString(), out resultInt);
                _state.OopNumber = resultInt;
            }

            return _state.OopNumber;
        }

        public string ReadActorCodeWord(int index, IExecutable instructionSource)
        {
            var result = new StringBuilder();

            while (true)
            {
                ReadActorCodeByte(index, instructionSource);
                if (_state.OopByte != 0x20)
                {
                    break;
                }
            }

            _state.OopByte = _state.OopByte.ToUpperCase();

            if (!(_state.OopByte >= 0x30 && _state.OopByte <= 0x39))
            {
                while ((_state.OopByte >= 0x41 && _state.OopByte <= 0x5A) ||
                       (_state.OopByte >= 0x30 && _state.OopByte <= 0x39) || (_state.OopByte == 0x3A) ||
                       (_state.OopByte == 0x5F))
                {
                    result.Append(_state.OopByte.ToChar());
                    ReadActorCodeByte(index, instructionSource);
                    _state.OopByte = _state.OopByte.ToUpperCase();
                }
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            _state.OopWord = result.ToString();
            return _state.OopWord;
        }

        public virtual int ReadKey()
        {
            var key = _keyboard.GetKey();
            _state.KeyPressed = key > 0 ? key : 0;
            return _state.KeyPressed;
        }

        public void RedrawBoard()
        {
            _hud.RedrawBoard();
        }

        public void RemoveActor(int index)
        {
            var actor = _actors[index];
            if (index < _state.ActIndex)
            {
                _state.ActIndex--;
            }

            _grid.TileAt(actor.Location).CopyFrom(actor.UnderTile);
            if (actor.Location.Y > 0)
            {
                UpdateBoard(actor.Location);
            }

            for (var i = 1; i <= _state.ActorCount; i++)
            {
                var a = _actors[i];
                if (a.Follower >= index)
                {
                    if (a.Follower == index)
                    {
                        a.Follower = -1;
                    }
                    else
                    {
                        a.Follower--;
                    }
                }

                if (a.Leader >= index)
                {
                    if (a.Leader == index)
                    {
                        a.Leader = -1;
                    }
                    else
                    {
                        a.Leader--;
                    }
                }
            }

            if (index < _state.ActorCount)
            {
                for (var i = index; i < _state.ActorCount; i++)
                {
                    _actors[i].CopyFrom(_actors[i + 1]);
                }
            }

            _state.ActorCount--;
        }

        public virtual void RemoveItem(IXyPair location)
        {
            _grid.TileAt(location).Id = _elements.EmptyId;
            UpdateBoard(location);
        }

        public IXyPair Rnd()
        {
            var result = new Vector();
            Rnd(result);
            return result;
        }

        public IXyPair RndP(IXyPair vector)
        {
            var result = new Vector();
            result.CopyFrom(
                _random.Synced(2) == 0
                    ? vector.Clockwise()
                    : vector.CounterClockwise());
            return result;
        }

        public virtual int SearchActorCode(int index, string term)
        {
            var result = -1;
            var termBytes = term.ToBytes();
            var actor = _actors[index];
            var offset = new Executable {Instruction = 0};

            while (offset.Instruction < actor.Length)
            {
                var oldOffset = offset.Instruction;
                var termOffset = 0;
                bool success;

                while (true)
                {
                    ReadActorCodeByte(index, offset);
                    if (termBytes[termOffset].ToUpperCase() != _state.OopByte.ToUpperCase())
                    {
                        success = false;
                        break;
                    }

                    termOffset++;
                    if (termOffset >= termBytes.Length)
                    {
                        success = true;
                        break;
                    }
                }

                if (success)
                {
                    ReadActorCodeByte(index, offset);
                    _state.OopByte = _state.OopByte.ToUpperCase();
                    if (!((_state.OopByte >= 0x41 && _state.OopByte <= 0x5A) || _state.OopByte == 0x5F))
                    {
                        result = oldOffset;
                        break;
                    }
                }

                oldOffset++;
                offset.Instruction = oldOffset;
            }

            return result;
        }

        public IXyPair Seek(IXyPair location)
        {
            var result = new Vector();
            if (_random.Synced(2) == 0 || _actors.Player.Location.Y == location.Y)
            {
                result.X = (_actors.Player.Location.X - location.X).Polarity();
            }

            if (result.X == 0)
            {
                result.Y = (_actors.Player.Location.Y - location.Y).Polarity();
            }

            if (_world.EnergyCycles > 0)
            {
                result.SetOpposite();
            }

            return result;
        }

        public void SetBoard(int boardIndex)
        {
            var element = _elements[_elements.PlayerId];
            _grid.TileAt(_actors.Player.Location).SetTo(element.Id, element.Color);
            PackBoard();
            UnpackBoard(boardIndex);
        }

        public void SetMessage(int duration, IMessage message)
        {
            var index = _actors.ActorIndexAt(new Location(0, 0));
            if (index >= 0)
            {
                RemoveActor(index);
                _hud.UpdateBorder();
            }

            var topMessage = message.Text[0];
            var bottomMessage = message.Text.Length > 1 ? message.Text[1] : string.Empty;

            SpawnActor(new Location(0, 0), new Tile(_elements.MessengerId, 0), 1, _state.DefaultActor);
            _actors[_state.ActorCount].P2 = duration / (_state.GameWaitTime + 1);
            _state.Message = topMessage;
            _state.Message2 = bottomMessage;
        }

        public virtual void ShowInGameHelp()
        {
            ShowHelp("GAME");
        }

        public void SpawnActor(IXyPair location, ITile tile, int cycle, IActor source)
        {
            // must reserve one actor for player, and one for messenger
            if (_state.ActorCount < _actors.Capacity - 2)
            {
                _state.ActorCount++;
                var actor = _actors[_state.ActorCount];

                if (source == null)
                {
                    source = _state.DefaultActor;
                }

                actor.CopyFrom(source);
                actor.Location.CopyFrom(location);
                actor.Cycle = cycle;
                actor.UnderTile.CopyFrom(_grid.TileAt(location));
                if (_grid.ElementAt(actor.Location).IsEditorFloor)
                {
                    var newColor = _grid.TileAt(actor.Location).Color & 0x70;
                    newColor |= tile.Color & 0x0F;
                    _grid.TileAt(actor.Location).Color = newColor;
                }
                else
                {
                    _grid.TileAt(actor.Location).Color = tile.Color;
                }

                _grid.TileAt(actor.Location).Id = tile.Id;
                if (actor.Location.Y > 0)
                {
                    UpdateBoard(actor.Location);
                }
            }
        }

        public bool SpawnProjectile(int id, IXyPair location, IXyPair vector, bool enemyOwned)
        {
            var target = location.Sum(vector);
            var element = _grid.ElementAt(target);

            if (element.IsFloor || element.Id == _elements.WaterId)
            {
                SpawnActor(target, new Tile(id, _elements[id].Color), 1, _state.DefaultActor);
                var actor = _actors[_state.ActorCount];
                actor.P1 = enemyOwned ? 1 : 0;
                actor.Vector.CopyFrom(vector);
                actor.P2 = 0x64;
                return true;
            }

            if ((element.Id != _elements.BreakableId) &&
                (!element.IsDestructible ||
                 (((element.Id != _elements.PlayerId) || (_world.EnergyCycles != 0)) && enemyOwned)))
            {
                return false;
            }

            Destroy(target);
            this.PlaySound(2, _sounds.BulletDie);
            return true;
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

        public string StoneText
        {
            get
            {
                foreach (var flag in _world.Flags.Select(f => f.ToUpperInvariant()))
                {
                    if (flag.Length > 0 && flag.StartsWith("Z"))
                    {
                        return flag.Substring(1);
                    }
                }

                return string.Empty;
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
            _gameSerializer.UnpackBoard(_grid, _boards[boardIndex].Data);
            _world.BoardIndex = boardIndex;
        }

        public void UpdateBoard(IXyPair location)
        {
            DrawTile(location, Draw(location));
        }

        public void UpdateRadius(IXyPair location, RadiusMode mode)
        {
            var source = location.Clone();
            var left = source.X - 9;
            var right = source.X + 9;
            var top = source.Y - 6;
            var bottom = source.Y + 6;
            for (var x = left; x <= right; x++)
            {
                for (var y = top; y <= bottom; y++)
                {
                    if (x >= 1 && x <= _grid.Width && y >= 1 && y <= _grid.Height)
                    {
                        var target = new Location(x, y);
                        if (mode != RadiusMode.Update)
                        {
                            if (Distance(source, target) < 50)
                            {
                                var element = _grid.ElementAt(target);
                                if (mode == RadiusMode.Explode)
                                {
                                    if (element.CodeEditText.Length > 0)
                                    {
                                        var actorIndex = _actors.ActorIndexAt(target);
                                        if (actorIndex > 0)
                                        {
                                            BroadcastLabel(-actorIndex, KnownLabels.Bombed, false);
                                        }
                                    }

                                    if (element.IsDestructible || element.Id == _elements.StarId)
                                    {
                                        Destroy(target);
                                    }

                                    if (element.Id == _elements.EmptyId || element.Id == _elements.BreakableId)
                                    {
                                        _grid.TileAt(target).SetTo(_elements.BreakableId, _random.Synced(7) + 9);
                                    }
                                }
                                else
                                {
                                    if (_grid.TileAt(target).Id == _elements.BreakableId)
                                    {
                                        _grid.TileAt(target).Id = _elements.EmptyId;
                                    }
                                }
                            }
                        }

                        UpdateBoard(target);
                    }
                }
            }
        }

        public void UpdateStatus()
        {
            _hud.UpdateStatus();
        }

        public virtual void WaitForTick()
        {
            while (TimerTick == TimerBase && ThreadActive)
            {
                Thread.Sleep(1);
            }

            TimerTick++;
        }

        protected virtual bool ActorIsLocked(int index)
        {
            return _actors[index].P2 != 0;
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
            for (var y = 0; y <= _grid.Height + 1; y++)
            {
                _grid.TileAt(0, y).Id = boardEdgeId;
                _grid.TileAt(_grid.Width + 1, y).Id = boardEdgeId;
            }

            for (var x = 0; x <= _grid.Width + 1; x++)
            {
                _grid.TileAt(x, 0).Id = boardEdgeId;
                _grid.TileAt(x, _grid.Height + 1).Id = boardEdgeId;
            }

            // clear out board
            for (var x = 1; x <= _grid.Width; x++)
            {
                for (var y = 1; y <= _grid.Height; y++)
                {
                    _grid.TileAt(x, y).SetTo(emptyId, 0);
                }
            }

            // build border
            for (var y = 1; y <= _grid.Height; y++)
            {
                _grid.TileAt(1, y).SetTo(boardBorderId, boardBorderColor);
                _grid.TileAt(_grid.Width, y).SetTo(boardBorderId, boardBorderColor);
            }

            for (var x = 1; x <= _grid.Width; x++)
            {
                _grid.TileAt(x, 1).SetTo(boardBorderId, boardBorderColor);
                _grid.TileAt(x, _grid.Height).SetTo(boardBorderId, boardBorderColor);
            }

            // generate player actor
            var element = _elements[_elements.PlayerId];
            _state.ActorCount = 0;
            _actors.Player.Location.SetTo(_grid.Width / 2, _grid.Height / 2);
            _grid.TileAt(_actors.Player.Location).SetTo(element.Id, element.Color);
            _actors.Player.Cycle = 1;
            _actors.Player.UnderTile.SetTo(0, 0);
            _actors.Player.Pointer = 0;
            _actors.Player.Length = 0;
        }

        private int ColorMatch(ITile tile)
        {
            var element = _elements[tile.Id];

            if (element.Color < 0xF0)
                return element.Color & 7;
            if (element.Color == 0xFE)
                return ((tile.Color >> 4) & 0x0F) + 8;
            return tile.Color & 0x0F;
        }

        private int Distance(IXyPair a, IXyPair b)
        {
            return (a.Y - b.Y).Square() * 2 + (a.X - b.X).Square();
        }

        private void DrawTile(IXyPair location, AnsiChar ac)
        {
            _hud.DrawTile(location.X - 1, location.Y - 1, ac);
        }

        private void EnterHighScore(int score)
        {
        }

        protected virtual void ExecuteDeath(IOopContext context)
        {
            var location = context.Actor.Location.Clone();
            Harm(context.Index);
            PlotTile(location, context.DeathTile);
        }

        protected virtual void ExecuteDirection(IOopContext context, IXyPair vector)
        {
            if (vector.IsZero())
            {
                context.Repeat = false;
            }
            else
            {
                var target = context.Actor.Location.Sum(vector);
                if (!_grid.ElementAt(target).IsFloor)
                {
                    Push(target, vector);
                }

                if (_grid.ElementAt(target).IsFloor)
                {
                    MoveActor(context.Index, target);
                    context.Repeat = false;
                }
            }
        }

        protected virtual void ExecuteMessage(IOopContext context)
        {
            if (context.Message.Count == 1)
            {
                SetMessage(0xC8, new Message(context.Message));
            }
            else
            {
                _state.KeyVector.SetTo(0, 0);
                _hud.ShowScroll(context.Message);
            }
        }

        private void FadeBoard(AnsiChar ac)
        {
            _hud.FadeBoard(ac);
        }

        public void FadeRed()
        {
            FadeBoard(new AnsiChar(0xDB, 0x04));
            RedrawBoard();
        }

        private IXyPair GetConveyorVector(int index)
        {
            return new Vector(_state.Vector8[index], _state.Vector8[index + 8]);
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

        public abstract bool HandleTitleInput(int hotkey);

        protected virtual void InitializeElements(bool showInvisibles)
        {
            // this isn't all the initializations.
            // todo: replace this with the ability to completely reinitialize engine default memory
            _elements[_elements.InvisibleId].Character = showInvisibles ? 0xB0 : 0x20;
            _elements[_elements.InvisibleId].Color = 0xFF;
            _elements[_elements.PlayerId].Character = 0x02;
        }

        protected virtual byte[] LoadFile(string filename)
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
            _grid.TileAt(_actors.Player.Location).SetTo(element.Id, element.Color);
            if (_state.PlayerElement == _elements.MonitorId)
            {
                SetMessage(0, new Message());
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
                                _elements[_grid.TileAt(actorData.Location).Id].Act(_state.ActIndex);
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
                        if (_grid.TileAt(_actors.Player.Location).Id == _elements.PlayerId)
                        {
                            DrawTile(_actors.Player.Location, new AnsiChar(0x20, 0x0F));
                        }
                        else
                        {
                            UpdateBoard(_actors.Player.Location);
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
                        _grid.ElementAt(target).Interact(target, 0, _state.KeyVector);
                    }

                    if (!_state.KeyVector.IsZero())
                    {
                        var target = _actors.Player.Location.Sum(_state.KeyVector);
                        if (_grid.ElementAt(target).IsFloor)
                        {
                            if (_grid.ElementAt(_actors.Player.Location).Id == _elements.PlayerId)
                            {
                                MoveActor(0, target);
                            }
                            else
                            {
                                UpdateBoard(_actors.Player.Location);
                                _actors.Player.Location.Add(_state.KeyVector);
                                _grid.TileAt(_actors.Player.Location)
                                    .SetTo(_elements.PlayerId, _elements[_elements.PlayerId].Color);
                                UpdateBoard(_actors.Player.Location);
                                UpdateRadius(_actors.Player.Location, RadiusMode.Update);
                                UpdateRadius(_actors.Player.Location.Difference(_state.KeyVector), RadiusMode.Update);
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
                    ClearSound();
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
                    _grid.TileAt(_actors.Player.Location).SetTo(element.Id, element.Color);
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

        protected void MoveTile(IXyPair source, IXyPair target)
        {
            var sourceIndex = _actors.ActorIndexAt(source);
            if (sourceIndex >= 0)
            {
                MoveActor(sourceIndex, target);
            }
            else
            {
                _grid.TileAt(target).CopyFrom(_grid.TileAt(source));
                UpdateBoard(target);
                RemoveItem(source);
                UpdateBoard(source);
            }
        }

        public bool PlayWorld()
        {
            var gameIsActive = false;

            if (_world.IsLocked)
            {
                var file = LoadFile(GetWorldName(string.IsNullOrWhiteSpace(_world.Name)
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

        protected abstract string GetWorldName(string baseName);

        protected virtual void ReadInput()
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

        private void ResetAlerts()
        {
            _alerts.AmmoPickup = true;
            _alerts.Dark = true;
            _alerts.EnergizerPickup = true;
            _alerts.FakeWall = true;
            _alerts.Forest = true;
            _alerts.GemPickup = true;
            _alerts.OutOfAmmo = true;
            _alerts.CantShootHere = true;
            _alerts.NotDark = true;
            _alerts.NoTorches = true;
            _alerts.TorchPickup = true;
        }

        private void Rnd(IXyPair result)
        {
            result.X = _random.Synced(3) - 1;
            if (result.X == 0)
            {
                result.Y = (_random.Synced(2) << 1) - 1;
            }
            else
            {
                result.Y = 0;
            }
        }

        protected void SetEditorMode()
        {
            InitializeElements(true);
            _state.EditorMode = true;
        }

        protected void SetGameMode()
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

        protected virtual void StartInit()
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

        protected virtual void StartMain()
        {
            StartInit();
            TitleScreenLoop();
            Terminated?.Invoke(this, EventArgs.Empty);
        }

        private void StopSound()
        {
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
                    var startPlaying = HandleTitleInput(hotkey);
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