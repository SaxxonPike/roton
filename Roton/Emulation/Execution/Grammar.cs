using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Execution
{
    internal class Grammar : IGrammar
    {
        private readonly IColorList _colors;
        private readonly IElementList _elements;

        public Grammar(IColorList colors, IElementList elements)
        {
            _colors = colors;
            _elements = elements;
            Cheats = GetCheats();
            Commands = GetCommands();
            Conditions = GetConditions();
            Directions = GetDirections();
            Items = GetItems();
        }

        private IDictionary<string, Action<IEngine>> Cheats { get; }
        private IDictionary<string, Action<IOopContext>> Commands { get; }
        private IDictionary<string, Func<IOopContext, bool?>> Conditions { get; }
        private IDictionary<string, Func<IOopContext, IXyPair>> Directions { get; }
        private IDictionary<string, Func<IOopContext, IOopItem>> Items { get; }

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

        public void Execute(IOopContext oopContext)
        {
            var command = oopContext.ReadNextWord();
            Action<IOopContext> func;
            Commands.TryGetValue(command, out func);
            func?.Invoke(oopContext);
        }

        public bool? GetCondition(IOopContext oopContext)
        {
            var condition = oopContext.ReadNextWord();
            Func<IOopContext, bool?> func;
            Conditions.TryGetValue(condition, out func);
            return func != null
                ? func(oopContext)
                : oopContext.Flags.Contains(condition);
        }

        public IXyPair GetDirection(IOopContext oopContext)
        {
            var direction = oopContext.ReadNextWord();
            Func<IOopContext, IXyPair> func;
            Directions.TryGetValue(direction, out func);
            return func?.Invoke(oopContext);
        }

        public IOopItem GetItem(IOopContext oopContext)
        {
            var item = oopContext.ReadNextWord();
            Func<IOopContext, IOopItem> func;
            Items.TryGetValue(item, out func);
            return func?.Invoke(oopContext);
        }

        public ITile GetKind(IOopContext oopContext)
        {
            var word = oopContext.ReadNextWord();
            var result = new Tile(0, 0);
            var success = false;

            for (var i = 1; i < 8; i++)
            {
                if (_colors[i] != word)
                    continue;

                result.Color = i + 8;
                word = oopContext.ReadNextWord();
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
                context.RaiseError("Bad #BECOME");
                return;
            }

            context.Died = true;
            context.DeathTile.CopyFrom(kind);
        }

        protected void Command_Bind(IOopContext context)
        {
        }

        protected void Command_Change(IOopContext context)
        {
        }

        protected void Command_Char(IOopContext context)
        {
            var value = context.ReadNextNumber();
            if (value >= 0)
            {
                context.Actor.P1 = value;
                context.UpdateBoard(context.Actor.Location);
            }
        }

        protected void Command_Clear(IOopContext context)
        {
            var flag = context.ReadNextWord();
            context.Flags.Remove(flag);
        }

        protected void Command_Cycle(IOopContext context)
        {
            var value = context.ReadNextNumber();
            if (value > 0)
            {
                context.Actor.Cycle = value;
            }
        }

        protected void Command_Die(IOopContext context)
        {
            context.Died = true;
            context.DeathTile.SetTo(context.Elements.EmptyId, 0);
        }

        protected void Command_End(IOopContext context)
        {
            context.OopByte = 0;
            context.Instruction = -1;
        }

        protected void Command_Endgame(IOopContext context)
        {
            context.World.Health = 0;
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
                if (!context.ElementAt(target).IsFloor)
                {
                    context.Push(target, vector);
                }
                if (context.ElementAt(target).IsFloor)
                {
                    context.MoveActor(context.Index, target);
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
        }

        protected void Command_Put(IOopContext context)
        {
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
        }

        protected void Command_Set(IOopContext context)
        {
            var flag = context.ReadNextWord();
            context.Flags.Add(flag);
        }

        protected void Command_Shoot(IOopContext context)
        {
        }

        protected void Command_Take(IOopContext context)
        {
            context.Repeat = ExecuteTransaction(context, true);
        }

        protected void Command_Then(IOopContext context)
        {
            // The actual code doesn't work this way.
            // TODO: Actually implement it without this hack.
            context.NextLine = false;
            context.CommandsExecuted--;
        }

        protected void Command_Throwstar(IOopContext context)
        {
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
            return context.Actor.Location.X == context.Player.Location.X ||
                   context.Actor.Location.Y == context.Player.Location.Y;
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
            return context.GetRandomDirection();
        }

        protected IXyPair Direction_RndNe(IOopContext context)
        {
            return context.GetRandomNumber(2) == 0
                ? Vector.North
                : Vector.East;
        }

        protected IXyPair Direction_RndNs(IOopContext context)
        {
            return context.GetRandomNumber(2) == 0
                ? Vector.North
                : Vector.South;
        }

        protected IXyPair Direction_RndP(IOopContext context)
        {
            return context.GetRandomNumber(2) == 0
                ? GetDirection(context).Clockwise()
                : GetDirection(context).CounterClockwise();
        }

        protected IXyPair Direction_Seek(IOopContext context)
        {
            return context.GetSeek(context.Actor.Location);
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
            var amount = context.ReadNextNumber();
            if (amount <= 0)
                return true;

            // Modify value if we are taking.
            if (take)
                context.OopNumber = -context.OopNumber;

            // Determine if the result will be in range.
            var pendingAmount = item.Value + context.OopNumber;
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

        protected IOopItem Item_Ammo(IOopContext context)
        {
            return new OopItem(
                () => context.World.Ammo,
                v => context.World.Ammo = v);
        }

        protected IOopItem Item_Gems(IOopContext context)
        {
            return new OopItem(
                () => context.World.Gems,
                v => context.World.Gems = v);
        }

        protected IOopItem Item_Health(IOopContext context)
        {
            return new OopItem(
                () => context.World.Health,
                v => context.World.Health = v);
        }

        protected IOopItem Item_Score(IOopContext context)
        {
            return new OopItem(
                () => context.World.Score,
                v => context.World.Score = v);
        }

        protected IOopItem Item_Stones(IOopContext context)
        {
            return new OopItem(
                () => context.World.Stones,
                v => context.World.Stones = v);
        }

        protected IOopItem Item_Time(IOopContext context)
        {
            return new OopItem(
                () => context.World.TimePassed,
                v => context.World.TimePassed = v);
        }

        protected IOopItem Item_Torches(IOopContext context)
        {
            return new OopItem(
                () => context.World.Torches,
                v => context.World.Torches = v);
        }
    }
}