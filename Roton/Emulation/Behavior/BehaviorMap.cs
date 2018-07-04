using System.Collections.Generic;
using System.Linq;
using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public class BehaviorMap : IBehaviorMap
    {
        private readonly IBehaviorMapConfiguration _config;
        private readonly IList<ElementBehavior> _elementBehaviors;
        private readonly IElementList _elementList;

        public BehaviorMap(IElementList elementList, IBehaviorMapConfiguration config, IEnumerable<ElementBehavior> elementBehaviors)
        {
            _elementList = elementList;
            _config = config;
            _elementBehaviors = elementBehaviors.ToList();
        }

        public IBehavior Map(int id)
        {
            if (id == _elementList.AmmoId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Ammo);
            if (id == _elementList.BearId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Bear);
            if (id == _elementList.BlinkRayHId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BlinkRayH);
            if (id == _elementList.BlinkRayVId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BlinkRayV);
            if (id == _elementList.BlinkWallId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BlinkWall);
            if (id == _elementList.BoardEdgeId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BoardEdge);
            if (id == _elementList.BombId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Bomb);
            if (id == _elementList.BoulderId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Boulder);
            if (id == _elementList.BreakableId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Breakable);
            if (id == _elementList.BulletId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Bullet);
            if (id == _elementList.ClockwiseId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Clockwise);
            if (id == _elementList.CounterId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Counter);
            if (id == _elementList.DoorId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Door);
            if (id == _elementList.DragonPupId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.DragonPup);
            if (id == _elementList.DuplicatorId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Duplicator);
            if (id == _elementList.EmptyId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Empty);
            if (id == _elementList.EnergizerId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Energizer);
            if (id == _elementList.FakeId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Fake);
            if (id == _elementList.FloorId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Floor);
            if (id == _elementList.ForestId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Forest);
            if (id == _elementList.GemId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Gem);
            if (id == _elementList.HeadId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Head);
            if (id == _elementList.InvisibleId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Invisible);
            if (id == _elementList.KeyId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Key);
            if (id == _elementList.LavaId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Lava);
            if (id == _elementList.LineId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Line);
            if (id == _elementList.LionId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Lion);
            if (id == _elementList.MessengerId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Messenger);
            if (id == _elementList.MonitorId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Monitor);
            if (id == _elementList.NormalId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Normal);
            if (id == _elementList.ObjectId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Object);
            if (id == _elementList.PairerId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Pairer);
            if (id == _elementList.PassageId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Passage);
            if (id == _elementList.PlayerId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Player);
            if (id == _elementList.PusherId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Pusher);
            if (id == _elementList.RicochetId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Ricochet);
            if (id == _elementList.RiverEId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.RiverE);
            if (id == _elementList.RiverNId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.RiverN);
            if (id == _elementList.RiverSId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.RiverS);
            if (id == _elementList.RiverWId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.RiverW);
            if (id == _elementList.RotonId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Roton);
            if (id == _elementList.RuffianId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Ruffian);
            if (id == _elementList.ScrollId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Scroll);
            if (id == _elementList.SegmentId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Segment);
            if (id == _elementList.SharkId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Shark);
            if (id == _elementList.SliderEwId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.SliderEw);
            if (id == _elementList.SliderNsId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.SliderNs);
            if (id == _elementList.SlimeId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Slime);
            if (id == _elementList.SolidId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Solid);
            if (id == _elementList.SpiderId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Spider);
            if (id == _elementList.SpinningGunId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.SpinningGun);
            if (id == _elementList.StarId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Star);
            if (id == _elementList.StoneId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Stone);
            if (id == _elementList.TigerId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Tiger);
            if (id == _elementList.TorchId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Torch);
            if (id == _elementList.TransporterId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Transporter);
            if (id == _elementList.WaterId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Water);
            if (id == _elementList.WebId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Web);
            if (id == _elementList.Count - 7)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BlueText);
            if (id == _elementList.Count - 6)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.GreenText);
            if (id == _elementList.Count - 5)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.CyanText);
            if (id == _elementList.Count - 4)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.RedText);
            if (id == _elementList.Count - 3)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.PurpleText);
            if (id == _elementList.Count - 2)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BrownText);
            if (id == _elementList.Count - 1)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BlackText);
            return _elementBehaviors.First(e => e.KnownName == KnownNames.Undefined);
        }
    }
}