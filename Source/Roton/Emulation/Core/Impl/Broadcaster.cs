using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class Broadcaster(
    ITracer tracer,
    IActorList actorList,
    IParser parser,
    IFacts facts,
    IActorLocker actorLocker,
    IActorNotifier actorNotifier
    ) : IBroadcaster
{
    public bool BroadcastLabel(int sender, ReadOnlySpan<char> label, bool ignoreLock)
    {
        var ignoreSelfLock = false;
        var success = false;

        if (sender < 0)
        {
            ignoreSelfLock = true;
            sender = -sender;
        }

        var info = new SearchContext
        {
            Index = 0,
            Offset = 0
        };

        while (ExecuteLabel(sender, ref info, label, "\r:"))
        {
            if (!actorLocker.IsActorLocked(info.Index) || ignoreLock || sender == info.Index && !ignoreSelfLock)
            {
                if (sender == info.Index)
                    success = true;

                tracer.TraceBroadcast(sender, label, info.Index, ignoreLock, ignoreSelfLock);
                actorList[info.Index].Instruction = info.Offset;
                actorNotifier.NotifyActorSentLabel(info.Index);
            }
        }

        return success;
    }

    public bool ExecuteLabel(int sender, ref SearchContext search, ReadOnlySpan<char> term, ReadOnlySpan<char> prefix)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var label = term;
        var success = false;
        var split = label.IndexOf(':');
        ReadOnlySpan<char> target = null;

        if (split > 0)
        {
            target = label.Slice(0, split);
            label = label.Slice(split + 1);
            success = parser.TryEvalTarget(sender, ref search, target);
        }
        else if (search.Index < sender)
        {
            label = term;
            search.Index = sender;
            split = 0;
            success = true;
        }

        while (success)
        {
            if (label.Equals(facts.RestartLabel, StringComparison.OrdinalIgnoreCase))
            {
                search.Offset = 0;
            }
            else
            {
                prefix.CopyTo(buffer);
                label.CopyTo(buffer.Slice(prefix.Length));
                search.Offset = parser.Search(search.Index, buffer.Slice(0, prefix.Length + label.Length));
                if (search.Offset < 0 && split > 0)
                {
                    success = parser.TryEvalTarget(sender, ref search, target);
                    continue;
                }
            }

            success = search.Offset >= 0;
            break;
        }

        return success;
    }


}