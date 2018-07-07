using Roton.Emulation.Execution;
using Roton.Emulation.Mapping;
using Roton.Emulation.SuperZZT;
using Roton.Extensions;

namespace Roton.Core
{
    public class Mover : IMover
    {
        private readonly IWorld _world;
        private readonly IHud _hud;
        private readonly IElements _elements;
        private readonly ISounder _sounder;
        private readonly ISounds _sounds;
        private readonly IState _state;
        private readonly IActors _actors;
        private readonly IPlotter _plotter;
        private readonly IMessager _messager;
        private readonly IAlerts _alerts;
        private readonly IBoard _board;
        private readonly IDrawer _drawer;
        private readonly IRadius _radius;
        private readonly ICompass _compass;
        private readonly ITiles _tiles;
        private readonly IMisc _misc;

        public Mover(IWorld world, IHud hud, IElements elements, ISounder sounder, ISounds sounds,
            IState state, IActors actors, IPlotter plotter, IMessager messager, IAlerts alerts, IBoard board, IDrawer drawer, IRadius radius,
            ICompass compass, ITiles tiles, IMisc misc)
        {
            _world = world;
            _hud = hud;
            _elements = elements;
            _sounder = sounder;
            _sounds = sounds;
            _state = state;
            _actors = actors;
            _plotter = plotter;
            _messager = messager;
            _alerts = alerts;
            _board = board;
            _drawer = drawer;
            _radius = radius;
            _compass = compass;
            _tiles = tiles;
            _misc = misc;
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
                _sounder.Play(2, _sounds.EnemySuicide);
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
                surrounding[i] = _tiles[center.Sum(_compass.GetConveyorVector(i))].Clone();
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
                        var source = center.Sum(_compass.GetConveyorVector(i));
                        var target = center.Sum(_compass.GetConveyorVector((i + 8 - direction) % 8));
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
                            _drawer.UpdateBoard(target);
                        }

                        if (!_elements[surrounding[(i + 8 + direction) % 8].Id].IsPushable)
                        {
                            _tiles[source].Id = _elements.EmptyId;
                            _drawer.UpdateBoard(source);
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
                _misc.RemoveItem(location);
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
                    _messager.SetMessage(0x64, _alerts.OuchMessage);
                    var color = _tiles[actor.Location].Color;
                    color &= 0x0F;
                    color |= 0x70;
                    _tiles[actor.Location].Color = color;
                    if (_world.Health > 0)
                    {
                        _world.TimePassed = 0;
                        if (_board.RestartOnZap)
                        {
                            _sounder.Play(4, _sounds.TimeOut);
                            _tiles[actor.Location].Id = _elements.EmptyId;
                            _drawer.Draw(actor.Location);
                            var oldLocation = actor.Location.Clone();
                            actor.Location.CopyFrom(_board.Entrance);
                            _radius.Update(oldLocation, 0);
                            _radius.Update(actor.Location, 0);
                            _state.GamePaused = true;
                        }

                        _sounder.Play(4, _sounds.Ouch);
                    }
                    else
                    {
                        _sounder.Play(5, _sounds.GameOver);
                    }
                }
            }
            else
            {
                var element = _tiles[actor.Location].Id;
                if (element == _elements.BulletId)
                {
                    _sounder.Play(3, _sounds.BulletDie);
                }
                else if (element != _elements.ObjectId)
                {
                    _sounder.Play(3, _sounds.EnemyDie);
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
                _plotter.ForcePlayerColor(index);
            }

            _drawer.UpdateBoard(target);
            _drawer.UpdateBoard(sourceLocation);

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
                                    _drawer.UpdateBoard(glowLocation);
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
                _drawer.UpdateBoard(target);
                _misc.RemoveItem(source);
                _drawer.UpdateBoard(source);
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
                    _sounder.Play(3, _sounds.Transporter);
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
                _drawer.UpdateBoard(actor.Location);
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
    }
}