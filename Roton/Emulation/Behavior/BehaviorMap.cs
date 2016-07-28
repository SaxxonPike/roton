using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class BehaviorMap : IBehaviorMap
    {
        private readonly IBehaviorMapConfiguration _config;
        private readonly IElementList _elementList;

        public BehaviorMap(IElementList elementList, IBehaviorMapConfiguration config)
        {
            _elementList = elementList;
            _config = config;
        }

        public IBehavior Map(int id)
        {
            if (id == _elementList.AmmoId)
                return new AmmoBehavior(_config.AmmoPerContainer);
            if (id == _elementList.BearId)
                return new BearBehavior();
            if (id == _elementList.BlinkRayHId)
                return new HorizontalBlinkRayBehavior();
            if (id == _elementList.BlinkRayVId)
                return new VerticalBlinkRayBehavior();
            if (id == _elementList.BlinkWallId)
                return new BlinkWallBehavior();
            if (id == _elementList.BoardEdgeId)
                return new BoardEdgeBehavior();
            if (id == _elementList.BombId)
                return new BombBehavior();
            if (id == _elementList.BoulderId)
                return new BoulderBehavior();
            if (id == _elementList.BreakableId)
                return new BreakableWallBehavior();
            if (id == _elementList.BulletId)
                return new BulletBehavior();
            if (id == _elementList.ClockwiseId)
                return new ClockwiseConveyorBehavior();
            if (id == _elementList.CounterId)
                return new CounterclockwiseConveyorBehavior();
            if (id == _elementList.DoorId)
                return new DoorBehavior();
            if (id == _elementList.DragonPupId)
                return new DragonPupBehavior();
            if (id == _elementList.DuplicatorId)
                return new DuplicatorBehavior();
            if (id == _elementList.EmptyId)
                return new EmptyBehavior();
            if (id == _elementList.EnergizerId)
                return new EnergizerBehavior();
            if (id == _elementList.FakeId)
                return new FakeWallBehavior();
            if (id == _elementList.FloorId)
                return new FloorBehavior();
            if (id == _elementList.ForestId)
                return new ForestBehavior(_config.ForestToFloor);
            if (id == _elementList.GemId)
                return new GemBehavior(_config.HealthPerGem, _config.ScorePerGem);
            if (id == _elementList.HeadId)
                return new CentipedeHeadBehavior();
            if (id == _elementList.InvisibleId)
                return new InvisibleWallBehavior();
            if (id == _elementList.KeyId)
                return new KeyBehavior();
            if (id == _elementList.LavaId)
                return new LavaBehavior();
            if (id == _elementList.LineId)
                return new LineWallBehavior();
            if (id == _elementList.LionId)
                return new LionBehavior();
            if (id == _elementList.MessengerId)
                return new MessengerBehavior();
            if (id == _elementList.MonitorId)
                return new MonitorBehavior();
            if (id == _elementList.NormalId)
                return new NormalWallBehavior();
            if (id == _elementList.ObjectId)
                return new ObjectBehavior(_config.MultiMovement);
            if (id == _elementList.PairerId)
                return new PairerBehavior();
            if (id == _elementList.PassageId)
                return new PassageBehavior(_config.BuggyPassages);
            if (id == _elementList.PlayerId)
                return new PlayerBehavior();
            if (id == _elementList.PusherId)
                return new PusherBehavior();
            if (id == _elementList.RicochetId)
                return new RicochetBehavior();
            if (id == _elementList.RiverEId)
                return new EastRiverBehavior();
            if (id == _elementList.RiverNId)
                return new NorthRiverBehavior();
            if (id == _elementList.RiverSId)
                return new SouthRiverBehavior();
            if (id == _elementList.RiverWId)
                return new WestRiverBehavior();
            if (id == _elementList.RotonId)
                return new RotonBehavior();
            if (id == _elementList.RuffianId)
                return new RuffianBehavior();
            if (id == _elementList.ScrollId)
                return new ScrollBehavior();
            if (id == _elementList.SegmentId)
                return new CentipedeSegmentBehavior();
            if (id == _elementList.SharkId)
                return new SharkBehavior();
            if (id == _elementList.SliderEwId)
                return new EastWestSliderBehavior();
            if (id == _elementList.SliderNsId)
                return new NorthSouthSliderBehavior();
            if (id == _elementList.SlimeId)
                return new SlimeBehavior();
            if (id == _elementList.SolidId)
                return new SolidWallBehavior();
            if (id == _elementList.SpiderId)
                return new SpiderBehavior();
            if (id == _elementList.SpinningGunId)
                return new SpinningGunBehavior();
            if (id == _elementList.StarId)
                return new StarBehavior();
            if (id == _elementList.StoneId)
                return new StoneBehavior();
            if (id == _elementList.TigerId)
                return new TigerBehavior();
            if (id == _elementList.TorchId)
                return new TorchBehavior();
            if (id == _elementList.TransporterId)
                return new TransporterBehavior();
            if (id == _elementList.WaterId)
                return new WaterBehavior();
            if (id == _elementList.WebId)
                return new WebBehavior();
            if (id == _elementList.Count - 7)
                return new BlueTextBehavior();
            if (id == _elementList.Count - 6)
                return new GreenTextBehavior();
            if (id == _elementList.Count - 5)
                return new CyanTextBehavior();
            if (id == _elementList.Count - 4)
                return new RedTextBehavior();
            if (id == _elementList.Count - 3)
                return new PurpleTextBehavior();
            if (id == _elementList.Count - 2)
                return new BrownTextBehavior();
            if (id == _elementList.Count - 1)
                return new BlackTextBehavior();
            return new UndefinedBehavior();
        }
    }
}