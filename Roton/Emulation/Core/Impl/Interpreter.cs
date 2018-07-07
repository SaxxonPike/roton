using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Core;
using Roton.Emulation.Cheats;
using Roton.Emulation.Commands;
using Roton.Extensions;

namespace Roton.Emulation.Execution
{
    public class Interpreter : IInterpreter
    {
        private readonly IWorld _world;
        private readonly IActors _actors;
        private readonly IRandom _random;
        private readonly IParser _parser;
        private readonly IBroadcaster _broadcaster;
        private readonly Lazy<ICheats> _cheats;
        private readonly Lazy<ICommands> _commands;
        private readonly ICompass _compass;
        private readonly ITiles _tiles;
        private readonly IMessenger _messenger;

        public Interpreter(
            IWorld world,
            IActors actors,
            IRandom random,
            IParser parser,
            IBroadcaster broadcaster,
            Lazy<ICheats> cheats,
            Lazy<ICommands> commands,
            ICompass compass,
            ITiles tiles,
            IMessenger messenger)
        {
            _world = world;
            _tiles = tiles;
            _actors = actors;
            _random = random;
            _parser = parser;
            _broadcaster = broadcaster;
            _cheats = cheats;
            _commands = commands;
            _compass = compass;
            _messenger = messenger;
        }

        public void Cheat(string input)
        {
            if (input == null)
                return;

            _cheats.Value.Get(input.ToUpperInvariant())?.Execute();

            if (input.Length < 2)
                return;

            if (input.StartsWith("+"))
                _world.Flags.Add(input.Substring(1));
            else if (input.StartsWith("-"))
                _world.Flags.Remove(input.Substring(1));
        }

        public void Execute(IOopContext context)
        {
            while (true)
            {
                context.Resume = false;
                context.Executed = true;

                var name = _parser.ReadWord(context.Index, context);
                if (name.Length == 0)
                    break;

                var command = _commands.Value.Get(name);

                if (command != null)
                {
                    command.Execute(context);
                }
                else
                {
                    if (!_broadcaster.BroadcastLabel(context.Index, name, false))
                    {
                        if (!name.Contains(':'))
                        {
                            _messenger.RaiseError($"Bad command {name}");
                        }
                    }
                    else
                    {
                        context.NextLine = false;
                    }
                }

                if (context.Executed)
                {
                    context.CommandsExecuted++;
                    context.Executed = false;
                }
                else
                {
                    context.Resume = true;
                }

                if (context.Resume)
                {
                    context.Resume = false;
                }
                else
                {
                    if (context.NextLine && context.Instruction > 0)
                    {
                        _parser.ReadLine(context.Index, context);
                    }
                    break;
                }
            }
        }

        protected bool? Condition_Aligned(IOopContext context)
        {
            return context.Actor.Location.X == _actors.Player.Location.X ||
                   context.Actor.Location.Y == _actors.Player.Location.Y;
        }

        protected bool? Condition_Any(IOopContext context)
        {
            var kind = _parser.GetKind(context);
            if (kind == null)
                return null;

            return _tiles.FindTile(kind, new Location(0, 1));
        }

        protected bool? Condition_Blocked(IOopContext context)
        {
            var direction = _parser.GetDirection(context);
            if (direction == null)
                return null;

            return !_tiles.ElementAt(context.Actor.Location.Sum(direction)).IsFloor;
        }

        protected bool? Condition_Contact(IOopContext context)
        {
            var player = _actors.Player;
            var distance = new Location16(context.Actor.Location).Difference(player.Location);
            return distance.X*distance.X + distance.Y*distance.Y == 1;
        }

        protected bool? Condition_Energized(IOopContext context)
        {
            return _world.EnergyCycles > 0;
        }

        protected bool? Condition_Not(IOopContext context)
        {
            return !_parser.GetCondition(context);
        }

        protected IXyPair Direction_Ccw(IOopContext context)
        {
            return _parser.GetDirection(context).CounterClockwise();
        }

        protected IXyPair Direction_Cw(IOopContext context)
        {
            return _parser.GetDirection(context).Clockwise();
        }

        protected IXyPair Direction_East(IOopContext context)
        {
            return Vector.East;
        }

        protected IXyPair Direction_Flow(IOopContext context)
        {
            return context.Actor.Vector.Clone();
        }

        protected IXyPair Direction_Idle(IOopContext context)
        {
            return Vector.Idle;
        }

        protected IXyPair Direction_North(IOopContext context)
        {
            return Vector.North;
        }

        protected IXyPair Direction_Opp(IOopContext context)
        {
            return _parser.GetDirection(context).Opposite();
        }

        protected IXyPair Direction_Rnd(IOopContext context)
        {
            return _compass.Rnd();
        }

        protected IXyPair Direction_RndNe(IOopContext context)
        {
            return _random.Synced(2) == 0
                ? Vector.North
                : Vector.East;
        }

        protected IXyPair Direction_RndNs(IOopContext context)
        {
            return _random.Synced(2) == 0
                ? Vector.North
                : Vector.South;
        }

        protected IXyPair Direction_RndP(IOopContext context)
        {
            var direction = _parser.GetDirection(context);
            return _random.Synced(2) == 0
                ? direction.Clockwise()
                : direction.CounterClockwise();
        }

        protected IXyPair Direction_Seek(IOopContext context)
        {
            return _compass.Seek(context.Actor.Location);
        }

        protected IXyPair Direction_South(IOopContext context)
        {
            return Vector.South;
        }

        protected IXyPair Direction_West(IOopContext context)
        {
            return Vector.West;
        }

        protected virtual IDictionary<string, Func<IOopContext, bool?>> GetConditions()
        {
            return new Dictionary<string, Func<IOopContext, bool?>>
            {
                {"ALLIGNED", Condition_Aligned},
                {"ANY", Condition_Any},
                {"BLOCKED", Condition_Blocked},
                {"CONTACT", Condition_Contact},
                {"ENERGIZED", Condition_Energized},
                {"NOT", Condition_Not}
            };
        }

        protected virtual IDictionary<string, Func<IOopContext, IXyPair>> GetDirections()
        {
            return new Dictionary<string, Func<IOopContext, IXyPair>>
            {
                {"CW", Direction_Cw},
                {"CCW", Direction_Ccw},
                {"E", Direction_East},
                {"EAST", Direction_East},
                {"FLOW", Direction_Flow},
                {"I", Direction_Idle},
                {"IDLE", Direction_Idle},
                {"N", Direction_North},
                {"NORTH", Direction_North},
                {"OPP", Direction_Opp},
                {"RND", Direction_Rnd},
                {"RNDNE", Direction_RndNe},
                {"RNDNS", Direction_RndNs},
                {"RNDP", Direction_RndP},
                {"S", Direction_South},
                {"SOUTH", Direction_South},
                {"SEEK", Direction_Seek},
                {"W", Direction_West},
                {"WEST", Direction_West}
            };
        }

        protected virtual IDictionary<string, Func<IOopContext, IOopItem>> GetItems()
        {
            return new Dictionary<string, Func<IOopContext, IOopItem>>
            {
                {"AMMO", Item_Ammo},
                {"GEMS", Item_Gems},
                {"HEALTH", Item_Health},
                {"SCORE", Item_Score},
                {"TIME", Item_Time},
                {"TORCHES", Item_Torches}
            };
        }
        
        protected IOopItem Item_Ammo(IOopContext context)
        {
            return new OopItem(
                () => _world.Ammo,
                v => _world.Ammo = v);
        }

        protected IOopItem Item_Gems(IOopContext context)
        {
            return new OopItem(
                () => _world.Gems,
                v => _world.Gems = v);
        }

        protected IOopItem Item_Health(IOopContext context)
        {
            return new OopItem(
                () => _world.Health,
                v => _world.Health = v);
        }

        protected IOopItem Item_Score(IOopContext context)
        {
            return new OopItem(
                () => _world.Score,
                v => _world.Score = v);
        }

        protected IOopItem Item_Stones(IOopContext context)
        {
            return new OopItem(
                () => _world.Stones,
                v => _world.Stones = v);
        }

        protected IOopItem Item_Time(IOopContext context)
        {
            return new OopItem(
                () => _world.TimePassed,
                v => _world.TimePassed = v);
        }

        protected IOopItem Item_Torches(IOopContext context)
        {
            return new OopItem(
                () => _world.Torches,
                v => _world.Torches = v);
        }
    }
}