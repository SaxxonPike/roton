using System.Runtime.CompilerServices;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalActorLocker(
    IActorList actors)
    : IActorLocker
{
    private ref Bool GetLockedRef(int index) =>
        ref Unsafe.As<HWord, Bool>(ref actors[index].P2);

    public void Lock(int index) =>
        GetLockedRef(index) = true;

    public void Unlock(int index) =>
        GetLockedRef(index) = false;

    public bool IsLocked(int index) =>
        GetLockedRef(index);
}