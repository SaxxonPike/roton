﻿using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super)]
    public sealed class SuperElementList : ElementList
    {
        private readonly IMemory _memory;
        private readonly byte[] _data;

        public SuperElementList(IMemory memory, IEngineResourceService engineResourceService)
            : base(80)
        {
            _memory = memory;
            _data = engineResourceService.GetElementData();
            Reset();
        }

        public override void Reset() => _memory.Write(0x7CAA, _data);

        public override int AmmoId => 0x05;

        public override int BearId => 0x22;

        public override int BlinkRayHId => 0x46;

        public override int BlinkRayVId => 0x47;

        public override int BlinkWallId => 0x1D;

        public override int BoardEdgeId => 0x01;

        public override int BombId => 0x0D;

        public override int BoulderId => 0x18;

        public override int BreakableId => 0x17;

        public override int BulletId => 0x45;

        public override int ClockwiseId => 0x10;

        public override int CounterId => 0x11;

        public override int DoorId => 0x09;

        public override int DragonPupId => 0x3C;

        public override int DuplicatorId => 0x0C;

        public override int EmptyId => 0x00;

        public override int EnergizerId => 0x0E;

        public override int FakeId => 0x1B;

        public override int FloorId => 0x2F;

        public override int ForestId => 0x14;

        public override int GemId => 0x07;

        public override int HeadId => 0x2C;

        public override int InvisibleId => 0x1C;

        public override int KeyId => 0x08;

        public override int LavaId => 0x13;

        public override int LineId => 0x1F;

        public override int LionId => 0x29;

        public override int MessengerId => 0x02;

        public override int MonitorId => 0x03;

        public override int NormalId => 0x16;

        public override int ObjectId => 0x24;

        public override int PairerId => 0x3D;

        public override int PassageId => 0x0B;

        public override int PlayerId => 0x04;

        public override int PusherId => 0x28;

        public override int RicochetId => 0x20;

        public override int RiverEId => 0x33;

        public override int RiverNId => 0x30;

        public override int RiverSId => 0x31;

        public override int RiverWId => 0x32;

        public override int RotonId => 0x3B;

        public override int RuffianId => 0x23;

        public override int ScrollId => 0x0A;

        public override int SegmentId => 0x2D;

        public override int SharkId => 0x26;

        public override int SliderEwId => 0x1A;

        public override int SliderNsId => 0x19;

        public override int SlimeId => 0x25;

        public override int SolidId => 0x15;

        public override int SpiderId => 0x3E;

        public override int SpinningGunId => 0x27;

        public override int StarId => 0x48;

        public override int StoneId => 0x40;

        public override int TigerId => 0x2A;

        public override int TransporterId => 0x1E;

        public override int WebId => 0x3F;

        protected override IElement GetElement(int index) => 
            new SuperElement(_memory, index);
    }
}