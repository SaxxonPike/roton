using System;
using System.Threading;
using Roton.Emulation.Behaviors;
using Roton.Emulation.Execution;
using Roton.Emulation.Mapping;
using Roton.Emulation.Timing;
using Roton.Events;
using Roton.Extensions;
using Roton.FileIo;

namespace Roton.Core
{
    public interface IGalaxy
    {
        void LockActor(int index);
        void UnlockActor(int index);
        bool IsActorLocked(int index);
        void RemoveItem(IXyPair location);
        string GetWorldName(string baseName);
        void EnterBoard();
        bool HandleTitleInput(int hotkey);
        void ShowInGameHelp();
        void ExecuteMessage(IOopContext context);
    }
    
    public interface IUniverse
    {
        bool BroadcastLabel(int sender, string label, bool force);
        AnsiChar Draw(IXyPair location);
        IXyPair GetConveyorVector(int index);
        IXyPair GetCardinalVector(int index);
        void PlaySound(int priority, ISound sound);
        void PlaySound(int priority, ISound sound, int offset, int length);
        void RaiseError(string error);
        IXyPair Rnd();
        IXyPair RndP(IXyPair vector);
        IXyPair Seek(IXyPair location);
        void SetMessage(int duration, IMessage message);
        bool Transact(IOopContext context, bool take);
        void Radius(IXyPair location, RadiusMode mode);
    }

    public class Universe : IUniverse
    {
        private readonly IWorld _world;
        private readonly IBoard _board;
        private readonly ITiles _tiles;
        private readonly IActors _actors;
        private readonly IState _state;
        private readonly IElements _elements;
        private readonly IInterpreter _interpreter;
        private readonly IParser _parser;
        private readonly IRandom _random;
        private readonly IHud _hud;
        private readonly ISounds _sounds;
        private readonly IAlerts _alerts;
        private readonly IGalaxy _galaxy;
        private readonly IClock _clock;
        private readonly IBoards _boards;
        private readonly IGameSerializer _gameSerializer;
        private readonly IOopContextFactory _oopContextFactory;
        private readonly IKeyboard _keyboard;
        private readonly ITimers _timers;
        private readonly IFileSystem _fileSystem;

        public Universe(IWorld world, IBoard board, ITiles tiles, IActors actors, IState state, IElements elements,
            IInterpreter interpreter, IParser parser, IRandom random, IHud hud, ISounds sounds, IAlerts alerts,
            IGalaxy galaxy, IClock clock, IBoards boards, IGameSerializer gameSerializer, IOopContextFactory oopContextFactory,
            IKeyboard keyboard, ITimers timers, IFileSystem fileSystem)
        {
            _world = world;
            _board = board;
            _tiles = tiles;
            _actors = actors;
            _state = state;
            _elements = elements;
            _interpreter = interpreter;
            _parser = parser;
            _random = random;
            _hud = hud;
            _sounds = sounds;
            _alerts = alerts;
            _galaxy = galaxy;
            _clock = clock;
            _boards = boards;
            _gameSerializer = gameSerializer;
            _oopContextFactory = oopContextFactory;
            _keyboard = keyboard;
            _timers = timers;
            _fileSystem = fileSystem;
        }

        public int Adjacent(IXyPair location, int id)
        {
            return (location.Y <= 1 || _tiles[location.Sum(Vector.North)].Id == id ? 1 : 0) |
                   (location.Y >= _tiles.Height || _tiles[location.Sum(Vector.South)].Id == id ? 2 : 0) |
                   (location.X <= 1 || _tiles[location.Sum(Vector.West)].Id == id ? 4 : 0) |
                   (location.X >= _tiles.Width || _tiles[location.Sum(Vector.East)].Id == id ? 8 : 0);
        }

        public AnsiChar Draw(IXyPair location)
        {
            if (_board.IsDark && !_tiles.ElementAt(location).IsAlwaysVisible &&
                (_world.TorchCycles <= 0 || _actors.Player.Location.DistanceTo(location) >= 50) && !_state.EditorMode)
            {
                return new AnsiChar(0xB0, 0x07);
            }

            var tile = _tiles[location];
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

        public IXyPair GetCardinalVector(int index)
        {
            return new Vector(_state.Vector4[index], _state.Vector4[index + 4]);
        }

        public IXyPair GetConveyorVector(int index)
        {
            return new Vector(_state.Vector8[index], _state.Vector8[index + 8]);
        }

        public IXyPair Rnd()
        {
            var result = new Vector();
            Rnd(result);
            return result;
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

        public IXyPair RndP(IXyPair vector)
        {
            var result = new Vector();
            result.CopyFrom(
                _random.Synced(2) == 0
                    ? vector.Clockwise()
                    : vector.CounterClockwise());
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

        public bool Transact(IOopContext context, bool take)
        {
            // Does the item exist?
            var item = _parser.GetItem(context);
            if (item == null)
                return false;

            // Do we have a valid amount?
            var amount = _parser.ReadNumber(context.Index, context);
            if (amount <= 0)
                return true;

            // Modify value if we are taking.
            if (take)
                _state.OopNumber = -_state.OopNumber;

            // Determine if the result will be in range.
            var pendingAmount = item.Value + _state.OopNumber;
            if ((pendingAmount & 0xFFFF) >= 0x8000)
                return true;

            // Successful transaction.
            item.Value = pendingAmount;
            return false;
        }

        public void UpdateBoard(IXyPair location)
        {
            _hud.DrawChar(location.X, location.Y, Draw(location));
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

        public void RaiseError(string error)
        {
            SetMessage(0xC8, _alerts.ErrorMessage(error));
            PlaySound(5, _sounds.Error);
        }

        public void Attack(int index, IXyPair location)
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

            if (index > 0 && index <= _state.ActIndex)
            {
                _state.ActIndex--;
            }

            if (_tiles[location].Id == _elements.PlayerId && _world.EnergyCycles > 0)
            {
                _world.Score += _tiles.ElementAt(_actors[index].Location).Points;
                _hud.UpdateStatus();
            }
            else
            {
                Destroy(location);
                PlaySound(2, _sounds.EnemySuicide);
            }
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
                surrounding[i] = _tiles[center.Sum(GetConveyorVector(i))].Clone();
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
                            var tile = _tiles[source];
                            var index = _actors.ActorIndexAt(source);
                            _tiles[source].CopyFrom(surrounding[i]);
                            _tiles[target].Id = _elements.EmptyId;
                            MoveActor(index, target);
                            _tiles[source].CopyFrom(tile);
                        }
                        else
                        {
                            _tiles[target].CopyFrom(surrounding[i]);
                            UpdateBoard(target);
                        }

                        if (!_elements[surrounding[(i + 8 + direction) % 8].Id].IsPushable)
                        {
                            _tiles[source].Id = _elements.EmptyId;
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

        public void Harm(int index)
        {
            var actor = _actors[index];
            if (index == 0)
            {
                if (_world.Health > 0)
                {
                    _world.Health -= 10;
                    _hud.UpdateStatus();
                    SetMessage(0x64, _alerts.OuchMessage);
                    var color = _tiles[actor.Location].Color;
                    color &= 0x0F;
                    color |= 0x70;
                    _tiles[actor.Location].Color = color;
                    if (_world.Health > 0)
                    {
                        _world.TimePassed = 0;
                        if (_board.RestartOnZap)
                        {
                            PlaySound(4, _sounds.TimeOut);
                            _tiles[actor.Location].Id = _elements.EmptyId;
                            UpdateBoard(actor.Location);
                            var oldLocation = actor.Location.Clone();
                            actor.Location.CopyFrom(_board.Entrance);
                            Radius(oldLocation, 0);
                            Radius(actor.Location, 0);
                            _state.GamePaused = true;
                        }

                        PlaySound(4, _sounds.Ouch);
                    }
                    else
                    {
                        PlaySound(5, _sounds.GameOver);
                    }
                }
            }
            else
            {
                var element = _tiles[actor.Location].Id;
                if (element == _elements.BulletId)
                {
                    PlaySound(3, _sounds.BulletDie);
                }
                else if (element != _elements.ObjectId)
                {
                    PlaySound(3, _sounds.EnemyDie);
                }

                RemoveActor(index);
            }
        }

        public void MoveActor(int index, IXyPair target)
        {
            var actor = _actors[index];
            var sourceLocation = actor.Location.Clone();
            var sourceTile = _tiles[actor.Location];
            var targetTile = _tiles[target];
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
                            if (glowLocation.X >= 1 && glowLocation.X <= _tiles.Width && glowLocation.Y >= 1 &&
                                glowLocation.Y <= _tiles.Height)
                            {
                                if ((sourceLocation.DistanceTo(glowLocation) < 50) ^
                                    (target.DistanceTo(glowLocation) < 50))
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

            if (_tiles.ElementAt(actor.Location).Id == _elements.PlayerId)
            {
                _tiles.ElementAt(actor.Location.Sum(vector)).Interact(actor.Location.Sum(vector), 0, vector);
            }

            if (vector.IsNonZero())
            {
                var target = actor.Location.Sum(vector);
                if (_tiles.ElementAt(target).IsFloor)
                {
                    MoveActor(index, target);
                }
            }
        }

        public void MoveTile(IXyPair source, IXyPair target)
        {
            var sourceIndex = _actors.ActorIndexAt(source);
            if (sourceIndex >= 0)
            {
                MoveActor(sourceIndex, target);
            }
            else
            {
                _tiles[target].CopyFrom(_tiles[source]);
                UpdateBoard(target);
                _galaxy.RemoveItem(source);
                UpdateBoard(source);
            }
        }

        public void Push(IXyPair location, IXyPair vector)
        {
            // this is here to prevent endless push loops
            // but doesn't exist in the original code
            if (vector.IsZero())
            {
                throw Exceptions.PushStackOverflow;
            }

            var tile = _tiles[location];
            if ((tile.Id == _elements.SliderEwId && vector.Y == 0) ||
                (tile.Id == _elements.SliderNsId && vector.X == 0) ||
                _elements[tile.Id].IsPushable)
            {
                var furtherTile = _tiles[location.Sum(vector)];
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
                    var element = _tiles.ElementAt(search);
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
                                element = _tiles.ElementAt(search);
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
                    PlaySound(3, _sounds.Transporter);
                }
            }
        }

        public void RemoveActor(int index)
        {
            var actor = _actors[index];
            if (index < _state.ActIndex)
            {
                _state.ActIndex--;
            }

            _tiles[actor.Location].CopyFrom(actor.UnderTile);
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

        public virtual void ForcePlayerColor(int index)
        {
            var actor = _actors[index];
            var playerElement = _elements[_elements.PlayerId];
            if (_tiles[actor.Location].Color != playerElement.Color ||
                playerElement.Character != 0x02)
            {
                playerElement.Character = 2;
                _tiles[actor.Location].Color = playerElement.Color;
                UpdateBoard(actor.Location);
            }
        }

        public void PlotTile(IXyPair location, ITile tile)
        {
            if (_tiles.ElementAt(location).Id == _elements.PlayerId)
                return;

            var targetElement = _elements[tile.Id];
            var existingTile = _tiles[location];
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

        public void Put(IXyPair location, IXyPair vector, ITile kind)
        {
            if (location.X >= 1 && location.X <= _tiles.Width && location.Y >= 1 &&
                location.Y <= _tiles.Height)
            {
                if (!_tiles.ElementAt(location).IsFloor)
                {
                    Push(location, vector);
                }

                PlotTile(location, kind);
            }
        }

        public virtual void RemoveItem(IXyPair location)
        {
            _tiles[location].Id = _elements.EmptyId;
            UpdateBoard(location);
        }

        public void Radius(IXyPair location, RadiusMode mode)
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
                    if (x >= 1 && x <= _tiles.Width && y >= 1 && y <= _tiles.Height)
                    {
                        var target = new Location(x, y);
                        if (mode != RadiusMode.Update)
                        {
                            if (source.DistanceTo(target) < 50)
                            {
                                var element = _tiles.ElementAt(target);
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
                                        _tiles[target].SetTo(_elements.BreakableId, _random.Synced(7) + 9);
                                    }
                                }
                                else
                                {
                                    if (_tiles[target].Id == _elements.BreakableId)
                                    {
                                        _tiles[target].Id = _elements.EmptyId;
                                    }
                                }
                            }
                        }

                        UpdateBoard(target);
                    }
                }
            }
        }

        public void SpawnActor(IXyPair location, ITile tile, int cycle, IActor source)
        {
            // must reserve one actor for player, and one for messenger
            if (_state.ActorCount >= _actors.Capacity - 2)
                return;

            _state.ActorCount++;
            var actor = _actors[_state.ActorCount];

            if (source == null)
            {
                source = _state.DefaultActor;
            }

            actor.CopyFrom(source);
            actor.Location.CopyFrom(location);
            actor.Cycle = cycle;
            actor.UnderTile.CopyFrom(_tiles[location]);
            if (_tiles.ElementAt(actor.Location).IsEditorFloor)
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
            if (actor.Location.Y > 0)
            {
                UpdateBoard(actor.Location);
            }
        }

        public bool SpawnProjectile(int id, IXyPair location, IXyPair vector, bool enemyOwned)
        {
            var target = location.Sum(vector);
            var element = _tiles.ElementAt(target);

            if (element.IsFloor || element.Id == _elements.WaterId)
            {
                SpawnActor(target, new Tile(id, _elements[id].Color), 1, _state.DefaultActor);
                var actor = _actors[_state.ActorCount];
                actor.P1 = enemyOwned ? 1 : 0;
                actor.Vector.CopyFrom(vector);
                actor.P2 = 0x64;
                return true;
            }

            if (element.Id != _elements.BreakableId &&
                (!element.IsDestructible ||
                 (element.Id != _elements.PlayerId || _world.EnergyCycles != 0) && enemyOwned))
            {
                return false;
            }

            Destroy(target);
            PlaySound(2, _sounds.BulletDie);
            return true;
        }

        private int _timerTick;

        private int TimerBase => _clock.Tick & 0x7FFF;

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
                            RaiseError("Bad direction");
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
                _hud.Initialize();
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

        private void ExecuteDeath(IOopContext context)
        {
            var location = context.Actor.Location.Clone();
            Harm(context.Index);
            PlotTile(location, context.DeathTile);
        }

        private void ExecuteDirection(IOopContext context, IXyPair vector)
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
                    Push(target, vector);
                }

                if (_tiles.ElementAt(target).IsFloor)
                {
                    MoveActor(context.Index, target);
                    context.Repeat = false;
                }
            }
        }

        private void ExecuteMessage(IOopContext context)
        {
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

        private void InitializeElements(bool showInvisibles)
        {
            // this isn't all the initializations.
            // todo: replace this with the ability to completely reinitialize engine default memory
            _elements[_elements.InvisibleId].Character = showInvisibles ? 0xB0 : 0x20;
            _elements[_elements.InvisibleId].Color = 0xFF;
            _elements[_elements.PlayerId].Character = 0x02;
        }

        private byte[] LoadFile(string filename)
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
                        _tiles.ElementAt(target).Interact(target, 0, _state.KeyVector);
                    }

                    if (!_state.KeyVector.IsZero())
                    {
                        var target = _actors.Player.Location.Sum(_state.KeyVector);
                        if (_tiles.ElementAt(target).IsFloor)
                        {
                            if (_tiles.ElementAt(_actors.Player.Location).Id == _elements.PlayerId)
                            {
                                MoveActor(0, target);
                            }
                            else
                            {
                                UpdateBoard(_actors.Player.Location);
                                _actors.Player.Location.Add(_state.KeyVector);
                                _tiles[_actors.Player.Location]
                                    .SetTo(_elements.PlayerId, _elements[_elements.PlayerId].Color);
                                UpdateBoard(_actors.Player.Location);
                                Radius(_actors.Player.Location, RadiusMode.Update);
                                Radius(_actors.Player.Location.Difference(_state.KeyVector), RadiusMode.Update);
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
                var file = LoadFile(_galaxy.GetWorldName(string.IsNullOrWhiteSpace(_world.Name)
                    ? _state.WorldFileName
                    : _world.Name));
                if (file != null)
                {
                    //RequestReplaceContext?.Invoke(this, new DataEventArgs {Data = file});
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
                _galaxy.EnterBoard();
                _state.PlayerElement = _elements.PlayerId;
                _state.GamePaused = true;
                MainLoop(true);
            }

            return gameIsActive;
        }

        private void ReadInput()
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

        private void StartInit()
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

        private void StartMain()
        {
            StartInit();
            TitleScreenLoop();
        }

        private void TitleScreenLoop()
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
                    var startPlaying = _galaxy.HandleTitleInput(hotkey);
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

        public bool BroadcastLabel(int sender, string label, bool force)
        {
            var external = false;
            var success = false;

            if (sender < 0)
            {
                external = true;
                sender = -sender;
            }

            var info = new SearchContext
            {
                SearchIndex = 0,
                SearchOffset = 0,
                SearchTarget = label
            };

            while (ExecuteLabel(sender, info, "\x000D:"))
            {
                if (!_galaxy.IsActorLocked(info.SearchIndex) || force || (sender == info.SearchIndex && !external))
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
                success = _parser.GetTarget(context);
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
                    context.SearchOffset = _parser.Search(context.SearchIndex, prefix + label);
                    if (context.SearchOffset < 0 && split > 0)
                    {
                        success = _parser.GetTarget(context);
                        continue;
                    }
                }

                success = context.SearchOffset >= 0;
                break;
            }

            return success;
        }

        public void ClearSound()
        {
            _state.SoundPlaying = false;
            StopSound();
        }

        public ISound EncodeMusic(string music)
        {
            return new Sound();
        }

        public void PlaySound(int priority, ISound sound, int offset, int length)
        {
        }

        public void PlaySound(int priority, ISound sound) => 
            PlaySound(priority, sound, 0, sound.Length);

        public void StopSound()
        {
        }
    }
}