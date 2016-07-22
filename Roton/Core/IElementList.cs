﻿using System.Collections.Generic;

namespace Roton.Core
{
    public interface IElementList : IList<IElement>
    {
        int AmmoId { get; }
        int BearId { get; }
        int BlinkRayHId { get; }
        int BlinkRayVId { get; }
        int BlinkWallId { get; }
        int BoardEdgeId { get; }
        int BombId { get; }
        int BoulderId { get; }
        int BreakableId { get; }
        int BulletId { get; }
        int ClockwiseId { get; }
        int CounterId { get; }
        int DoorId { get; }
        int DragonPupId { get; }
        int DuplicatorId { get; }
        int EmptyId { get; }
        int EnergizerId { get; }
        int FakeId { get; }
        int FloorId { get; }
        int ForestId { get; }
        int GemId { get; }
        int HeadId { get; }
        int InvisibleId { get; }
        int KeyId { get; }
        int LavaId { get; }
        int LineId { get; }
        int LionId { get; }
        int MessengerId { get; }
        int MonitorId { get; }
        int NormalId { get; }
        int ObjectId { get; }
        int PairerId { get; }
        int PassageId { get; }
        int PlayerId { get; }
        int PusherId { get; }
        int RicochetId { get; }
        int RiverEId { get; }
        int RiverNId { get; }
        int RiverSId { get; }
        int RiverWId { get; }
        int RotonId { get; }
        int RuffianId { get; }
        int ScrollId { get; }
        int SegmentId { get; }
        int SharkId { get; }
        int SliderEwId { get; }
        int SliderNsId { get; }
        int SlimeId { get; }
        int SolidId { get; }
        int SpiderId { get; }
        int SpinningGunId { get; }
        int StarId { get; }
        int StoneId { get; }
        int TigerId { get; }
        int TorchId { get; }
        int TransporterId { get; }
        int WaterId { get; }
        int WebId { get; }

        IElement AmmoElement { get; }
        IElement BearElement { get; }
        IElement BlinkRayHElement { get; }
        IElement BlinkRayVElement { get; }
        IElement BlinkWallElement { get; }
        IElement BoardEdgeElement { get; }
        IElement BombElement { get; }
        IElement BoulderElement { get; }
        IElement BreakableElement { get; }
        IElement BulletElement { get; }
        IElement ClockwiseElement { get; }
        IElement CounterElement { get; }
        IElement DoorElement { get; }
        IElement DragonPupElement { get; }
        IElement DuplicatorElement { get; }
        IElement EmptyElement { get; }
        IElement EnergizerElement { get; }
        IElement FakeElement { get; }
        IElement FloorElement { get; }
        IElement ForestElement { get; }
        IElement GemElement { get; }
        IElement HeadElement { get; }
        IElement InvisibleElement { get; }
        IElement KeyElement { get; }
        IElement LavaElement { get; }
        IElement LineElement { get; }
        IElement LionElement { get; }
        IElement MessengerElement { get; }
        IElement MonitorElement { get; }
        IElement NormalElement { get; }
        IElement ObjectElement { get; }
        IElement PairerElement { get; }
        IElement PassageElement { get; }
        IElement PlayerElement { get; }
        IElement PusherElement { get; }
        IElement RicochetElement { get; }
        IElement RiverEElement { get; }
        IElement RiverNElement { get; }
        IElement RiverSElement { get; }
        IElement RiverWElement { get; }
        IElement RotonElement { get; }
        IElement RuffianElement { get; }
        IElement ScrollElement { get; }
        IElement SegmentElement { get; }
        IElement SharkElement { get; }
        IElement SliderEwElement { get; }
        IElement SliderNsElement { get; }
        IElement SlimeElement { get; }
        IElement SolidElement { get; }
        IElement SpiderElement { get; }
        IElement SpinningGunElement { get; }
        IElement StarElement { get; }
        IElement StoneElement { get; }
        IElement TigerElement { get; }
        IElement TorchElement { get; }
        IElement TransporterElement { get; }
        IElement WaterElement { get; }
        IElement WebElement { get; }
    }
}