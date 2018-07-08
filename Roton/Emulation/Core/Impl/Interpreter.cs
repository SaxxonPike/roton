using System.Linq;
using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Execution
{
    public class Interpreter : IInterpreter
    {
        private readonly IEngine _engine;

        public Interpreter(IEngine engine)
        {
            _engine = engine;
        }

        public void Cheat(string input)
        {
            if (input == null)
                return;

            _engine.Cheats.Get(input.ToUpperInvariant())?.Execute();

            if (input.Length < 2)
                return;

            if (input.StartsWith("+"))
                _engine.Flags.Add(input.Substring(1));
            else if (input.StartsWith("-"))
                _engine.Flags.Remove(input.Substring(1));
        }

        public void Execute(IOopContext context)
        {
            while (true)
            {
                context.Resume = false;
                context.Executed = true;

                var name = _engine.Parser.ReadWord(context.Index, context);
                if (name.Length == 0)
                    break;

                var command = _engine.Commands.Get(name);

                if (command != null)
                {
                    command.Execute(context);
                }
                else
                {
                    if (!_engine.BroadcastLabel(context.Index, name, false))
                    {
                        if (!name.Contains(':'))
                        {
                            _engine.RaiseError($"Bad command {name}");
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
                        _engine.Parser.ReadLine(context.Index, context);
                    }
                    break;
                }
            }
        }

//
//        protected virtual IDictionary<string, Func<IOopContext, bool?>> GetConditions()
//        {
//            return new Dictionary<string, Func<IOopContext, bool?>>
//            {
//                {"ALLIGNED", Condition_Aligned},
//                {"ANY", Condition_Any},
//                {"BLOCKED", Condition_Blocked},
//                {"CONTACT", Condition_Contact},
//                {"ENERGIZED", Condition_Energized},
//                {"NOT", Condition_Not}
//            };
//        }
//
//        protected virtual IDictionary<string, Func<IOopContext, IXyPair>> GetDirections()
//        {
//            return new Dictionary<string, Func<IOopContext, IXyPair>>
//            {
//                {"CW", Direction_Cw},
//                {"CCW", Direction_Ccw},
//                {"E", Direction_East},
//                {"EAST", Direction_East},
//                {"FLOW", Direction_Flow},
//                {"I", Direction_Idle},
//                {"IDLE", Direction_Idle},
//                {"N", Direction_North},
//                {"NORTH", Direction_North},
//                {"OPP", Direction_Opp},
//                {"RND", Direction_Rnd},
//                {"RNDNE", Direction_RndNe},
//                {"RNDNS", Direction_RndNs},
//                {"RNDP", Direction_RndP},
//                {"S", Direction_South},
//                {"SOUTH", Direction_South},
//                {"SEEK", Direction_Seek},
//                {"W", Direction_West},
//                {"WEST", Direction_West}
//            };
//        }
//
//        protected virtual IDictionary<string, Func<IOopContext, IOopItem>> GetItems()
//        {
//            return new Dictionary<string, Func<IOopContext, IOopItem>>
//            {
//                {"AMMO", Item_Ammo},
//                {"GEMS", Item_Gems},
//                {"HEALTH", Item_Health},
//                {"SCORE", Item_Score},
//                {"TIME", Item_Time},
//                {"TORCHES", Item_Torches}
//            };
//        }
    }
}