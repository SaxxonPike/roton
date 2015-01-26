using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal partial class MemoryElementCollectionBase : FixedList<Element>
    {
        public MemoryElementCollectionBase(Memory memory)
        {
            this.Memory = memory;
            this.Cache = new MemoryElementBase[Count];

            for (int i = 0; i < Count; i++)
            {
                MemoryElementBase element = GetElement(i);
                if (i == AmmoId) { element.KnownName = "Ammo"; }
                else if (i == BearId) { element.KnownName = "Bear"; }
                else if (i == BlinkRayHId) { element.KnownName = "Blink Ray (H)"; }
                else if (i == BlinkRayVId) { element.KnownName = "Blink Ray (V)"; }
                else if (i == BlinkWallId) { element.KnownName = "Blink Wall"; }
                else if (i == BoardEdgeId) { element.KnownName = "Board Edge"; }
                else if (i == BombId) { element.KnownName = "Bomb"; }
                else if (i == BoulderId) { element.KnownName = "Boulder"; }
                else if (i == BreakableId) { element.KnownName = "Breakable Wall"; }
                else if (i == BulletId) { element.KnownName = "Bullet"; }
                else if (i == ClockwiseId) { element.KnownName = "Conveyor (Clockwise)"; }
                else if (i == CounterId) { element.KnownName = "Conveyor (Counter-Clockwise)"; }
                else if (i == DoorId) { element.KnownName = "Door"; }
                else if (i == DragonPupId) { element.KnownName = "Dragon Pup"; }
                else if (i == DuplicatorId) { element.KnownName = "Duplicator"; }
                else if (i == EmptyId) { element.KnownName = "Empty"; }
                else if (i == EnergizerId) { element.KnownName = "Energizer"; }
                else if (i == FakeId) { element.KnownName = "Fake Wall"; }
                else if (i == FloorId) { element.KnownName = "Floor"; }
                else if (i == ForestId) { element.KnownName = "Forest"; }
                else if (i == GemId) { element.KnownName = "Gem"; }
                else if (i == HeadId) { element.KnownName = "Centipede Head"; }
                else if (i == InvisibleId) { element.KnownName = "Invisible Wall"; }
                else if (i == KeyId) { element.KnownName = "Key"; }
                else if (i == LavaId) { element.KnownName = "Lava"; }
                else if (i == LineId) { element.KnownName = "Line"; }
                else if (i == LionId) { element.KnownName = "Lion"; }
                else if (i == MessengerId) { element.KnownName = "Messenger"; }
                else if (i == MonitorId) { element.KnownName = "Monitor"; }
                else if (i == NormalId) { element.KnownName = "Normal"; }
                else if (i == ObjectId) { element.KnownName = "Object"; }
                else if (i == PairerId) { element.KnownName = "Pairer"; }
                else if (i == PassageId) { element.KnownName = "Passage"; }
                else if (i == PlayerId) { element.KnownName = "Player"; }
                else if (i == PusherId) { element.KnownName = "Pusher"; }
                else if (i == RicochetId) { element.KnownName = "Ricochet"; }
                else if (i == RiverEId) { element.KnownName = "River (E)"; }
                else if (i == RiverNId) { element.KnownName = "River (N)"; }
                else if (i == RiverSId) { element.KnownName = "River (S)"; }
                else if (i == RiverWId) { element.KnownName = "River (W)"; }
                else if (i == RotonId) { element.KnownName = "Roton"; }
                else if (i == RuffianId) { element.KnownName = "Ruffian"; }
                else if (i == ScrollId) { element.KnownName = "Scroll"; }
                else if (i == SegmentId) { element.KnownName = "Centipede Segment"; }
                else if (i == SharkId) { element.KnownName = "Shark"; }
                else if (i == SliderEWId) { element.KnownName = "Slider (EW)"; }
                else if (i == SliderNSId) { element.KnownName = "Slider (NS)"; }
                else if (i == SlimeId) { element.KnownName = "Slime"; }
                else if (i == SolidId) { element.KnownName = "Solid"; }
                else if (i == SpiderId) { element.KnownName = "Spider"; }
                else if (i == SpinningGunId) { element.KnownName = "Spinning Gun"; }
                else if (i == StarId) { element.KnownName = "Star"; }
                else if (i == StoneId) { element.KnownName = "Stone"; }
                else if (i == TigerId) { element.KnownName = "Tiger"; }
                else if (i == TorchId) { element.KnownName = "Torch"; }
                else if (i == TransporterId) { element.KnownName = "Transporter"; }
                else if (i == WaterId) { element.KnownName = "Water"; }
                else if (i == WebId) { element.KnownName = "Web"; }
                else if (i == Count - 7) { element.KnownName = "Text (Blue)"; }
                else if (i == Count - 6) { element.KnownName = "Text (Green)"; }
                else if (i == Count - 5) { element.KnownName = "Text (Cyan)"; }
                else if (i == Count - 4) { element.KnownName = "Text (Red)"; }
                else if (i == Count - 3) { element.KnownName = "Text (Purple)"; }
                else if (i == Count - 2) { element.KnownName = "Text (Brown)"; }
                else if (i == Count - 1) { element.KnownName = "Text (Black)"; }
                this.Cache[i] = element;
            }
        }

        virtual public int AmmoId { get { return -1; } }
        virtual public int BearId { get { return -1; } }
        virtual public int BlinkRayHId { get { return -1; } }
        virtual public int BlinkRayVId { get { return -1; } }
        virtual public int BlinkWallId { get { return -1; } }
        virtual public int BoardEdgeId { get { return 1; } }
        virtual public int BombId { get { return -1; } }
        virtual public int BoulderId { get { return -1; } }
        virtual public int BreakableId { get { return -1; } }
        virtual public int BulletId { get { return -1; } }
        virtual public int ClockwiseId { get { return -1; } }
        virtual public int CounterId { get { return -1; } }
        virtual public int DoorId { get { return -1; } }
        virtual public int DragonPupId { get { return -1; } }
        virtual public int DuplicatorId { get { return -1; } }
        virtual public int EmptyId { get { return -1; } }
        virtual public int EnergizerId { get { return -1; } }
        virtual public int FakeId { get { return -1; } }
        virtual public int FloorId { get { return -1; } }
        virtual public int ForestId { get { return -1; } }
        virtual public int GemId { get { return -1; } }
        virtual public int HeadId { get { return -1; } }
        virtual public int InvisibleId { get { return -1; } }
        virtual public int KeyId { get { return -1; } }
        virtual public int LavaId { get { return -1; } }
        virtual public int LineId { get { return -1; } }
        virtual public int LionId { get { return -1; } }
        virtual public int MessengerId { get { return -1; } }
        virtual public int MonitorId { get { return -1; } }
        virtual public int NormalId { get { return -1; } }
        virtual public int ObjectId { get { return -1; } }
        virtual public int PairerId { get { return -1; } }
        virtual public int PassageId { get { return -1; } }
        virtual public int PlayerId { get { return -1; } }
        virtual public int PusherId { get { return -1; } }
        virtual public int RicochetId { get { return -1; } }
        virtual public int RiverEId { get { return -1; } }
        virtual public int RiverNId { get { return -1; } }
        virtual public int RiverSId { get { return -1; } }
        virtual public int RiverWId { get { return -1; } }
        virtual public int RotonId { get { return -1; } }
        virtual public int RuffianId { get { return -1; } }
        virtual public int ScrollId { get { return -1; } }
        virtual public int SegmentId { get { return -1; } }
        virtual public int SharkId { get { return -1; } }
        virtual public int SliderEWId { get { return -1; } }
        virtual public int SliderNSId { get { return -1; } }
        virtual public int SlimeId { get { return -1; } }
        virtual public int SolidId { get { return -1; } }
        virtual public int SpiderId { get { return -1; } }
        virtual public int SpinningGunId { get { return -1; } }
        virtual public int StarId { get { return -1; } }
        virtual public int StoneId { get { return -1; } }
        virtual public int TigerId { get { return -1; } }
        virtual public int TorchId { get { return -1; } }
        virtual public int TransporterId { get { return -1; } }
        virtual public int WaterId { get { return -1; } }
        virtual public int WebId { get { return -1; } }

        public Element AmmoElement { get { return this[AmmoId]; } }
        public Element BearElement { get { return this[BearId]; } }
        public Element BlinkRayHElement { get { return this[BlinkRayHId]; } }
        public Element BlinkRayVElement { get { return this[BlinkRayVId]; } }
        public Element BlinkWallElement { get { return this[BlinkWallId]; } }
        public Element BoardEdgeElement { get { return this[BoardEdgeId]; } }
        public Element BombElement { get { return this[BombId]; } }
        public Element BoulderElement { get { return this[BoulderId]; } }
        public Element BreakableElement { get { return this[BreakableId]; } }
        public Element BulletElement { get { return this[BulletId]; } }
        public Element ClockwiseElement { get { return this[ClockwiseId]; } }
        public Element CounterElement { get { return this[CounterId]; } }
        public Element DoorElement { get { return this[DoorId]; } }
        public Element DragonPupElement { get { return this[DragonPupId]; } }
        public Element DuplicatorElement { get { return this[DuplicatorId]; } }
        public Element EmptyElement { get { return this[EmptyId]; } }
        public Element EnergizerElement { get { return this[EnergizerId]; } }
        public Element FakeElement { get { return this[FakeId]; } }
        public Element FloorElement { get { return this[FloorId]; } }
        public Element ForestElement { get { return this[ForestId]; } }
        public Element GemElement { get { return this[GemId]; } }
        public Element HeadElement { get { return this[HeadId]; } }
        public Element InvisibleElement { get { return this[InvisibleId]; } }
        public Element KeyElement { get { return this[KeyId]; } }
        public Element LavaElement { get { return this[LavaId]; } }
        public Element LineElement { get { return this[LineId]; } }
        public Element LionElement { get { return this[LionId]; } }
        public Element MessengerElement { get { return this[MessengerId]; } }
        public Element MonitorElement { get { return this[MonitorId]; } }
        public Element NormalElement { get { return this[NormalId]; } }
        public Element ObjectElement { get { return this[ObjectId]; } }
        public Element PairerElement { get { return this[PairerId]; } }
        public Element PassageElement { get { return this[PassageId]; } }
        public Element PlayerElement { get { return this[PlayerId]; } }
        public Element PusherElement { get { return this[PusherId]; } }
        public Element RicochetElement { get { return this[RicochetId]; } }
        public Element RiverEElement { get { return this[RiverEId]; } }
        public Element RiverNElement { get { return this[RiverNId]; } }
        public Element RiverSElement { get { return this[RiverSId]; } }
        public Element RiverWElement { get { return this[RiverWId]; } }
        public Element RotonElement { get { return this[RotonId]; } }
        public Element RuffianElement { get { return this[RuffianId]; } }
        public Element ScrollElement { get { return this[ScrollId]; } }
        public Element SegmentElement { get { return this[SegmentId]; } }
        public Element SharkElement { get { return this[SharkId]; } }
        public Element SliderEWElement { get { return this[SliderEWId]; } }
        public Element SliderNSElement { get { return this[SliderNSId]; } }
        public Element SlimeElement { get { return this[SlimeId]; } }
        public Element SolidElement { get { return this[SolidId]; } }
        public Element SpiderElement { get { return this[SpiderId]; } }
        public Element SpinningGunElement { get { return this[SpinningGunId]; } }
        public Element StarElement { get { return this[StarId]; } }
        public Element StoneElement { get { return this[StoneId]; } }
        public Element TigerElement { get { return this[TigerId]; } }
        public Element TorchElement { get { return this[TorchId]; } }
        public Element TransporterElement { get { return this[TransporterId]; } }
        public Element WaterElement { get { return this[WaterId]; } }
        public Element WebElement { get { return this[WebId]; } }

        public override Element this[int index]
        {
            get
            {
                if (index >= 0 && index < Count)
                    return Cache[index];
                return GetElement(index);
            }
            set
            {
                if (value is MemoryElementBase)
                {
                    GetElement(index).CopyFrom(value as MemoryElementBase);
                }
            }
        }

        private MemoryElementBase[] Cache
        {
            get;
            set;
        }

        abstract protected Type ElementType { get; }

        abstract protected MemoryElementBase GetElement(int index);

        public Memory Memory
        {
            get;
            private set;
        }
    }
}
