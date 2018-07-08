using System;
using Roton.Core;
using Roton.Emulation.Behaviors;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztElements : Elements
    {
        private readonly Lazy<IBehaviorMap> _behaviorMap;

        public ZztElements(IMemory memory, Lazy<IBehaviorMap> behaviorMap, IEngineResourceProvider engineResourceProvider)
            : base(memory, 54)
        {
            _behaviorMap = behaviorMap;
            memory.Write(0x4AD4, engineResourceProvider.GetElementData());
        }

        public override int AmmoId => 0x05;
        public override int BearId => 0x22;
        public override int BlinkRayHId => 0x21;
        public override int BlinkRayVId => 0x2B;
        public override int BlinkWallId => 0x1D;
        public override int BoardEdgeId => 0x01;
        public override int BombId => 0x0D;
        public override int BoulderId => 0x18;
        public override int BreakableId => 0x17;
        public override int BulletId => 0x12;
        public override int ClockwiseId => 0x10;
        public override int CounterId => 0x11;
        public override int DoorId => 0x09;
        public override int DuplicatorId => 0x0C;

        public override int EmptyId => 0x00;
        public override int EnergizerId => 0x0E;
        public override int FakeId => 0x1B;
        public override int ForestId => 0x14;
        public override int GemId => 0x07;
        public override int HeadId => 0x2C;
        public override int InvisibleId => 0x1C;
        public override int KeyId => 0x08;
        public override int LineId => 0x1F;
        public override int LionId => 0x29;
        public override int MessengerId => 0x02;
        public override int MonitorId => 0x03;
        public override int NormalId => 0x16;
        public override int ObjectId => 0x24;
        public override int PassageId => 0x0B;
        public override int PlayerId => 0x04;
        public override int PusherId => 0x28;
        public override int RicochetId => 0x20;
        public override int RuffianId => 0x23;
        public override int ScrollId => 0x0A;
        public override int SegmentId => 0x2D;
        public override int SharkId => 0x26;
        public override int SliderEwId => 0x1A;
        public override int SliderNsId => 0x19;
        public override int SlimeId => 0x25;
        public override int SolidId => 0x15;
        public override int SpinningGunId => 0x27;
        public override int StarId => 0x0F;
        public override int TigerId => 0x2A;
        public override int TorchId => 0x06;
        public override int TransporterId => 0x1E;
        public override int WaterId => 0x13;

        protected override IElement GetElement(int index)
        {
            return new ZztElement(Memory, index, _behaviorMap.Value.Map(index));
        }
    }
}