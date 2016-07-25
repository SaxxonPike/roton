using System.Collections.Generic;
using System.Linq;
using Roton.Core;
using Roton.Core.Collections;
using Roton.Emulation.Behavior;

namespace Roton.Emulation.Mapping
{
    internal abstract class ElementList : FixedList<IElement>, IElementList
    {
        protected ElementList(IMemory memory, int count)
        {
            Count = count;
            Memory = memory;
            BehaviorMap = new BehaviorMap(this);
            Cache = new Dictionary<int, IElement>();
        }

        protected IBehaviorMap BehaviorMap { get; }

        private IDictionary<int, IElement> Cache { get; }

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

        public string GetKnownName(int index)
        {
            if (index == AmmoId)
            {
                return "Ammo";
            }
            if (index == BearId)
            {
                return "Bear";
            }
            if (index == BlinkRayHId)
            {
                return "Blink Ray (H)";
            }
            if (index == BlinkRayVId)
            {
                return "Blink Ray (V)";
            }
            if (index == BlinkWallId)
            {
                return "Blink Wall";
            }
            if (index == BoardEdgeId)
            {
                return "Board Edge";
            }
            if (index == BombId)
            {
                return "Bomb";
            }
            if (index == BoulderId)
            {
                return "Boulder";
            }
            if (index == BreakableId)
            {
                return "Breakable Wall";
            }
            if (index == BulletId)
            {
                return "Bullet";
            }
            if (index == ClockwiseId)
            {
                return "Conveyor (Clockwise)";
            }
            if (index == CounterId)
            {
                return "Conveyor (Counter-Clockwise)";
            }
            if (index == DoorId)
            {
                return "Door";
            }
            if (index == DragonPupId)
            {
                return "Dragon Pup";
            }
            if (index == DuplicatorId)
            {
                return "Duplicator";
            }
            if (index == EmptyId)
            {
                return "Empty";
            }
            if (index == EnergizerId)
            {
                return "Energizer";
            }
            if (index == FakeId)
            {
                return "Fake Wall";
            }
            if (index == FloorId)
            {
                return "Floor";
            }
            if (index == ForestId)
            {
                return "Forest";
            }
            if (index == GemId)
            {
                return "Gem";
            }
            if (index == HeadId)
            {
                return "Centipede Head";
            }
            if (index == InvisibleId)
            {
                return "Invisible Wall";
            }
            if (index == KeyId)
            {
                return "Key";
            }
            if (index == LavaId)
            {
                return "Lava";
            }
            if (index == LineId)
            {
                return "Line";
            }
            if (index == LionId)
            {
                return "Lion";
            }
            if (index == MessengerId)
            {
                return "Messenger";
            }
            if (index == MonitorId)
            {
                return "Monitor";
            }
            if (index == NormalId)
            {
                return "Normal";
            }
            if (index == ObjectId)
            {
                return "Object";
            }
            if (index == PairerId)
            {
                return "Pairer";
            }
            if (index == PassageId)
            {
                return "Passage";
            }
            if (index == PlayerId)
            {
                return "Player";
            }
            if (index == PusherId)
            {
                return "Pusher";
            }
            if (index == RicochetId)
            {
                return "Ricochet";
            }
            if (index == RiverEId)
            {
                return "River (E)";
            }
            if (index == RiverNId)
            {
                return "River (N)";
            }
            if (index == RiverSId)
            {
                return "River (S)";
            }
            if (index == RiverWId)
            {
                return "River (W)";
            }
            if (index == RotonId)
            {
                return "Roton";
            }
            if (index == RuffianId)
            {
                return "Ruffian";
            }
            if (index == ScrollId)
            {
                return "Scroll";
            }
            if (index == SegmentId)
            {
                return "Centipede Segment";
            }
            if (index == SharkId)
            {
                return "Shark";
            }
            if (index == SliderEwId)
            {
                return "Slider (EW)";
            }
            if (index == SliderNsId)
            {
                return "Slider (NS)";
            }
            if (index == SlimeId)
            {
                return "Slime";
            }
            if (index == SolidId)
            {
                return "Solid";
            }
            if (index == SpiderId)
            {
                return "Spider";
            }
            if (index == SpinningGunId)
            {
                return "Spinning Gun";
            }
            if (index == StarId)
            {
                return "Star";
            }
            if (index == StoneId)
            {
                return "Stone";
            }
            if (index == TigerId)
            {
                return "Tiger";
            }
            if (index == TorchId)
            {
                return "Torch";
            }
            if (index == TransporterId)
            {
                return "Transporter";
            }
            if (index == WaterId)
            {
                return "Water";
            }
            if (index == WebId)
            {
                return "Web";
            }
            if (index == Count - 7)
            {
                return "Text (Blue)";
            }
            if (index == Count - 6)
            {
                return "Text (Green)";
            }
            if (index == Count - 5)
            {
                return "Text (Cyan)";
            }
            if (index == Count - 4)
            {
                return "Text (Red)";
            }
            if (index == Count - 3)
            {
                return "Text (Purple)";
            }
            if (index == Count - 2)
            {
                return "Text (Brown)";
            }
            if (index == Count - 1)
            {
                return "Text (Black)";
            }
            return $"Element {index}";
        }

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