using System;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

public abstract class ElementList(int count) : CachedFixedList<IElement>(count), IElementList
{
    public sealed override int Count { get; } = count;

    public virtual int AmmoId => -1;
    public virtual int BearId => -1;
    public virtual int BlinkRayHId => -1;
    public virtual int BlinkRayVId => -1;
    public virtual int BlinkWallId => -1;
    public virtual int BoardEdgeId => 1;
    public virtual int BombId => -1;
    public virtual int BoulderId => -1;
    public virtual int BreakableId => -1;
    public virtual int BulletId => -1;
    public virtual int ClockwiseId => -1;
    public virtual int CounterId => -1;
    public virtual int DoorId => -1;
    public virtual int DragonPupId => -1;
    public virtual int DuplicatorId => -1;
    public virtual int EmptyId => -1;
    public virtual int EnergizerId => -1;
    public virtual int FakeId => -1;
    public virtual int FloorId => -1;
    public virtual int ForestId => -1;
    public virtual int GemId => -1;
    public virtual int HeadId => -1;
    public virtual int InvisibleId => -1;
    public virtual int KeyId => -1;
    public virtual int LavaId => -1;
    public virtual int LineId => -1;
    public virtual int LionId => -1;
    public virtual int MessengerId => -1;
    public virtual int MonitorId => -1;
    public virtual int NormalId => -1;
    public virtual int ObjectId => -1;
    public virtual int PairerId => -1;
    public virtual int PassageId => -1;
    public virtual int PlayerId => -1;
    public virtual int PusherId => -1;
    public virtual int RicochetId => -1;
    public virtual int RiverEId => -1;
    public virtual int RiverNId => -1;
    public virtual int RiverSId => -1;
    public virtual int RiverWId => -1;
    public virtual int RotonId => -1;
    public virtual int RuffianId => -1;
    public virtual int ScrollId => -1;
    public virtual int SegmentId => -1;
    public virtual int SharkId => -1;
    public virtual int SliderEwId => -1;
    public virtual int SliderNsId => -1;
    public virtual int SlimeId => -1;
    public virtual int SolidId => -1;
    public virtual int SpiderId => -1;
    public virtual int SpinningGunId => -1;
    public virtual int StarId => -1;
    public virtual int StoneId => -1;
    public virtual int TigerId => -1;
    public virtual int TorchId => -1;
    public virtual int TransporterId => -1;
    public virtual int WaterId => -1;
    public virtual int WebId => -1;

    public int IndexOf(ReadOnlySpan<char> name)
    {
        for (var i = 0; i < Count; i++)
            if (GetItem(i).NameMatches(name))
                return i;

        return -1;
    }

    public abstract void Reset();

    public abstract bool IsWater(int id);

    public bool AreAdjacent(int idA, int idB) =>
        idA == idB || idA == BoardEdgeId || idB == BoardEdgeId;

    protected sealed override void SetItem(int index, IElement value) =>
        throw Exceptions.InvalidSet;
}