using System.Collections.Generic;

namespace Roton.Emulation.Data
{
    public interface IElementList : IEnumerable<IElement>
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
        int Count { get; }
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
        IElement this[int index] { get; }
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
    }
}