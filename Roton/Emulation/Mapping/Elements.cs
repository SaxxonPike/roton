using System.Collections.Generic;
using Roton.Core;
using Roton.Core.Collections;

namespace Roton.Emulation.Mapping
{
    public abstract class Elements : FixedList<IElement>, IElements
    {
        protected Elements(IMemory memory, int count)
        {
            Count = count;
            Memory = memory;
            Cache = new Dictionary<int, IElement>();
        }

        private IDictionary<int, IElement> Cache { get; }

        protected IMemory Memory { get; }

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
        public sealed override int Count { get; }
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

        protected abstract IElement GetElement(int index);

        protected sealed override IElement GetItem(int index)
        {
            IElement element;
            Cache.TryGetValue(index, out element);
            if (element != null)
                return element;

            element = GetElement(index);
            Cache[index] = element;
            return element;
        }

        protected sealed override void SetItem(int index, IElement value)
        {
            throw Exceptions.InvalidSet;
        }
    }
}