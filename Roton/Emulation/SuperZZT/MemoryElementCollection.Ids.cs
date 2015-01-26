using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal partial class MemoryElementCollection : MemoryElementCollectionBase
    {
        protected override Type ElementType
        {
            get { return typeof(MemoryElement); }
        }

        public override int EmptyId
        {
            get
            {
                return 0x00;
            }
        }

        public override int BoardEdgeId
        {
            get
            {
                return 0x01;
            }
        }

        public override int MessengerId
        {
            get
            {
                return 0x02;
            }
        }

        public override int MonitorId
        {
            get
            {
                return 0x03;
            }
        }

        public override int PlayerId
        {
            get
            {
                return 0x04;
            }
        }

        public override int AmmoId
        {
            get
            {
                return 0x05;
            }
        }

        public override int GemId
        {
            get
            {
                return 0x07;
            }
        }

        public override int KeyId
        {
            get
            {
                return 0x08;
            }
        }

        public override int DoorId
        {
            get
            {
                return 0x09;
            }
        }

        public override int ScrollId
        {
            get
            {
                return 0x0A;
            }
        }

        public override int PassageId
        {
            get
            {
                return 0x0B;
            }
        }

        public override int DuplicatorId
        {
            get
            {
                return 0x0C;
            }
        }

        public override int BombId
        {
            get
            {
                return 0x0D;
            }
        }

        public override int EnergizerId
        {
            get
            {
                return 0x0E;
            }
        }

        public override int ClockwiseId
        {
            get
            {
                return 0x10;
            }
        }

        public override int CounterId
        {
            get
            {
                return 0x11;
            }
        }

        public override int LavaId
        {
            get
            {
                return 0x13;
            }
        }

        public override int ForestId
        {
            get
            {
                return 0x14;
            }
        }

        public override int SolidId
        {
            get
            {
                return 0x15;
            }
        }

        public override int NormalId
        {
            get
            {
                return 0x16;
            }
        }

        public override int BreakableId
        {
            get
            {
                return 0x17;
            }
        }

        public override int BoulderId
        {
            get
            {
                return 0x18;
            }
        }

        public override int SliderNSId
        {
            get
            {
                return 0x19;
            }
        }

        public override int SliderEWId
        {
            get
            {
                return 0x1A;
            }
        }

        public override int FakeId
        {
            get
            {
                return 0x1B;
            }
        }

        public override int InvisibleId
        {
            get
            {
                return 0x1C;
            }
        }

        public override int BlinkWallId
        {
            get
            {
                return 0x1D;
            }
        }

        public override int TransporterId
        {
            get
            {
                return 0x1E;
            }
        }

        public override int LineId
        {
            get
            {
                return 0x1F;
            }
        }

        public override int RicochetId
        {
            get
            {
                return 0x20;
            }
        }

        public override int BearId
        {
            get
            {
                return 0x22;
            }
        }

        public override int RuffianId
        {
            get
            {
                return 0x23;
            }
        }

        public override int ObjectId
        {
            get
            {
                return 0x24;
            }
        }

        public override int SlimeId
        {
            get
            {
                return 0x25;
            }
        }

        public override int SharkId
        {
            get
            {
                return 0x26;
            }
        }

        public override int SpinningGunId
        {
            get
            {
                return 0x27;
            }
        }

        public override int PusherId
        {
            get
            {
                return 0x28;
            }
        }

        public override int LionId
        {
            get
            {
                return 0x29;
            }
        }

        public override int TigerId
        {
            get
            {
                return 0x2A;
            }
        }

        public override int HeadId
        {
            get
            {
                return 0x2C;
            }
        }

        public override int SegmentId
        {
            get
            {
                return 0x2D;
            }
        }

        public override int FloorId
        {
            get
            {
                return 0x2F;
            }
        }

        public override int RiverNId
        {
            get
            {
                return 0x30;
            }
        }

        public override int RiverSId
        {
            get
            {
                return 0x31;
            }
        }

        public override int RiverWId
        {
            get
            {
                return 0x32;
            }
        }

        public override int RiverEId
        {
            get
            {
                return 0x33;
            }
        }

        public override int RotonId
        {
            get
            {
                return 0x3B;
            }
        }

        public override int DragonPupId
        {
            get
            {
                return 0x3C;
            }
        }

        public override int PairerId
        {
            get
            {
                return 0x3D;
            }
        }

        public override int SpiderId
        {
            get
            {
                return 0x3E;
            }
        }

        public override int WebId
        {
            get
            {
                return 0x3F;
            }
        }

        public override int StoneId
        {
            get
            {
                return 0x40;
            }
        }

        public override int BulletId
        {
            get
            {
                return 0x45;
            }
        }

        public override int BlinkRayHId
        {
            get
            {
                return 0x46;
            }
        }

        public override int BlinkRayVId
        {
            get
            {
                return 0x47;
            }
        }

        public override int StarId
        {
            get
            {
                return 0x48;
            }
        }
    }
}
