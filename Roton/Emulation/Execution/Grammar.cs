using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Emulation.Execution
{
    public abstract class Grammar : IGrammar
    {
        private readonly IColors _colors;
        private readonly IElements _elements;
        private readonly IWorld _world;
        private readonly IBoard _board;
        private readonly IEngine _engine;
        private readonly ISounds _sounds;
        private readonly IGrid _grid;
        private readonly IActors _actors;
        private readonly IFlags _flags;
        private readonly IState _state;
        private readonly IRandom _random;

        protected Grammar(
            IColors colors, 
            IElements elements, 
            IWorld world, 
            IBoard board, 
            IEngine engine,
            ISounds sounds,
            IGrid grid,
            IActors actors,
            IFlags flags,
            IState state,
            IRandom random)
        {
            _colors = colors;
            _elements = elements;
            _world = world;
            _board = board;
            _engine = engine;
            _sounds = sounds;
            _grid = grid;
            _actors = actors;
            _flags = flags;
            _state = state;
            _random = random;
            Cheats = GetCheats();
            Commands = GetCommands();
            Conditions = GetConditions();
            Directions = GetDirections();
            Items = GetItems();
            Targets = GetTargets();
        }

        private IDictionary<string, Action<IEngine>> Cheats { get; }
        private IDictionary<string, Action<IOopContext>> Commands { get; }
        private IDictionary<string, Func<IOopContext, bool?>> Conditions { get; }
        private IDictionary<string, Func<IOopContext, IXyPair>> Directions { get; }
        private IDictionary<string, Func<IOopContext, IOopItem>> Items { get; }
        private IDictionary<string, Func<ISearchContext, bool>> Targets { get; }

        public void Cheat(string input)
        {
            if (input == null)
                return;

            Action<IEngine> func;
            Cheats.TryGetValue(input.ToUpperInvariant(), out func);
            func?.Invoke(_engine);

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

                var command = _engine.ReadActorCodeWord(context.Index, context);
                if (command.Length == 0)
                    break;

                Action<IOopContext> func;
                Commands.TryGetValue(command, out func);

                if (func != null)
                {
                    func.Invoke(context);
                }
                else
                {
                    if (!_engine.BroadcastLabel(context.Index, command, false))
                    {
                        if (!command.Contains(':'))
                        {
                            _engine.RaiseError($"Bad command {command}");
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
                        _engine.ReadActorCodeLine(context.Index, context);
                    }
                    break;
                }
            }
        }

        public bool? GetCondition(IOopContext oopContext)
        {
            var condition = _engine.ReadActorCodeWord(oopContext.Index, oopContext);
            Func<IOopContext, bool?> func;
            Conditions.TryGetValue(condition, out func);
            return func != null
                ? func(oopContext)
                : _flags.Contains(condition);
        }

        public IXyPair GetDirection(IOopContext oopContext)
        {
            var direction = _engine.ReadActorCodeWord(oopContext.Index, oopContext);
            Func<IOopContext, IXyPair> func;
            Directions.TryGetValue(direction, out func);
            return func?.Invoke(oopContext);
        }

        public IOopItem GetItem(IOopContext oopContext)
        {
            var item = _engine.ReadActorCodeWord(oopContext.Index, oopContext);
            Func<IOopContext, IOopItem> func;
            Items.TryGetValue(item, out func);
            return func?.Invoke(oopContext);
        }

        public ITile GetKind(IOopContext oopContext)
        {
            var word = _engine.ReadActorCodeWord(oopContext.Index, oopContext);
            var result = new Tile(0, 0);
            var success = false;

            for (var i = 1; i < 8; i++)
            {
                if (_colors[i].ToUpperInvariant() != word)
                    continue;

                result.Color = i + 8;
                word = _engine.ReadActorCodeWord(oopContext.Index, oopContext);
                break;
            }

            foreach (var element in _elements.Where(e => e != null))
            {
                if (new string(element.Name.ToUpperInvariant().Where(c => c >= 0x41 && c <= 0x5A).ToArray()) != word)
                    continue;

                success = true;
                result.Id = element.Id;
                break;
            }

            return success ? result : null;
        }

        public bool GetTarget(ISearchContext context)
        {
            context.SearchIndex++;
            Func<ISearchContext, bool> func;
            Targets.TryGetValue(context.SearchTarget, out func);
            return func?.Invoke(context) ?? Target_Default(context);
        }

        protected void Cheat_Ammo()
        {
            _world.Ammo += 5;
        }

        protected void Cheat_Dark()
        {
            _board.IsDark = true;
            _engine.RedrawBoard();
        }

        protected void Cheat_Gems()
        {
            _world.Gems += 5;
        }

        protected void Cheat_Health()
        {
            _world.Health += 50;
        }

        protected void Cheat_Keys()
        {
            for (var i = 1; i < 8; i++)
            {
                _world.Keys[i] = true;
            }
        }

        protected void Cheat_Time()
        {
            _world.TimePassed -= 30;
        }

        protected void Cheat_Torches()
        {
            _world.Torches += 3;
        }

        protected void Cheat_Zap()
        {
            for (var i = 0; i < 4; i++)
            {
                _engine.Destroy(_actors.Player.Location.Sum(_engine.GetCardinalVector(i)));
            }
        }

        protected void Command_Become(IOopContext context)
        {
            var kind = GetKind(context);
            if (kind == null)
            {
                _engine.RaiseError("Bad #BECOME");
                return;
            }

            context.Died = true;
            context.DeathTile.CopyFrom(kind);
        }

        protected void Command_Bind(IOopContext context)
        {
            var search = new SearchContext(_engine);
            var target = _engine.ReadActorCodeWord(context.Index, context);
            search.SearchTarget = target;
            if (GetTarget(search))
            {
                var targetActor = _actors[search.SearchIndex];
                context.Actor.Pointer = targetActor.Pointer;
                context.Actor.Length = targetActor.Length;
                context.Instruction = 0;
            }
        }

        protected void Command_Change(IOopContext context)
        {
            var success = false;
            var source = GetKind(context);
            if (source != null)
            {
                var target = GetKind(context);
                if (target != null)
                {
                    var targetElement = _elements[target.Id];
                    success = true;
                    if (target.Color == 0 && targetElement.Color < 0xF0)
                    {
                        target.Color = targetElement.Color;
                    }
                    var location = new Location(0, 1);
                    while (_engine.FindTile(source, location))
                    {
                        _engine.PlotTile(location, target);
                    }
                }
            }

            if (!success)
            {
                _engine.RaiseError("Bad #CHANGE");
            }
        }

        protected void Command_Char(IOopContext context)
        {
            var value = _engine.ReadActorCodeNumber(context.Index, context);
            if (value >= 0)
            {
                context.Actor.P1 = value;
                _engine.UpdateBoard(context.Actor.Location);
            }
        }

        protected void Command_Clear(IOopContext context)
        {
            var flag = _engine.ReadActorCodeWord(context.Index, context);
            _flags.Remove(flag);
        }

        protected void Command_Cycle(IOopContext context)
        {
            var value = _engine.ReadActorCodeNumber(context.Index, context);
            if (value > 0)
            {
                context.Actor.Cycle = value;
            }
        }

        protected void Command_Die(IOopContext context)
        {
            context.Died = true;
            context.DeathTile.SetTo(_elements.EmptyId, 0);
        }

        protected void Command_End(IOopContext context)
        {
            _state.OopByte = 0;
            context.Instruction = -1;
        }

        protected void Command_Endgame(IOopContext context)
        {
            _world.Health = 0;
        }

        protected void Command_Give(IOopContext context)
        {
            context.Resume = ExecuteTransaction(context, false);
            _engine.UpdateStatus();
        }

        protected void Command_Go(IOopContext context)
        {
            var vector = GetDirection(context);
            if (vector != null)
            {
                var target = context.Actor.Location.Sum(vector);
                if (!_grid.ElementAt(target).IsFloor)
                {
                    _engine.Push(target, vector);
                }
                if (_grid.ElementAt(target).IsFloor)
                {
                    _engine.MoveActor(context.Index, target);
                    context.Moved = true;
                }
                else
                {
                    context.Repeat = true;
                }
            }
        }

        protected void Command_Idle(IOopContext context)
        {
            context.Moved = true;
        }

        protected void Command_If(IOopContext context)
        {
            var condition = GetCondition(context);
            if (condition.HasValue)
            {
                context.Resume = condition.Value;
            }
        }

        protected virtual void Command_Lock(IOopContext context)
        {
            context.Actor.P2 = 1;
        }

        protected void Command_Play(IOopContext context)
        {
            var notes = _engine.ReadActorCodeLine(context.Index, context);
            var sound = _engine.EncodeMusic(notes);
            _engine.PlaySound(-1, sound);
            context.NextLine = false;
        }

        protected void Command_Put(IOopContext context)
        {
            var vector = GetDirection(context);
            var success = false;

            if (vector != null)
            {
                var kind = GetKind(context);
                if (kind != null)
                {
                    success = true;
                    PutTile(_engine, context.Actor.Location.Sum(vector), vector, kind);
                }
            }

            if (!success)
                _engine.RaiseError("Bad #PUT");
        }

        protected void Command_Restart(IOopContext context)
        {
            context.Instruction = 0;
            context.NextLine = false;
        }

        protected void Command_Restore(IOopContext context)
        {
            _engine.ReadActorCodeWord(context.Index, context);
            while (true)
            {
                context.SearchTarget = _engine.ReadActorCodeWord(context.Index, context);
                var result = _engine.ExecuteLabel(context.Index, context, "\xD\x27");
                if (!result)
                    break;

                while (context.SearchOffset >= 0)
                {
                    _actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x3A;
                    context.SearchOffset = _engine.SearchActorCode(context.SearchIndex,
                        $"\xD\x27{_engine.ReadActorCodeWord(context.Index, context)}");
                }
            }
        }

        protected void Command_Send(IOopContext context)
        {
            var target = _engine.ReadActorCodeWord(context.Index, context);
            context.NextLine = _engine.BroadcastLabel(context.Index, target, false);
        }

        protected void Command_Set(IOopContext context)
        {
            var flag = _engine.ReadActorCodeWord(context.Index, context);
            _flags.Add(flag);
        }

        protected void Command_Shoot(IOopContext context)
        {
            var vector = GetDirection(context);
            if (vector != null)
            {
                var projectile = _elements.Bullet();
                var success = _engine.SpawnProjectile(projectile.Id, context.Actor.Location, vector, true);
                if (success)
                {
                    _engine.PlaySound(2, _sounds.EnemyShoot);
                }
                context.Moved = true;
            }
        }

        protected void Command_Take(IOopContext context)
        {
            context.Resume = ExecuteTransaction(context, true);
            _engine.UpdateStatus();
        }

        protected void Command_Then(IOopContext context)
        {
            // The actual code doesn't work this way.
            // We cheat a little by not advancing the execution counter.
            context.Resume = true;
            context.CommandsExecuted--;
        }

        protected void Command_Throwstar(IOopContext context)
        {
            var vector = GetDirection(context);
            if (vector != null)
            {
                var projectile = _elements.Star();
                _engine.SpawnProjectile(projectile.Id, context.Actor.Location, vector, true);
            }
            context.Moved = true;
        }

        protected void Command_Try(IOopContext context)
        {
            var vector = GetDirection(context);
            if (vector == null)
                return;

            var target = vector.Sum(context.Actor.Location);
            if (!_grid.ElementAt(target).IsFloor)
            {
                _engine.Push(target, vector);
            }
            if (_grid.ElementAt(target).IsFloor)
            {
                _engine.MoveActor(context.Index, target);
                context.Moved = true;
                context.Resume = false;
            }
            else
            {
                context.Resume = true;
            }
        }

        protected virtual void Command_Unlock(IOopContext context)
        {
            context.Actor.P2 = 0;
        }

        protected void Command_Walk(IOopContext context)
        {
            var vector = GetDirection(context);
            if (vector != null)
            {
                context.Actor.Vector.CopyFrom(vector);
            }
        }

        protected void Command_Zap(IOopContext context)
        {
            _engine.ReadActorCodeWord(context.Index, context);
            while (true)
            {
                context.SearchTarget = _engine.ReadActorCodeWord(context.Index, context);
                var result = _engine.ExecuteLabel(context.Index, context, "\xD\x3A");
                if (!result)
                    break;
                _actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x27;
            }
        }

        protected bool? Condition_Aligned(IOopContext context)
        {
            return context.Actor.Location.X == _actors.Player.Location.X ||
                   context.Actor.Location.Y == _actors.Player.Location.Y;
        }

        protected bool? Condition_Any(IOopContext context)
        {
            var kind = GetKind(context);
            if (kind == null)
                return null;

            return _engine.FindTile(kind, new Location(0, 1));
        }

        protected bool? Condition_Blocked(IOopContext context)
        {
            var direction = GetDirection(context);
            if (direction == null)
                return null;

            return !_grid.ElementAt(context.Actor.Location.Sum(direction)).IsFloor;
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
            return !GetCondition(context);
        }

        protected IXyPair Direction_Ccw(IOopContext context)
        {
            return GetDirection(context).CounterClockwise();
        }

        protected IXyPair Direction_Cw(IOopContext context)
        {
            return GetDirection(context).Clockwise();
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
            return GetDirection(context).Opposite();
        }

        protected IXyPair Direction_Rnd(IOopContext context)
        {
            return _engine.Rnd();
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
            var direction = GetDirection(context);
            return _random.Synced(2) == 0
                ? direction.Clockwise()
                : direction.CounterClockwise();
        }

        protected IXyPair Direction_Seek(IOopContext context)
        {
            return _engine.Seek(context.Actor.Location);
        }

        protected IXyPair Direction_South(IOopContext context)
        {
            return Vector.South;
        }

        protected IXyPair Direction_West(IOopContext context)
        {
            return Vector.West;
        }

        private bool ExecuteTransaction(IOopContext context, bool take)
        {
            // Does the item exist?
            var item = GetItem(context);
            if (item == null)
                return false;

            // Do we have a valid amount?
            var amount = _engine.ReadActorCodeNumber(context.Index, context);
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

        protected virtual IDictionary<string, Action<IEngine>> GetCheats()
        {
            return new Dictionary<string, Action<IEngine>>
            {
                {"AMMO", engine => Cheat_Ammo()},
                {"DARK", engine => Cheat_Dark()},
                {"GEMS", engine => Cheat_Gems()},
                {"HEALTH", engine => Cheat_Health()},
                {"KEYS", engine => Cheat_Keys()},
                {"TIME", engine => Cheat_Time()},
                {"TORCHES", engine => Cheat_Torches()},
                {"ZAP", engine => Cheat_Zap()}
            };
        }

        protected virtual IDictionary<string, Action<IOopContext>> GetCommands()
        {
            return new Dictionary<string, Action<IOopContext>>
            {
                {"BECOME", Command_Become},
                {"BIND", Command_Bind},
                {"CHANGE", Command_Change},
                {"CHAR", Command_Char},
                {"CLEAR", Command_Clear},
                {"CYCLE", Command_Cycle},
                {"DIE", Command_Die},
                {"END", Command_End},
                {"ENDGAME", Command_Endgame},
                {"GIVE", Command_Give},
                {"GO", Command_Go},
                {"IDLE", Command_Idle},
                {"IF", Command_If},
                {"LOCK", Command_Lock},
                {"PLAY", Command_Play},
                {"PUT", Command_Put},
                {"RESTART", Command_Restart},
                {"RESTORE", Command_Restore},
                {"SEND", Command_Send},
                {"SET", Command_Set},
                {"SHOOT", Command_Shoot},
                {"TAKE", Command_Take},
                {"THEN", Command_Then},
                {"THROWSTAR", Command_Throwstar},
                {"TRY", Command_Try},
                {"UNLOCK", Command_Unlock},
                {"WALK", Command_Walk},
                {"ZAP", Command_Zap}
            };
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

        protected virtual IDictionary<string, Func<ISearchContext, bool>> GetTargets()
        {
            return new Dictionary<string, Func<ISearchContext, bool>>
            {
                {"ALL", Target_All},
                {"OTHERS", Target_Others},
                {"SELF", Target_Self}
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

        protected virtual void PutTile(IEngine engine, IXyPair location, IXyPair vector, ITile kind)
        {
            if (location.X >= 1 && location.X <= _grid.Width && location.Y >= 1 &&
                location.Y <= _grid.Height)
            {
                if (!_grid.ElementAt(location).IsFloor)
                {
                    _engine.Push(location, vector);
                }
                engine.PlotTile(location, kind);
            }
        }

        protected bool Target_All(ISearchContext context)
        {
            return context.SearchIndex < _actors.Count;
        }

        private bool Target_Default(ISearchContext context)
        {
            while (context.SearchIndex < _actors.Count)
            {
                if (_actors[context.SearchIndex].Pointer != 0)
                {
                    var instruction = new Executable();
                    var firstByte = _engine.ReadActorCodeByte(context.SearchIndex, instruction);
                    if (firstByte == 0x40)
                    {
                        var name = _engine.ReadActorCodeWord(context.SearchIndex, instruction);
                        if (name == context.SearchTarget)
                        {
                            return true;
                        }
                    }
                }
                context.SearchIndex++;
            }
            return false;
        }

        protected bool Target_Others(ISearchContext context)
        {
            if (context.SearchIndex >= _actors.Count)
                return false;

            if (context.SearchIndex == context.SearchOffset)
                context.SearchIndex++;

            return context.SearchIndex < _actors.Count;
        }

        protected bool Target_Self(ISearchContext context)
        {
            if (context.SearchOffset <= 0)
                return false;

            if (context.SearchIndex > context.SearchOffset)
                return false;

            context.SearchIndex = context.SearchOffset;
            return true;
        }
    }
}