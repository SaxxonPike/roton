using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Emulation.Execution
{
    internal abstract class Grammar : IGrammar
    {
        private readonly IColorList _colors;
        private readonly IElementList _elements;

        protected Grammar(IColorList colors, IElementList elements)
        {
            _colors = colors;
            _elements = elements;
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

        public void Cheat(IEngine engine, string input)
        {
            if (input == null)
                return;

            Action<IEngine> func;
            Cheats.TryGetValue(input.ToUpperInvariant(), out func);
            func?.Invoke(engine);

            if (input.Length < 2)
                return;

            if (input.StartsWith("+"))
                engine.World.Flags.Add(input.Substring(1));
            else if (input.StartsWith("-"))
                engine.World.Flags.Remove(input.Substring(1));
        }

        public void Execute(IOopContext context)
        {
            while (true)
            {
                context.Resume = false;
                context.Executed = true;

                var command = context.ReadWord();
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
                    if (!context.Engine.BroadcastLabel(context.Index, command, false))
                    {
                        if (!command.Contains(':'))
                        {
                            context.Engine.RaiseError($"Bad command {command}");
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
                        context.ReadLine();
                    }
                    break;
                }
            }
        }

        public bool? GetCondition(IOopContext oopContext)
        {
            var condition = oopContext.ReadWord();
            Func<IOopContext, bool?> func;
            Conditions.TryGetValue(condition, out func);
            return func != null
                ? func(oopContext)
                : oopContext.GetFlags().Contains(condition);
        }

        public IXyPair GetDirection(IOopContext oopContext)
        {
            var direction = oopContext.ReadWord();
            Func<IOopContext, IXyPair> func;
            Directions.TryGetValue(direction, out func);
            return func?.Invoke(oopContext);
        }

        public IOopItem GetItem(IOopContext oopContext)
        {
            var item = oopContext.ReadWord();
            Func<IOopContext, IOopItem> func;
            Items.TryGetValue(item, out func);
            return func?.Invoke(oopContext);
        }

        public ITile GetKind(IOopContext oopContext)
        {
            var word = oopContext.ReadWord();
            var result = new Tile(0, 0);
            var success = false;

            for (var i = 1; i < 8; i++)
            {
                if (_colors[i].ToUpperInvariant() != word)
                    continue;

                result.Color = i + 8;
                word = oopContext.ReadWord();
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

        protected void Cheat_Ammo(IEngine engine)
        {
            engine.World.Ammo += 5;
        }

        protected void Cheat_Dark(IEngine engine)
        {
            engine.Board.IsDark = true;
            engine.RedrawBoard();
        }

        protected void Cheat_Gems(IEngine engine)
        {
            engine.World.Gems += 5;
        }

        protected void Cheat_Health(IEngine engine)
        {
            engine.World.Health += 50;
        }

        protected void Cheat_Keys(IEngine engine)
        {
            for (var i = 1; i < 8; i++)
            {
                engine.World.Keys[i] = true;
            }
        }

        protected void Cheat_Time(IEngine engine)
        {
            engine.World.TimePassed -= 30;
        }

        protected void Cheat_Torches(IEngine engine)
        {
            engine.World.Torches += 3;
        }

        protected void Cheat_Zap(IEngine engine)
        {
            for (var i = 0; i < 4; i++)
            {
                engine.Destroy(engine.Player.Location.Sum(engine.GetCardinalVector(i)));
            }
        }

        protected void Command_Become(IOopContext context)
        {
            var kind = GetKind(context);
            if (kind == null)
            {
                context.Engine.RaiseError("Bad #BECOME");
                return;
            }

            context.Died = true;
            context.DeathTile.CopyFrom(kind);
        }

        protected void Command_Bind(IOopContext context)
        {
            var search = new SearchContext(context.Engine);
            var target = context.ReadWord();
            search.SearchTarget = target;
            if (GetTarget(search))
            {
                var targetActor = context.GetActor(search.SearchIndex);
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
                    var targetElement = context.GetElement(target);
                    success = true;
                    if (target.Color == 0 && targetElement.Color < 0xF0)
                    {
                        target.Color = targetElement.Color;
                    }
                    var location = new Location();
                    while (context.Engine.FindTile(source, location))
                    {
                        context.Engine.PlotTile(location, target);
                    }
                }
            }

            if (!success)
            {
                context.Engine.RaiseError("Bad #CHANGE");
            }
        }

        protected void Command_Char(IOopContext context)
        {
            var value = context.ReadNumber();
            if (value >= 0)
            {
                context.Actor.P1 = value;
                context.Engine.UpdateBoard(context.Actor.Location);
            }
        }

        protected void Command_Clear(IOopContext context)
        {
            var flag = context.ReadWord();
            context.GetFlags().Remove(flag);
        }

        protected void Command_Cycle(IOopContext context)
        {
            var value = context.ReadNumber();
            if (value > 0)
            {
                context.Actor.Cycle = value;
            }
        }

        protected void Command_Die(IOopContext context)
        {
            context.Died = true;
            context.DeathTile.SetTo(context.Engine.Elements.EmptyId, 0);
        }

        protected void Command_End(IOopContext context)
        {
            context.SetByte(0);
            context.Instruction = -1;
        }

        protected void Command_Endgame(IOopContext context)
        {
            context.GetWorld().Health = 0;
        }

        protected void Command_Give(IOopContext context)
        {
            context.Repeat = ExecuteTransaction(context, false);
        }

        protected void Command_Go(IOopContext context)
        {
            var vector = GetDirection(context);
            if (vector != null)
            {
                var target = context.Actor.Location.Sum(vector);
                if (!context.Engine.ElementAt(target).IsFloor)
                {
                    context.Engine.Push(target, vector);
                }
                if (context.Engine.ElementAt(target).IsFloor)
                {
                    context.Engine.MoveActor(context.Index, target);
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
                context.Repeat = condition.Value;
            }
        }

        protected virtual void Command_Lock(IOopContext context)
        {
            context.Actor.P2 = 1;
        }

        protected void Command_Play(IOopContext context)
        {
            var notes = context.ReadLine();
            var sound = context.Engine.EncodeMusic(notes);
            context.Engine.PlaySound(-1, sound);
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
                    PutTile(context.Engine, context.Actor.Location.Sum(vector), vector, kind);
                }
            }

            if (!success)
                context.Engine.RaiseError("Bad #PUT");
        }

        protected void Command_Restart(IOopContext context)
        {
            context.Instruction = 0;
            context.NextLine = false;
        }

        protected void Command_Restore(IOopContext context)
        {
        }

        protected void Command_Send(IOopContext context)
        {
            var target = context.ReadWord();
            context.NextLine = context.Engine.BroadcastLabel(context.Index, target, false);
        }

        protected void Command_Set(IOopContext context)
        {
            var flag = context.ReadWord();
            context.GetFlags().Add(flag);
        }

        protected void Command_Shoot(IOopContext context)
        {
            var vector = GetDirection(context);
            if (vector != null)
            {
                var projectile = context.Engine.Elements.Bullet();
                var success = context.Engine.SpawnProjectile(projectile.Id, context.Actor.Location, vector, true);
                if (success)
                {
                    context.Engine.PlaySound(2, context.Engine.SoundSet.EnemyShoot);
                }
                context.Moved = true;
            }
        }

        protected void Command_Take(IOopContext context)
        {
            context.Repeat = ExecuteTransaction(context, true);
        }

        protected void Command_Then(IOopContext context)
        {
            // The actual code doesn't work this way.
            // We cheat a little by not advancing the execution counter.
            context.NextLine = false;
            context.CommandsExecuted--;
        }

        protected void Command_Throwstar(IOopContext context)
        {
            var vector = GetDirection(context);
            if (vector != null)
            {
                var projectile = context.Engine.Elements.Star();
                context.Engine.SpawnProjectile(projectile.Id, context.Actor.Location, vector, true);
            }
            context.Moved = true;
        }

        protected void Command_Try(IOopContext context)
        {
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
        }

        protected bool? Condition_Alligned(IOopContext context)
        {
            return context.Actor.Location.X == context.Engine.Player.Location.X ||
                   context.Actor.Location.Y == context.Engine.Player.Location.Y;
        }

        protected bool? Condition_Any(IOopContext context)
        {
            var kind = GetKind(context);
            return false;
        }

        protected bool? Condition_Blocked(IOopContext context)
        {
            var direction = GetDirection(context);
            return false;
        }

        protected bool? Condition_Contact(IOopContext context)
        {
            return false;
        }

        protected bool? Condition_Energized(IOopContext context)
        {
            return false;
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
            return context.Engine.Rnd();
        }

        protected IXyPair Direction_RndNe(IOopContext context)
        {
            return context.Engine.SyncRandomNumber(2) == 0
                ? Vector.North
                : Vector.East;
        }

        protected IXyPair Direction_RndNs(IOopContext context)
        {
            return context.Engine.SyncRandomNumber(2) == 0
                ? Vector.North
                : Vector.South;
        }

        protected IXyPair Direction_RndP(IOopContext context)
        {
            return context.Engine.SyncRandomNumber(2) == 0
                ? GetDirection(context).Clockwise()
                : GetDirection(context).CounterClockwise();
        }

        protected IXyPair Direction_Seek(IOopContext context)
        {
            return context.Engine.Seek(context.Actor.Location);
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
                return true;

            // Do we have a valid amount?
            var amount = context.ReadNumber();
            if (amount <= 0)
                return true;

            // Modify value if we are taking.
            if (take)
                context.SetNumber(-context.GetNumber());

            // Determine if the result will be in range.
            var pendingAmount = item.Value + context.GetNumber();
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
                {"AMMO", Cheat_Ammo},
                {"DARK", Cheat_Dark},
                {"GEMS", Cheat_Gems},
                {"HEALTH", Cheat_Health},
                {"KEYS", Cheat_Keys},
                {"TIME", Cheat_Time},
                {"TORCHES", Cheat_Torches},
                {"ZAP", Cheat_Zap}
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
                {"ALLIGNED", Condition_Alligned},
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
                () => context.GetWorld().Ammo,
                v => context.GetWorld().Ammo = v);
        }

        protected IOopItem Item_Gems(IOopContext context)
        {
            return new OopItem(
                () => context.GetWorld().Gems,
                v => context.GetWorld().Gems = v);
        }

        protected IOopItem Item_Health(IOopContext context)
        {
            return new OopItem(
                () => context.GetWorld().Health,
                v => context.GetWorld().Health = v);
        }

        protected IOopItem Item_Score(IOopContext context)
        {
            return new OopItem(
                () => context.GetWorld().Score,
                v => context.GetWorld().Score = v);
        }

        protected IOopItem Item_Stones(IOopContext context)
        {
            return new OopItem(
                () => context.GetWorld().Stones,
                v => context.GetWorld().Stones = v);
        }

        protected IOopItem Item_Time(IOopContext context)
        {
            return new OopItem(
                () => context.GetWorld().TimePassed,
                v => context.GetWorld().TimePassed = v);
        }

        protected IOopItem Item_Torches(IOopContext context)
        {
            return new OopItem(
                () => context.GetWorld().Torches,
                v => context.GetWorld().Torches = v);
        }

        protected virtual void PutTile(IEngine engine, IXyPair location, IXyPair vector, ITile kind)
        {
            if (location.X >= 1 && location.X <= engine.Tiles.Width && location.Y >= 1 &&
                location.Y <= engine.Tiles.Height)
            {
                if (!engine.ElementAt(location).IsFloor)
                {
                    engine.Push(location, vector);
                }
                engine.PlotTile(location, kind);
            }
        }

        protected bool Target_All(ISearchContext context)
        {
            return context.SearchIndex < context.Engine.Actors.Count;
        }

        private bool Target_Default(ISearchContext context)
        {
            while (context.SearchIndex < context.Engine.Actors.Count)
            {
                if (context.Engine.Actors[context.SearchIndex].Pointer != 0)
                {
                    var instruction = new Executable();
                    var firstByte = context.Engine.ReadActorCodeByte(context.SearchIndex, instruction);
                    if (firstByte == 0x40)
                    {
                        var name = context.Engine.ReadActorCodeWord(context.SearchIndex, instruction);
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
            if (context.SearchIndex >= context.Engine.Actors.Count)
                return false;

            if (context.SearchIndex == context.SearchOffset)
                context.SearchIndex++;

            return context.SearchIndex < context.Engine.Actors.Count;
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