using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Data;

namespace Roton.Emulation.Behaviors
{
    public class BehaviorMap : IBehaviorMap
    {
        private readonly ICollection<IBehavior> _elementBehaviors;
        private readonly IElements _elements;

        public BehaviorMap(IElements elements, ICollection<IBehavior> elementBehaviors)
        {
            _elements = elements;
            _elementBehaviors = elementBehaviors;
        }

        public IBehavior Map(int id)
        {
            if (id == _elements.AmmoId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Ammo);
            if (id == _elements.BearId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Bear);
            if (id == _elements.BlinkRayHId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BlinkRayH);
            if (id == _elements.BlinkRayVId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BlinkRayV);
            if (id == _elements.BlinkWallId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BlinkWall);
            if (id == _elements.BoardEdgeId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BoardEdge);
            if (id == _elements.BombId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Bomb);
            if (id == _elements.BoulderId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Boulder);
            if (id == _elements.BreakableId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Breakable);
            if (id == _elements.BulletId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Bullet);
            if (id == _elements.ClockwiseId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Clockwise);
            if (id == _elements.CounterId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Counter);
            if (id == _elements.DoorId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Door);
            if (id == _elements.DragonPupId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.DragonPup);
            if (id == _elements.DuplicatorId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Duplicator);
            if (id == _elements.EmptyId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Empty);
            if (id == _elements.EnergizerId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Energizer);
            if (id == _elements.FakeId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Fake);
            if (id == _elements.FloorId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Floor);
            if (id == _elements.ForestId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Forest);
            if (id == _elements.GemId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Gem);
            if (id == _elements.HeadId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Head);
            if (id == _elements.InvisibleId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Invisible);
            if (id == _elements.KeyId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Key);
            if (id == _elements.LavaId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Lava);
            if (id == _elements.LineId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Line);
            if (id == _elements.LionId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Lion);
            if (id == _elements.MessengerId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Messenger);
            if (id == _elements.MonitorId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Monitor);
            if (id == _elements.NormalId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Normal);
            if (id == _elements.ObjectId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Object);
            if (id == _elements.PairerId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Pairer);
            if (id == _elements.PassageId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Passage);
            if (id == _elements.PlayerId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Player);
            if (id == _elements.PusherId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Pusher);
            if (id == _elements.RicochetId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Ricochet);
            if (id == _elements.RiverEId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.RiverE);
            if (id == _elements.RiverNId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.RiverN);
            if (id == _elements.RiverSId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.RiverS);
            if (id == _elements.RiverWId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.RiverW);
            if (id == _elements.RotonId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Roton);
            if (id == _elements.RuffianId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Ruffian);
            if (id == _elements.ScrollId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Scroll);
            if (id == _elements.SegmentId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Segment);
            if (id == _elements.SharkId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Shark);
            if (id == _elements.SliderEwId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.SliderEw);
            if (id == _elements.SliderNsId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.SliderNs);
            if (id == _elements.SlimeId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Slime);
            if (id == _elements.SolidId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Solid);
            if (id == _elements.SpiderId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Spider);
            if (id == _elements.SpinningGunId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.SpinningGun);
            if (id == _elements.StarId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Star);
            if (id == _elements.StoneId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Stone);
            if (id == _elements.TigerId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Tiger);
            if (id == _elements.TorchId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Torch);
            if (id == _elements.TransporterId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Transporter);
            if (id == _elements.WaterId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Water);
            if (id == _elements.WebId)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.Web);
            if (id == _elements.Count - 7)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BlueText);
            if (id == _elements.Count - 6)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.GreenText);
            if (id == _elements.Count - 5)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.CyanText);
            if (id == _elements.Count - 4)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.RedText);
            if (id == _elements.Count - 3)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.PurpleText);
            if (id == _elements.Count - 2)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BrownText);
            if (id == _elements.Count - 1)
                return _elementBehaviors.First(e => e.KnownName == KnownNames.BlackText);
            return _elementBehaviors.First(e => e.KnownName == KnownNames.Undefined);
        }
    }
}