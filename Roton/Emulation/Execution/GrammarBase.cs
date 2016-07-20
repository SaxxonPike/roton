using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Execution
{
    public abstract class GrammarBase : IGrammar
    {
        private readonly IColorList _colors;
        private readonly IElementList _elements;

        public GrammarBase(IColorList colors, IElementList elements)
        {
            _colors = colors;
            _elements = elements;
            Cheats = GetCheats();
            Commands = GetCommands();
            Conditions = GetConditions();
            Directions = GetDirections();
            Items = GetItems();
        }

        private IDictionary<string, Action<ICore>> Cheats { get; }
        private IDictionary<string, Action<IOopContext>> Commands { get; }
        private IDictionary<string, Func<IOopContext, bool?>> Conditions { get; }
        private IDictionary<string, Func<IOopContext, IXyPair>> Directions { get; }
        private IDictionary<string, Func<IOopContext, IOopItem>> Items { get; }

        public void Cheat(ICore core, string input)
        {
            if (input == null)
                return;

            Action<ICore> func;
            Cheats.TryGetValue(input.ToUpperInvariant(), out func);
            func?.Invoke(core);

            if (input.Length < 2)
                return;

            if (input.StartsWith("+"))
                core.Flags.Add(input.Substring(1));
            else if (input.StartsWith("-"))
                core.Flags.Remove(input.Substring(1));
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

        protected abstract IDictionary<string, Action<ICore>> GetCheats();
        protected abstract IDictionary<string, Action<IOopContext>> GetCommands();
        protected abstract IDictionary<string, Func<IOopContext, bool?>> GetConditions();
        protected abstract IDictionary<string, Func<IOopContext, IXyPair>> GetDirections();
        protected abstract IDictionary<string, Func<IOopContext, IOopItem>> GetItems();

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

        protected IXyPair Direction_Cw(IOopContext context)
        {
            return GetDirection(context).Clockwise();
        }

        protected IXyPair Direction_Ccw(IOopContext context)
        {
            return GetDirection(context).CounterClockwise();
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

        protected IXyPair Direction_South(IOopContext context)
        {
            return Vector.South;
        }

        protected IXyPair Direction_Seek(IOopContext context)
        {
            return context.GetSeek(context.Actor.Location);
        }

        protected IXyPair Direction_West(IOopContext context)
        {
            return Vector.West;
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

        protected virtual void Cheat_Ammo(ICore core)
        {
            core.WorldData.Ammo += 5;
        }

        protected void Cheat_Dark(ICore core)
        {
            core.BoardData.Dark = true;
            core.RedrawBoard();
        }

        protected void Cheat_Gems(ICore core)
        {
            core.WorldData.Gems += 5;
        }

        protected void Cheat_Health(ICore core)
        {
            core.WorldData.Health += 50;
        }

        protected void Cheat_Keys(ICore core)
        {
            for (var i = 1; i < 8; i++)
            {
                core.Keys[i] = true;
            }
        }

        protected void Cheat_Time(ICore core)
        {
            core.WorldData.TimePassed -= 30;
        }

        protected void Cheat_Torches(ICore core)
        {
            core.WorldData.Torches += 3;
        }

        protected void Cheat_Zap(ICore core)
        {
            for (var i = 0; i < 4; i++)
            {
                core.Destroy(core.Player.Location.Sum(core.GetCardinalVector(i)));
            }
        }
    }
}
