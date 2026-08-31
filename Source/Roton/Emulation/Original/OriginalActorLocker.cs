using System.Runtime.CompilerServices;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public class OriginalActorLocker(
    IActorList actors)
    : ActorLocker
{
    protected override ref Bool GetLockedRef(int index) =>
        ref Unsafe.As<HWord, Bool>(ref actors[index].P2);
}