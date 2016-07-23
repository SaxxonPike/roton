using Roton.Core;
using Roton.Core.Collections;

namespace Roton.Emulation.Mapping
{
    internal abstract class ElementList : FixedList<IElement>, IElementList
    {
        protected ElementList(IMemory memory, int count)
        {
            Count = count;
            Memory = memory;
            Cache = new IElement[Count];

            for (var i = 0; i < Count; i++)
            {
                var element = GetElement(i);
                if (i == AmmoId)
                {
                    element.KnownName = "Ammo";
                }
                else if (i == BearId)
                {
                    element.KnownName = "Bear";
                }
                else if (i == BlinkRayHId)
                {
                    element.KnownName = "Blink Ray (H)";
                }
                else if (i == BlinkRayVId)
                {
                    element.KnownName = "Blink Ray (V)";
                }
                else if (i == BlinkWallId)
                {
                    element.KnownName = "Blink Wall";
                }
                else if (i == BoardEdgeId)
                {
                    element.KnownName = "Board Edge";
                }
                else if (i == BombId)
                {
                    element.KnownName = "Bomb";
                }
                else if (i == BoulderId)
                {
                    element.KnownName = "Boulder";
                }
                else if (i == BreakableId)
                {
                    element.KnownName = "Breakable Wall";
                }
                else if (i == BulletId)
                {
                    element.KnownName = "Bullet";
                }
                else if (i == ClockwiseId)
                {
                    element.KnownName = "Conveyor (Clockwise)";
                }
                else if (i == CounterId)
                {
                    element.KnownName = "Conveyor (Counter-Clockwise)";
                }
                else if (i == DoorId)
                {
                    element.KnownName = "Door";
                }
                else if (i == DragonPupId)
                {
                    element.KnownName = "Dragon Pup";
                }
                else if (i == DuplicatorId)
                {
                    element.KnownName = "Duplicator";
                }
                else if (i == EmptyId)
                {
                    element.KnownName = "Empty";
                }
                else if (i == EnergizerId)
                {
                    element.KnownName = "Energizer";
                }
                else if (i == FakeId)
                {
                    element.KnownName = "Fake Wall";
                }
                else if (i == FloorId)
                {
                    element.KnownName = "Floor";
                }
                else if (i == ForestId)
                {
                    element.KnownName = "Forest";
                }
                else if (i == GemId)
                {
                    element.KnownName = "Gem";
                }
                else if (i == HeadId)
                {
                    element.KnownName = "Centipede Head";
                }
                else if (i == InvisibleId)
                {
                    element.KnownName = "Invisible Wall";
                }
                else if (i == KeyId)
                {
                    element.KnownName = "Key";
                }
                else if (i == LavaId)
                {
                    element.KnownName = "Lava";
                }
                else if (i == LineId)
                {
                    element.KnownName = "Line";
                }
                else if (i == LionId)
                {
                    element.KnownName = "Lion";
                }
                else if (i == MessengerId)
                {
                    element.KnownName = "Messenger";
                }
                else if (i == MonitorId)
                {
                    element.KnownName = "Monitor";
                }
                else if (i == NormalId)
                {
                    element.KnownName = "Normal";
                }
                else if (i == ObjectId)
                {
                    element.KnownName = "Object";
                }
                else if (i == PairerId)
                {
                    element.KnownName = "Pairer";
                }
                else if (i == PassageId)
                {
                    element.KnownName = "Passage";
                }
                else if (i == PlayerId)
                {
                    element.KnownName = "Player";
                }
                else if (i == PusherId)
                {
                    element.KnownName = "Pusher";
                }
                else if (i == RicochetId)
                {
                    element.KnownName = "Ricochet";
                }
                else if (i == RiverEId)
                {
                    element.KnownName = "River (E)";
                }
                else if (i == RiverNId)
                {
                    element.KnownName = "River (N)";
                }
                else if (i == RiverSId)
                {
                    element.KnownName = "River (S)";
                }
                else if (i == RiverWId)
                {
                    element.KnownName = "River (W)";
                }
                else if (i == RotonId)
                {
                    element.KnownName = "Roton";
                }
                else if (i == RuffianId)
                {
                    element.KnownName = "Ruffian";
                }
                else if (i == ScrollId)
                {
                    element.KnownName = "Scroll";
                }
                else if (i == SegmentId)
                {
                    element.KnownName = "Centipede Segment";
                }
                else if (i == SharkId)
                {
                    element.KnownName = "Shark";
                }
                else if (i == SliderEwId)
                {
                    element.KnownName = "Slider (EW)";
                }
                else if (i == SliderNsId)
                {
                    element.KnownName = "Slider (NS)";
                }
                else if (i == SlimeId)
                {
                    element.KnownName = "Slime";
                }
                else if (i == SolidId)
                {
                    element.KnownName = "Solid";
                }
                else if (i == SpiderId)
                {
                    element.KnownName = "Spider";
                }
                else if (i == SpinningGunId)
                {
                    element.KnownName = "Spinning Gun";
                }
                else if (i == StarId)
                {
                    element.KnownName = "Star";
                }
                else if (i == StoneId)
                {
                    element.KnownName = "Stone";
                }
                else if (i == TigerId)
                {
                    element.KnownName = "Tiger";
                }
                else if (i == TorchId)
                {
                    element.KnownName = "Torch";
                }
                else if (i == TransporterId)
                {
                    element.KnownName = "Transporter";
                }
                else if (i == WaterId)
                {
                    element.KnownName = "Water";
                }
                else if (i == WebId)
                {
                    element.KnownName = "Web";
                }
                else if (i == Count - 7)
                {
                    element.KnownName = "Text (Blue)";
                }
                else if (i == Count - 6)
                {
                    element.KnownName = "Text (Green)";
                }
                else if (i == Count - 5)
                {
                    element.KnownName = "Text (Cyan)";
                }
                else if (i == Count - 4)
                {
                    element.KnownName = "Text (Red)";
                }
                else if (i == Count - 3)
                {
                    element.KnownName = "Text (Purple)";
                }
                else if (i == Count - 2)
                {
                    element.KnownName = "Text (Brown)";
                }
                else if (i == Count - 1)
                {
                    element.KnownName = "Text (Black)";
                }
                Cache[i] = element;
            }
        }

        private IElement[] Cache { get; }

        protected IMemory Memory { get; private set; }

        public IElement AmmoElement => this[AmmoId];

        public virtual int AmmoId => -1;
        public IElement BearElement => this[BearId];
        public virtual int BearId => -1;
        public IElement BlinkRayHElement => this[BlinkRayHId];
        public virtual int BlinkRayHId => -1;
        public IElement BlinkRayVElement => this[BlinkRayVId];
        public virtual int BlinkRayVId => -1;
        public IElement BlinkWallElement => this[BlinkWallId];
        public virtual int BlinkWallId => -1;
        public IElement BoardEdgeElement => this[BoardEdgeId];
        public virtual int BoardEdgeId => 1;
        public IElement BombElement => this[BombId];
        public virtual int BombId => -1;
        public IElement BoulderElement => this[BoulderId];
        public virtual int BoulderId => -1;
        public IElement BreakableElement => this[BreakableId];
        public virtual int BreakableId => -1;
        public IElement BulletElement => this[BulletId];
        public virtual int BulletId => -1;
        public IElement ClockwiseElement => this[ClockwiseId];
        public virtual int ClockwiseId => -1;

        public sealed override int Count { get; }
        public IElement CounterElement => this[CounterId];
        public virtual int CounterId => -1;
        public IElement DoorElement => this[DoorId];
        public virtual int DoorId => -1;
        public IElement DragonPupElement => this[DragonPupId];
        public virtual int DragonPupId => -1;
        public IElement DuplicatorElement => this[DuplicatorId];
        public virtual int DuplicatorId => -1;
        public IElement EmptyElement => this[EmptyId];
        public virtual int EmptyId => -1;
        public IElement EnergizerElement => this[EnergizerId];
        public virtual int EnergizerId => -1;
        public IElement FakeElement => this[FakeId];
        public virtual int FakeId => -1;
        public IElement FloorElement => this[FloorId];
        public virtual int FloorId => -1;
        public IElement ForestElement => this[ForestId];
        public virtual int ForestId => -1;
        public IElement GemElement => this[GemId];
        public virtual int GemId => -1;
        public IElement HeadElement => this[HeadId];
        public virtual int HeadId => -1;
        public IElement InvisibleElement => this[InvisibleId];
        public virtual int InvisibleId => -1;
        public IElement KeyElement => this[KeyId];
        public virtual int KeyId => -1;
        public IElement LavaElement => this[LavaId];
        public virtual int LavaId => -1;
        public IElement LineElement => this[LineId];
        public virtual int LineId => -1;
        public IElement LionElement => this[LionId];
        public virtual int LionId => -1;
        public IElement MessengerElement => this[MessengerId];
        public virtual int MessengerId => -1;
        public IElement MonitorElement => this[MonitorId];
        public virtual int MonitorId => -1;
        public IElement NormalElement => this[NormalId];
        public virtual int NormalId => -1;
        public IElement ObjectElement => this[ObjectId];
        public virtual int ObjectId => -1;
        public IElement PairerElement => this[PairerId];
        public virtual int PairerId => -1;
        public IElement PassageElement => this[PassageId];
        public virtual int PassageId => -1;
        public IElement PlayerElement => this[PlayerId];
        public virtual int PlayerId => -1;
        public IElement PusherElement => this[PusherId];
        public virtual int PusherId => -1;
        public IElement RicochetElement => this[RicochetId];
        public virtual int RicochetId => -1;
        public IElement RiverEElement => this[RiverEId];
        public virtual int RiverEId => -1;
        public IElement RiverNElement => this[RiverNId];
        public virtual int RiverNId => -1;
        public IElement RiverSElement => this[RiverSId];
        public virtual int RiverSId => -1;
        public IElement RiverWElement => this[RiverWId];
        public virtual int RiverWId => -1;
        public IElement RotonElement => this[RotonId];
        public virtual int RotonId => -1;
        public IElement RuffianElement => this[RuffianId];
        public virtual int RuffianId => -1;
        public IElement ScrollElement => this[ScrollId];
        public virtual int ScrollId => -1;
        public IElement SegmentElement => this[SegmentId];
        public virtual int SegmentId => -1;
        public IElement SharkElement => this[SharkId];
        public virtual int SharkId => -1;
        public IElement SliderEwElement => this[SliderEwId];
        public virtual int SliderEwId => -1;
        public IElement SliderNsElement => this[SliderNsId];
        public virtual int SliderNsId => -1;
        public IElement SlimeElement => this[SlimeId];
        public virtual int SlimeId => -1;
        public IElement SolidElement => this[SolidId];
        public virtual int SolidId => -1;
        public IElement SpiderElement => this[SpiderId];
        public virtual int SpiderId => -1;
        public IElement SpinningGunElement => this[SpinningGunId];
        public virtual int SpinningGunId => -1;
        public IElement StarElement => this[StarId];
        public virtual int StarId => -1;
        public IElement StoneElement => this[StoneId];
        public virtual int StoneId => -1;
        public IElement TigerElement => this[TigerId];
        public virtual int TigerId => -1;
        public IElement TorchElement => this[TorchId];
        public virtual int TorchId => -1;
        public IElement TransporterElement => this[TransporterId];
        public virtual int TransporterId => -1;
        public IElement WaterElement => this[WaterId];
        public virtual int WaterId => -1;
        public IElement WebElement => this[WebId];
        public virtual int WebId => -1;

        protected abstract IElement GetElement(int index);

        protected sealed override IElement GetItem(int index)
        {
            if (index >= 0 && index < Count)
                return Cache[index];
            return GetElement(index);
        }

        protected sealed override void SetItem(int index, IElement value)
        {
            throw Exceptions.InvalidSet;
        }
    }
}