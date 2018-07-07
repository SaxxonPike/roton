using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.Execution
{
    public class Broadcaster : IBroadcaster
    {
        private readonly IActors _actors;
        private readonly IParser _parser;
        private readonly ILocker _locker;

        public Broadcaster(IActors actors, IParser parser, ILocker locker)
        {
            _actors = actors;
            _parser = parser;
            _locker = locker;
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
                if (!_locker.IsLocked(info.SearchIndex) || force || (sender == info.SearchIndex && !external))
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
        
    }
}