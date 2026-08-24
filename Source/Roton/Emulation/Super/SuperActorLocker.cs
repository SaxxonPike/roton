using System.Runtime.CompilerServices;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public class SuperActorLocker(
    IActorList actorList)
    : ActorLocker
{
    protected override ref Bool GetLockedRef(int index) =>
        ref Unsafe.As<HWord, Bool>(ref actorList[index].P3);
}