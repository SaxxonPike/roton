using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Emulation.Timing;
using Roton.Extensions;
using Roton.FileIo;

namespace Roton.Emulation.Execution
{
    internal abstract class Core : IDisplayInfo, ICore
    {
        private readonly ICoreConfiguration _config;
        private int _timerTick;

        protected Core(ICoreConfiguration config)
        {
            _config = config;
            Boards = new List<IPackedBoard>();
            Memory = new Memory();
            Random = new Random();
            RandomDeterministic = new Random(0);
        }

        private Random Random { get; }

        private Random RandomDeterministic { get; }

        private int TimerBase => CoreTimer.Tick & 0x7FFF;

        private Thread Thread { get; set; }

        private bool ThreadActive { get; set; }

        private int TimerTick
        {
            get { return _timerTick; }
            set { _timerTick = value & 0x7FFF; }
        }

        public bool AboutShown
        {
            get { return StateData.AboutShown; }
            set { StateData.AboutShown = value; }
        }

        public void ActBear(int index)
        {
            var actor = Actors[index];
            var vector = new Vector();

            if (Player.Location.X == actor.Location.X || (8 - actor.P1 < Player.Location.Y.AbsDiff(actor.Location.Y)))
            {
                if (8 - actor.P1 < Player.Location.X.AbsDiff(actor.Location.X))
                {
                    vector.SetTo(0, 0);
                }
                else
                {
                    vector.SetTo(0, (Player.Location.Y - actor.Location.Y).Polarity());
                }
            }
            else
            {
                vector.SetTo((Player.Location.X - actor.Location.X).Polarity(), 0);
            }

            var target = actor.Location.Sum(vector);
            var targetElement = ElementAt(target);

            if (targetElement.IsFloor)
            {
                MoveActor(index, target);
            }
            else if (targetElement.Id == Elements.PlayerId || targetElement.Id == Elements.BreakableId)
            {
                Attack(index, target);
            }
        }

        public void ActBlinkWall(int index)
        {
            var actor = Actors[index];

            if (actor.P3 == 0)
                actor.P3 = actor.P1 + 1;

            if (actor.P3 == 1)
            {
                actor.P3 = actor.P2*2 + 1;

                var erasedRay = false;
                var target = actor.Location.Sum(actor.Vector);
                var emptyElement = Elements.EmptyId;

                var rayElement = actor.Vector.X == 0
                    ? Elements.BlinkRayVId
                    : Elements.BlinkRayHId;

                var color = TileAt(actor.Location).Color;
                var rayTile = new Tile(rayElement, color);

                while (TileAt(target).Matches(rayTile))
                {
                    TileAt(target).Id = emptyElement;
                    UpdateBoard(target);
                    target.Add(actor.Vector);
                    erasedRay = true;
                }

                if (erasedRay) return;
                var blocked = false;

                do
                {
                    if (ElementAt(target).IsDestructible)
                    {
                        Destroy(target);
                    }

                    if (TileAt(target).Id == Elements.PlayerId)
                    {
                        var playerIndex = ActorIndexAt(target);
                        IXyPair testVector;

                        if (actor.Vector.Y == 0)
                        {
                            testVector = new Vector(0, 1);
                            if (TileAt(target.Difference(testVector)).Id == emptyElement)
                            {
                                MoveActor(playerIndex, target.Difference(testVector));
                            }
                            else if (TileAt(target.Sum(testVector)).Id == emptyElement)
                            {
                                MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }
                        else
                        {
                            testVector = new Vector(1, 0);
                            if (TileAt(target.Sum(testVector)).Id == emptyElement)
                            {
                                MoveActor(playerIndex, target.Sum(testVector));
                            }
                            else if (TileAt(target.Difference(testVector)).Id == emptyElement)
                            {
                                // "sum" is not a mistake; this is an original engine bug
                                MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }
                        if (TileAt(target).Id == Elements.PlayerId)
                        {
                            while (Health > 0)
                            {
                                Harm(0);
                            }
                            blocked = true;
                        }
                    }
                    if (TileAt(target).Id == emptyElement)
                    {
                        TileAt(target).CopyFrom(rayTile);
                        UpdateBoard(target);
                    }
                    else
                    {
                        blocked = true;
                    }
                    target.Add(actor.Vector);
                } while (!blocked);
            }
            else
            {
                actor.P3--;
            }
        }

        public void ActBomb(int index)
        {
            var actor = Actors[index];
            if (actor.P1 <= 0) return;

            actor.P1--;
            UpdateBoard(actor.Location);
            switch (actor.P1)
            {
                case 1:
                    PlaySound(1, Sounds.BombExplode);
                    UpdateRadius(actor.Location, RadiusMode.Explode);
                    break;
                case 0:
                    var location = actor.Location.Clone();
                    RemoveActor(index);
                    UpdateRadius(location, RadiusMode.Clear);
                    break;
                default:
                    PlaySound(1, (actor.P1 & 0x01) == 0 ? Sounds.BombTock : Sounds.BombTick);
                    break;
            }
        }

        public void ActBullet(int index)
        {
            var actor = Actors[index];
            var canRicochet = true;
            while (true)
            {
                var target = actor.Location.Sum(actor.Vector);
                var element = ElementAt(target);
                if (element.IsFloor || element.Id == Elements.WaterId)
                {
                    MoveActor(index, target);
                    break;
                }
                if (canRicochet && element.Id == Elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetOpposite();
                    PlaySound(1, Sounds.Ricochet);
                    continue;
                }
                if (element.Id == Elements.BreakableId ||
                    (element.IsDestructible && (element.Id == Elements.PlayerId || actor.P1 == 0)))
                {
                    if (element.Points != 0)
                    {
                        Score += element.Points;
                        UpdateStatus();
                    }
                    Attack(index, target);
                    break;
                }
                if (canRicochet && TileAt(actor.Location.Sum(actor.Vector.Clockwise())).Id == Elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetCounterClockwise();
                    PlaySound(1, Sounds.Ricochet);
                    continue;
                }
                if (canRicochet &&
                    TileAt(actor.Location.Sum(actor.Vector.CounterClockwise())).Id == Elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetClockwise();
                    PlaySound(1, Sounds.Ricochet);
                    continue;
                }
                RemoveActor(index);
                ActIndex--;
                if (element.Id == Elements.ObjectId || element.Id == Elements.ScrollId)
                {
                    BroadcastLabel(-ActorIndexAt(target), @"SHOT", false);
                }
                break;
            }
        }

        public void ActClockwise(int index)
        {
            var actor = Actors[index];
            UpdateBoard(actor.Location);
            Convey(actor.Location, 1);
        }

        public void ActCounter(int index)
        {
            var actor = Actors[index];
            UpdateBoard(actor.Location);
            Convey(actor.Location, -1);
        }

        public void ActDragonPup(int index)
        {
            UpdateBoard(Actors[index].Location);
        }

        public void ActDuplicator(int index)
        {
            var actor = Actors[index];
            var source = actor.Location.Sum(actor.Vector);
            var target = actor.Location.Difference(actor.Vector);

            if (actor.P1 > 4)
            {
                if (TileAt(target).Id == Elements.PlayerId)
                {
                    ElementAt(source).Interact(source, 0, KeyVector);
                }
                else
                {
                    if (TileAt(target).Id != Elements.EmptyId)
                    {
                        Push(target, actor.Vector.Opposite());
                    }
                    if (TileAt(target).Id == Elements.EmptyId)
                    {
                        var sourceIndex = ActorIndexAt(source);
                        if (sourceIndex > 0)
                        {
                            if (ActorCount < Actors.Count - 2)
                            {
                                var sourceTile = TileAt(source);
                                SpawnActor(target, sourceTile, Elements[sourceTile.Id].Cycle, Actors[sourceIndex]);
                                UpdateBoard(target);
                            }
                        }
                        else if (sourceIndex != 0)
                        {
                            TileAt(target).CopyFrom(TileAt(source));
                            UpdateBoard(target);
                        }
                        PlaySound(3, Sounds.Duplicate);
                    }
                    else
                    {
                        PlaySound(3, Sounds.DuplicateFail);
                    }
                }
                actor.P1 = 0;
            }
            else
            {
                actor.P1++;
            }

            UpdateBoard(actor.Location);
            actor.Cycle = (9 - actor.P2)*3;
        }

        public void ActHead(int index)
        {
            var player = Player;
            var actor = Actors[index];

            // The centipede can randomly change direction towards the player if aligned

            if (player.Location.X == actor.Location.X && actor.P1 > RandomNumberDeterministic(10))
            {
                Seek(actor.Location, actor.Vector);
            }
            else if (player.Location.Y == actor.Location.Y && actor.P1 > RandomNumberDeterministic(10))
            {
                Seek(actor.Location, actor.Vector);
            }
            else if (actor.Vector.IsZero() || actor.P2 > RandomNumberDeterministic(10) << 2)
            {
                Rnd(actor.Vector);
            }

            if (actor.Vector.IsNonZero())
            {
                // The centipede wants to move, determine where it can

                var vector = actor.Vector.Clone();
                var element = ElementAt(actor.Location.Sum(vector));
                if (!element.IsFloor && element.Id != Elements.PlayerId)
                {
                    RndP(vector, actor.Vector);
                    element = ElementAt(actor.Location.Sum(actor.Vector));
                    if (!element.IsFloor && element.Id != Elements.PlayerId)
                    {
                        actor.Vector.SetOpposite();
                        element = ElementAt(actor.Location.Sum(actor.Vector));
                        if (!element.IsFloor && element.Id != Elements.PlayerId)
                        {
                            actor.Vector.CopyFrom(vector.Opposite());
                            element = ElementAt(actor.Location.Sum(actor.Vector));
                            if (!element.IsFloor && element.Id != Elements.PlayerId)
                            {
                                actor.Vector.SetTo(0, 0);
                            }
                        }
                    }
                }
            }

            if (actor.Vector.IsZero())
            {
                // Reverse the centipede

                TileAt(actor.Location).Id = Elements.SegmentId;
                UpdateBoard(actor.Location);
                var segmentIndex = index;
                while (true)
                {
                    var segment = Actors[segmentIndex];
                    var i = segment.Follower;
                    segment.Follower = segment.Leader;
                    segment.Leader = i;
                    if (i > 0)
                        segmentIndex = i;
                    else
                        break;
                }
                var newHead = Actors[segmentIndex];
                TileAt(newHead.Location).Id = Elements.HeadId;
                UpdateBoard(newHead.Location);
            }
            else
            {
                // The centipede has a direction to go, so move it

                var target = actor.Location.Sum(actor.Vector);

                if (ElementAt(target).Id == Elements.PlayerId)
                {
                    // The centipede is moving into a player

                    if (actor.Follower > 0)
                    {
                        var follower = Actors[actor.Follower];
                        TileAt(follower.Location).Id = Elements.HeadId;
                        follower.Leader = -1;
                        UpdateBoard(follower.Location);
                    }
                    actor.Follower = -1;
                    actor.Leader = -1;
                    Attack(index, target);
                }
                else
                {
                    MoveActor(index, target);
                    var segmentIndex = index;

                    // The centipede has moved, so move its followers

                    do
                    {
                        var segment = Actors[segmentIndex];
                        var origin = segment.Location.Difference(segment.Vector);
                        var vector = segment.Vector;

                        if (segment.Follower < 0)
                        {
                            // Determine if there are any eligible new follower segments
                            if (ElementAt(origin.Difference(vector)).Id == Elements.SegmentId &&
                                ActorAt(origin.Difference(vector)).Leader <= 0)
                            {
                                segment.Follower = ActorIndexAt(origin.Difference(vector));
                            }
                            else if (ElementAt(origin.Difference(vector.Swap())).Id == Elements.SegmentId &&
                                     ActorAt(origin.Difference(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = ActorIndexAt(origin.Difference(vector.Swap()));
                            }
                            else if (ElementAt(origin.Sum(vector.Swap())).Id == Elements.SegmentId &&
                                     ActorAt(origin.Sum(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = ActorIndexAt(origin.Sum(vector.Swap()));
                            }
                            else
                            {
                                segment.Follower = -1;
                            }
                        }

                        // Move follower segment
                        var followerIndex = segment.Follower;
                        if (followerIndex == segmentIndex)
                        {
                            throw Exceptions.SelfReferenceCentipede;
                        }
                        if (followerIndex > 0)
                        {
                            var follower = Actors[followerIndex];
                            follower.Leader = segmentIndex;
                            follower.P1 = segment.P1;
                            follower.P2 = segment.P2;
                            follower.Vector.SetTo(origin.X - follower.Location.X, origin.Y - follower.Location.Y);
                            MoveActor(segment.Follower, origin);
                        }

                        segmentIndex = segment.Follower;
                    } while (segmentIndex > 0);
                }
            }
        }

        public int ActIndex
        {
            get { return StateData.ActIndex; }
            set { StateData.ActIndex = value; }
        }

        public void ActLion(int index)
        {
            var actor = Actors[index];
            var vector = new Vector();
            if (actor.P1 >= RandomNumberDeterministic(10))
            {
                Seek(actor.Location, vector);
            }
            else
            {
                Rnd(vector);
            }
            var target = actor.Location.Sum(vector);
            var element = ElementAt(target);
            if (element.IsFloor)
            {
                MoveActor(index, target);
            }
            else if (element.Id == Elements.PlayerId)
            {
                Attack(index, target);
            }
        }

        public void ActMessenger(int index)
        {
            var actor = Actors[index];
            if (actor.Location.X == 0)
            {
                Hud.DrawMessage(Message, actor.P2%7 + 9);
                actor.P2--;
                if (actor.P2 > 0) return;

                RemoveActor(index);
                ActIndex--;
                UpdateBorder();
                Message = "";
            }
        }

        public virtual void ActMonitor(int index)
        {
            if (KeyPressed != 0)
            {
                BreakGameLoop = true;
            }
        }

        public void ActObject(int index)
        {
            var actor = Actors[index];
            if (actor.Instruction >= 0)
            {
                ExecuteCode(index, actor, @"Interaction");
            }
            if (actor.Vector.IsZero()) return;

            var target = actor.Location.Sum(actor.Vector);
            if (ElementAt(target).IsFloor)
            {
                MoveActor(index, target);
            }
            else
            {
                BroadcastLabel(-index, @"THUD", false);
            }
        }

        public IActor ActorAt(IXyPair location)
        {
            return Actors[ActorIndexAt(location)];
        }

        public int ActorCount
        {
            get { return StateData.ActorCount; }
            set { StateData.ActorCount = value; }
        }

        public int ActorIndexAt(IXyPair location)
        {
            var index = 0;
            foreach (var actor in Actors)
            {
                if (actor.Location.X == location.X && actor.Location.Y == location.Y)
                    return index;
                index++;
            }
            return -1;
        }

        public abstract IActorList Actors { get; }

        public void ActPairer(int index)
        {
            // pairer code only reads the actor's tile...
        }

        public virtual void ActPlayer(int index)
        {
            var actor = Actors[index];
            var playerElement = Elements.PlayerElement;

            // Energizer graphics

            if (EnergyCycles > 0)
            {
                playerElement.Character = playerElement.Character == 1 ? 2 : 1;

                if ((GameCycle & 0x01) == 0)
                {
                    TileAt(actor.Location).Color = ((GameCycle%7 + 1) << 4) | 0x0F;
                }
                else
                {
                    TileAt(actor.Location).Color = 0x0F;
                }

                UpdateBoard(actor.Location);
            }
            else
            {
                ForcePlayerColor(index);
            }

            // Health logic

            if (Health <= 0)
            {
                KeyVector.SetTo(0, 0);
                KeyShift = false;
                if (ActorIndexAt(new Location(0, 0)) == -1)
                {
                    SetMessage(0x7D00, Alerts.GameOverMessage);
                }
                GameWaitTime = 0;
                GameOver = true;
            }

            if (KeyVector.IsNonZero())
            {
                if (KeyShift || KeyPressed == 0x20)
                {
                    // Shooting logic

                    if (Shots > 0)
                    {
                        if (Ammo > 0)
                        {
                            var bulletCount = Actors.Count(a => a.P1 == 0 && TileAt(a.Location).Id == Elements.BulletId);
                            if (bulletCount < Shots)
                            {
                                if (SpawnProjectile(Elements.BulletId, actor.Location, KeyVector, false))
                                {
                                    Ammo--;
                                    UpdateStatus();
                                    PlaySound(2, Sounds.Shoot);
                                }
                            }
                        }
                        else
                        {
                            if (Alerts.OutOfAmmo)
                            {
                                SetMessage(0xC8, Alerts.NoAmmoMessage);
                                Alerts.OutOfAmmo = false;
                            }
                        }
                    }
                    else
                    {
                        if (Alerts.CantShootHere)
                        {
                            SetMessage(0xC8, Alerts.NoShootMessage);
                            Alerts.CantShootHere = false;
                        }
                    }
                }
                else
                {
                    // Movement logic

                    ElementAt(actor.Location.Sum(KeyVector)).Interact(actor.Location.Sum(KeyVector), 0, KeyVector);
                    if (!KeyVector.IsZero())
                    {
                        if (!SoundPlaying)
                        {
                            Speaker.PlayDrum(3);
                        }
                        if (ElementAt(actor.Location.Sum(KeyVector)).IsFloor)
                        {
                            MoveActor(0, actor.Location.Sum(KeyVector));
                        }
                    }
                }
            }

            // Hotkey logic

            switch (KeyPressed.ToUpperCase())
            {
                case 0x54: // T
                    if (TorchesEnabled)
                    {
                        if (TorchCycles <= 0)
                        {
                            if (Torches <= 0)
                            {
                                if (Alerts.NoTorches)
                                {
                                    SetMessage(0xC8, Alerts.NoTorchMessage);
                                    Alerts.NoTorches = false;
                                }
                            }
                            else if (!Dark)
                            {
                                if (Alerts.NotDark)
                                {
                                    SetMessage(0xC8, Alerts.NotDarkMessage);
                                    Alerts.NotDark = false;
                                }
                            }
                            else
                            {
                                Torches--;
                                TorchCycles = 0xC8;
                                UpdateRadius(actor.Location, RadiusMode.Update);
                                UpdateStatus();
                            }
                        }
                    }
                    break;
                case 0x51: // Q
                case 0x1B: // escape
                    BreakGameLoop = GameOver || Hud.EndGameConfirmation();
                    break;
                case 0x53: // S
                    break;
                case 0x50: // P
                    if (Health > 0)
                    {
                        GamePaused = true;
                    }
                    break;
                case 0x42: // B
                    GameQuiet = !GameQuiet;
                    ClearSound();
                    UpdateStatus();
                    KeyPressed = 0x20;
                    break;
                case 0x48: // H
                    ShowInGameHelp();
                    break;
                case 0x46: // F
                    break;
                case 0x3F: // ?
                    break;
            }

            // Torch logic

            if (TorchCycles > 0)
            {
                TorchCycles--;
                if (TorchCycles <= 0)
                {
                    UpdateRadius(actor.Location, RadiusMode.Update);
                    PlaySound(3, Sounds.TorchOut);
                }

                if (TorchCycles%40 == 0)
                {
                    UpdateStatus();
                }
            }

            // Energizer logic

            if (EnergyCycles > 0)
            {
                EnergyCycles--;
                if (EnergyCycles == 10)
                {
                    PlaySound(9, Sounds.EnergyOut);
                }
                else if (EnergyCycles <= 0)
                {
                    ForcePlayerColor(index);
                }
            }

            // Time limit logic

            if (TimeLimit > 0)
            {
                if (Health > 0)
                {
                    if (GetPlayerTimeElapsed(100))
                    {
                        TimePassed++;
                        if (TimeLimit - 10 == TimePassed)
                        {
                            SetMessage(0xC8, Alerts.TimeMessage);
                            PlaySound(3, Sounds.TimeLow);
                        }
                        else if (TimePassed >= TimeLimit)
                        {
                            Harm(0);
                        }
                        UpdateStatus();
                    }
                }
            }
        }

        public void ActPusher(int index)
        {
            var actor = Actors[index];
            var source = actor.Location.Clone();

            if (!ElementAt(actor.Location.Sum(actor.Vector)).IsFloor)
            {
                Push(actor.Location.Sum(actor.Vector), actor.Vector);
            }

            index = ActorIndexAt(source);
            actor = Actors[index];
            if (!ElementAt(actor.Location.Sum(actor.Vector)).IsFloor) return;

            MoveActor(index, actor.Location.Sum(actor.Vector));
            PlaySound(2, Sounds.Push);
            var behindLocation = actor.Location.Difference(actor.Vector);
            if (TileAt(behindLocation).Id != Elements.PusherId) return;

            var behindIndex = ActorIndexAt(behindLocation);
            var behindActor = Actors[behindIndex];
            if (behindActor.Vector.X == actor.Vector.X && behindActor.Vector.Y == actor.Vector.Y)
            {
                Elements.PusherElement.Act(behindIndex);
            }
        }

        public void ActRoton(int index)
        {
            var actor = Actors[index];

            actor.P3--;
            if (actor.P3 < -actor.P2%10)
            {
                actor.P3 = actor.P2*10 + RandomNumberDeterministic(10);
            }

            Seek(actor.Location, actor.Vector);
            if (actor.P1 <= RandomNumberDeterministic(10))
            {
                var temp = actor.Vector.X;
                actor.Vector.X = -actor.P2.Polarity()*actor.Vector.Y;
                actor.Vector.Y = actor.P2.Polarity()*temp;
            }

            var target = actor.Location.Sum(actor.Vector);
            if (ElementAt(target).IsFloor)
            {
                MoveActor(index, target);
            }
            else if (TileAt(target).Id == Elements.PlayerId)
            {
                Attack(index, target);
            }
        }

        public void ActRuffian(int index)
        {
            var actor = Actors[index];

            if (actor.Vector.IsZero())
            {
                if (actor.P2 + 8 <= RandomNumberDeterministic(17))
                {
                    if (actor.P1 >= RandomNumberDeterministic(9))
                    {
                        Seek(actor.Location, actor.Vector);
                    }
                    else
                    {
                        Rnd(actor.Vector);
                    }
                }
            }
            else
            {
                if (actor.Location.X == Player.Location.X || actor.Location.Y == Player.Location.Y)
                {
                    if (actor.P1 >= RandomNumberDeterministic(9))
                    {
                        Seek(actor.Location, actor.Vector);
                    }
                }

                var target = actor.Location.Sum(actor.Vector);
                if (ElementAt(target).Id == Elements.PlayerId)
                {
                    Attack(index, target);
                }
                else if (ElementAt(target).IsFloor)
                {
                    MoveActor(index, target);
                    if (actor.P2 + 8 <= RandomNumberDeterministic(17))
                    {
                        actor.Vector.SetTo(0, 0);
                    }
                }
                else
                {
                    actor.Vector.SetTo(0, 0);
                }
            }
        }

        public void ActScroll(int index)
        {
            var actor = Actors[index];
            var color = TileAt(actor.Location).Color;

            color++;
            if (color > 0x0F)
            {
                color = 0x09;
            }
            TileAt(actor.Location).Color = color;
            UpdateBoard(actor.Location);
        }

        public void ActSegment(int index)
        {
            var actor = Actors[index];
            if (actor.Leader < 0)
            {
                if (actor.Leader < -1)
                {
                    TileAt(actor.Location).Id = Elements.HeadId;
                }
                else
                {
                    actor.Leader--;
                }
            }
        }

        public void ActShark(int index)
        {
            var actor = Actors[index];
            var vector = new Vector();

            if (actor.P1 > RandomNumberDeterministic(10))
            {
                Seek(actor.Location, vector);
            }
            else
            {
                Rnd(vector);
            }

            var target = actor.Location.Sum(vector);
            var targetElement = ElementAt(target);

            if (targetElement.Id == Elements.WaterId)
            {
                MoveActor(index, target);
            }
            else if (targetElement.Id == Elements.PlayerId)
            {
                Attack(index, target);
            }
        }

        public void ActSlime(int index)
        {
            var actor = Actors[index];

            if (actor.P1 >= actor.P2)
            {
                var spawnCount = 0;
                var color = TileAt(actor.Location).Color;
                var slimeElement = Elements.SlimeElement;
                var slimeTrailTile = new Tile(Elements.BreakableId, color);
                var source = actor.Location.Clone();
                actor.P1 = 0;

                for (var i = 0; i < 4; i++)
                {
                    var target = source.Sum(GetVector4(i));
                    if (ElementAt(target).IsFloor)
                    {
                        if (spawnCount == 0)
                        {
                            MoveActor(index, target);
                            TileAt(source).CopyFrom(slimeTrailTile);
                            UpdateBoard(source);
                        }
                        else
                        {
                            SpawnActor(target, new Tile(Elements.SlimeId, color), slimeElement.Cycle, null);
                            Actors[ActorCount].P2 = actor.P2;
                        }
                        spawnCount++;
                    }
                }

                if (spawnCount == 0)
                {
                    RemoveActor(index);
                    TileAt(source).CopyFrom(slimeTrailTile);
                    UpdateBoard(source);
                }
            }
            else
            {
                actor.P1++;
            }
        }

        public void ActSpider(int index)
        {
            var actor = Actors[index];
            var vector = new Vector();

            if (actor.P1 <= RandomNumberDeterministic(10))
            {
                Rnd(vector);
            }
            else
            {
                Seek(actor.Location, vector);
            }

            if (!ActSpiderAttemptDirection(index, vector))
            {
                var i = (RandomNumberDeterministic(2) << 1) - 1;
                if (!ActSpiderAttemptDirection(index, vector.Product(i).Swap()))
                {
                    if (!ActSpiderAttemptDirection(index, vector.Product(i).Swap().Opposite()))
                    {
                        ActSpiderAttemptDirection(index, vector.Opposite());
                    }
                }
            }
        }

        public void ActSpinningGun(int index)
        {
            var actor = Actors[index];
            var firingElement = Elements.BulletId;
            var shot = false;

            UpdateBoard(actor.Location);

            if (actor.P2 >= 0x80)
            {
                firingElement = Elements.StarId;
            }

            if ((actor.P2 & 0x7F) > RandomNumberDeterministic(9))
            {
                if (actor.P1 >= RandomNumberDeterministic(9))
                {
                    if (actor.Location.X.AbsDiff(Player.Location.X) <= 2)
                    {
                        shot = SpawnProjectile(firingElement, actor.Location,
                            new Vector(0, (Player.Location.Y - actor.Location.Y).Polarity()), true);
                    }
                    if (!shot && actor.Location.Y.AbsDiff(Player.Location.Y) <= 2)
                    {
                        SpawnProjectile(firingElement, actor.Location,
                            new Vector((Player.Location.X - actor.Location.X).Polarity(), 0), true);
                    }
                }
                else
                {
                    SpawnProjectile(firingElement, actor.Location, Rnd(), true);
                }
            }
        }

        public void ActStar(int index)
        {
            var actor = Actors[index];

            actor.P2 = (actor.P2 - 1) & 0xFF;
            if (actor.P2 > 0)
            {
                if ((actor.P2 & 1) == 0)
                {
                    Seek(actor.Location, actor.Vector);
                    var targetLocation = actor.Location.Sum(actor.Vector);
                    var targetElement = ElementAt(targetLocation);

                    if (targetElement.Id == Elements.PlayerId || targetElement.Id == Elements.BreakableId)
                    {
                        Attack(index, targetLocation);
                    }
                    else
                    {
                        if (!targetElement.IsFloor)
                        {
                            Push(targetLocation, actor.Vector);
                        }
                        if (targetElement.IsFloor || targetElement.Id == Elements.WaterId)
                        {
                            MoveActor(index, targetLocation);
                        }
                    }
                }
                else
                {
                    UpdateBoard(actor.Location);
                }
            }
            else
            {
                RemoveActor(index);
            }
        }

        public void ActStone(int index)
        {
            UpdateBoard(Actors[index].Location);
        }

        public void ActTiger(int index)
        {
            var actor = Actors[index];
            var firingElement = Elements.BulletId;

            if (actor.P2 >= 0x80)
            {
                firingElement = Elements.StarId;
            }

            if ((actor.P2 & 0x7F) > 3*RandomNumberDeterministic(10))
            {
                var shot = actor.Location.X.AbsDiff(Player.Location.X) <= 2 &&
                           SpawnProjectile(firingElement, actor.Location,
                               new Vector(0, (Player.Location.Y - actor.Location.Y).Polarity()), true);

                if (!shot && actor.Location.Y.AbsDiff(Player.Location.Y) <= 2)
                {
                    SpawnProjectile(firingElement, actor.Location,
                        new Vector((Player.Location.X - actor.Location.X).Polarity(), 0), true);
                }
            }

            ActLion(index);
        }

        public void ActTransporter(int index)
        {
            UpdateBoard(Actors[index].Location);
        }

        public IAlerts Alerts => StateData.Alerts;

        public void Attack(int index, IXyPair location)
        {
            if (index == 0 && EnergyCycles > 0)
            {
                Score += Elements[Tiles[location].Id].Points;
                UpdateStatus();
            }
            else
            {
                Harm(index);
            }

            if (index > 0 && index <= ActIndex)
            {
                ActIndex--;
            }

            if (Tiles[location].Id == Elements.PlayerId && EnergyCycles > 0)
            {
                Score += Elements[Tiles[Actors[index].Location].Id].Points;
                UpdateStatus();
            }
            else
            {
                Destroy(location);
                PlaySound(2, Sounds.EnemySuicide);
            }
        }

        public int Board
        {
            get { return WorldData.Board; }
            set { WorldData.Board = value; }
        }

        public int BoardCount
        {
            get { return StateData.BoardCount; }
            set { StateData.BoardCount = value; }
        }

        public abstract IBoard BoardData { get; }

        public string BoardName
        {
            get { return BoardData.Name; }
            set { BoardData.Name = value; }
        }

        public IList<IPackedBoard> Boards { get; }

        public ITile BorderTile => StateData.BorderTile;

        public bool BreakGameLoop
        {
            get { return StateData.BreakGameLoop; }
            set { StateData.BreakGameLoop = value; }
        }

        public bool CancelScroll
        {
            get { return StateData.CancelScroll; }
            set { StateData.CancelScroll = value; }
        }

        public void ClearBoard()
        {
            var emptyId = Elements.EmptyId;
            var boardEdgeId = EdgeTile.Id;
            var boardBorderId = BorderTile.Id;
            var boardBorderColor = BorderTile.Color;

            // board properties
            BoardName = "";
            Message = "";
            Shots = 0xFF;
            Dark = false;
            RestartOnZap = false;
            TimeLimit = 0;
            ExitEast = 0;
            ExitNorth = 0;
            ExitSouth = 0;
            ExitWest = 0;

            // build board edges
            for (var y = 0; y <= Tiles.Height + 1; y++)
            {
                TileAt(0, y).Id = boardEdgeId;
                TileAt(Width + 1, y).Id = boardEdgeId;
            }
            for (var x = 0; x <= Width + 1; x++)
            {
                TileAt(x, 0).Id = boardEdgeId;
                TileAt(x, Height + 1).Id = boardEdgeId;
            }

            // clear out board
            for (var x = 1; x <= Width; x++)
            {
                for (var y = 1; y <= Height; y++)
                {
                    TileAt(x, y).SetTo(emptyId, 0);
                }
            }

            // build border
            for (var y = 1; y <= Height; y++)
            {
                TileAt(1, y).SetTo(boardBorderId, boardBorderColor);
                TileAt(Width, y).SetTo(boardBorderId, boardBorderColor);
            }
            for (var x = 1; x <= Width; x++)
            {
                TileAt(x, 1).SetTo(boardBorderId, boardBorderColor);
                TileAt(x, Height).SetTo(boardBorderId, boardBorderColor);
            }

            // generate player actor
            var element = Elements.PlayerElement;
            ActorCount = 0;
            Player.Location.SetTo(Width/2, Height/2);
            TileAt(Player.Location).SetTo(element.Id, element.Color);
            Player.Cycle = 1;
            Player.UnderTile.SetTo(0, 0);
            Player.Pointer = 0;
            Player.Length = 0;
        }

        public void ClearSound()
        {
            SoundPlaying = false;
            StopSound();
        }

        public void ClearWorld()
        {
            BoardCount = 0;
            Boards.Clear();
            ResetAlerts();
            ClearBoard();
            Boards.Add(new PackedBoard(Serializer.PackBoard(Tiles)));
            Board = 0;
            Ammo = 0;
            Gems = 0;
            Health = 100;
            EnergyCycles = 0;
            Torches = 0;
            TorchCycles = 0;
            Score = 0;
            TimePassed = 0;
            Stones = -1;
            Keys.Clear();
            Flags.Clear();
            SetBoard(0);
            BoardName = "Introduction screen";
            WorldFileName = "";
            WorldName = "";
        }

        public IColorList Colors => StateData.Colors;

        public void Convey(IXyPair center, int direction)
        {
            int beginIndex;
            int endIndex;

            var surrounding = new ITile[8];

            if (direction == 1)
            {
                beginIndex = 0;
                endIndex = 8;
            }
            else
            {
                beginIndex = 7;
                endIndex = -1;
            }

            var pushable = true;
            for (var i = beginIndex; i != endIndex; i += direction)
            {
                surrounding[i] = TileAt(center.Sum(GetConveyorVector(i))).Clone();
                var element = Elements[surrounding[i].Id];
                if (element.Id == Elements.EmptyId)
                    pushable = true;
                else if (!element.IsPushable)
                    pushable = false;
            }

            for (var i = beginIndex; i != endIndex; i += direction)
            {
                var element = Elements[surrounding[i].Id];

                if (pushable)
                {
                    if (element.IsPushable)
                    {
                        var source = center.Sum(GetConveyorVector(i));
                        var target = center.Sum(GetConveyorVector((i + 8 - direction)%8));
                        if (element.Cycle > -1)
                        {
                            var tile = TileAt(source);
                            var index = ActorIndexAt(source);
                            TileAt(source).CopyFrom(surrounding[i]);
                            TileAt(target).Id = Elements.EmptyId;
                            MoveActor(index, target);
                            TileAt(source).CopyFrom(tile);
                        }
                        else
                        {
                            TileAt(target).CopyFrom(surrounding[i]);
                            UpdateBoard(target);
                        }

                        if (!Elements[surrounding[(i + 8 + direction)%8].Id].IsPushable)
                        {
                            TileAt(source).Id = Elements.EmptyId;
                            UpdateBoard(source);
                        }
                    }
                    else
                    {
                        pushable = false;
                    }
                }
                else
                {
                    if (element.Id == Elements.EmptyId)
                        pushable = true;
                    else if (!element.IsPushable)
                        pushable = false;
                }
            }
        }

        public bool Dark
        {
            get { return BoardData.Dark; }
            set { BoardData.Dark = value; }
        }

        public IActor DefaultActor => StateData.DefaultActor;

        public string DefaultBoardName
        {
            get { return StateData.DefaultBoardName; }
            set { StateData.DefaultBoardName = value; }
        }

        public string DefaultSaveName
        {
            get { return StateData.DefaultSaveName; }
            set { StateData.DefaultSaveName = value; }
        }

        public string DefaultWorldName
        {
            get { return StateData.DefaultWorldName; }
            set { StateData.DefaultWorldName = value; }
        }

        public void Destroy(IXyPair location)
        {
            var index = ActorIndexAt(location);
            if (index == -1)
            {
                RemoveItem(location);
            }
            else
            {
                Harm(index);
            }
        }

        public IFileSystem Disk { get; set; }

        public AnsiChar DrawBlinkWall(IXyPair location)
        {
            return new AnsiChar(0xCE, Tiles[location].Color);
        }

        public AnsiChar DrawBomb(IXyPair location)
        {
            var p1 = Actors[ActorIndexAt(location)].P1;
            return new AnsiChar(p1 > 1 ? 0x30 + p1 : 0x0B, Tiles[location].Color);
        }

        public AnsiChar DrawClockwise(IXyPair location)
        {
            switch ((GameCycle/Elements[Elements.ClockwiseId].Cycle) & 0x3)
            {
                case 0:
                    return new AnsiChar(0xB3, Tiles[location].Color);
                case 1:
                    return new AnsiChar(0x2F, Tiles[location].Color);
                case 2:
                    return new AnsiChar(0xC4, Tiles[location].Color);
                default:
                    return new AnsiChar(0x5C, Tiles[location].Color);
            }
        }

        public AnsiChar DrawCounter(IXyPair location)
        {
            switch ((GameCycle/Elements[Elements.CounterId].Cycle) & 0x3)
            {
                case 3:
                    return new AnsiChar(0xB3, Tiles[location].Color);
                case 2:
                    return new AnsiChar(0x2F, Tiles[location].Color);
                case 1:
                    return new AnsiChar(0xC4, Tiles[location].Color);
                default:
                    return new AnsiChar(0x5C, Tiles[location].Color);
            }
        }

        public AnsiChar DrawDragonPup(IXyPair location)
        {
            switch (GameCycle & 0x3)
            {
                case 0:
                case 2:
                    return new AnsiChar(0x94, Tiles[location].Color);
                case 1:
                    return new AnsiChar(0xA2, Tiles[location].Color);
                default:
                    return new AnsiChar(0x95, Tiles[location].Color);
            }
        }

        public AnsiChar DrawDuplicator(IXyPair location)
        {
            switch (Actors[ActorIndexAt(location)].P1)
            {
                case 2:
                    return new AnsiChar(0xF9, Tiles[location].Color);
                case 3:
                    return new AnsiChar(0xF8, Tiles[location].Color);
                case 4:
                    return new AnsiChar(0x6F, Tiles[location].Color);
                case 5:
                    return new AnsiChar(0x4F, Tiles[location].Color);
                default:
                    return new AnsiChar(0xFA, Tiles[location].Color);
            }
        }

        public AnsiChar DrawLine(IXyPair location)
        {
            return new AnsiChar(LineChars[Adjacent(location, Elements.LineId)], Tiles[location].Color);
        }

        public AnsiChar DrawObject(IXyPair location)
        {
            return new AnsiChar(Actors[ActorIndexAt(location)].P1, Tiles[location].Color);
        }

        public AnsiChar DrawPusher(IXyPair location)
        {
            var actor = Actors[ActorIndexAt(location)];
            switch (actor.Vector.X)
            {
                case 1:
                    return new AnsiChar(0x10, Tiles[location].Color);
                case -1:
                    return new AnsiChar(0x11, Tiles[location].Color);
                default:
                    return actor.Vector.Y == -1
                        ? new AnsiChar(0x1E, Tiles[location].Color)
                        : new AnsiChar(0x1F, Tiles[location].Color);
            }
        }

        public AnsiChar DrawSpinningGun(IXyPair location)
        {
            switch (GameCycle & 0x7)
            {
                case 0:
                case 1:
                    return new AnsiChar(0x18, Tiles[location].Color);
                case 2:
                case 3:
                    return new AnsiChar(0x1A, Tiles[location].Color);
                case 4:
                case 5:
                    return new AnsiChar(0x19, Tiles[location].Color);
                default:
                    return new AnsiChar(0x1B, Tiles[location].Color);
            }
        }

        public AnsiChar DrawStar(IXyPair location)
        {
            var tile = Tiles[location];
            tile.Color++;
            if (tile.Color > 15)
                tile.Color = 9;
            return new AnsiChar(StarChars[GameCycle & 0x3], tile.Color);
        }

        public AnsiChar DrawStone(IXyPair location)
        {
            return new AnsiChar(0x41 + RandomNumber(0x1A), Tiles[location].Color);
        }

        public AnsiChar DrawTransporter(IXyPair location)
        {
            var actor = Actors[ActorIndexAt(location)];
            int index;

            if (actor.Vector.X == 0)
            {
                if (actor.Cycle > 0)
                    index = (GameCycle/actor.Cycle) & 0x3;
                else
                    index = 0;
                index += (actor.Vector.Y << 1) + 2;
                return new AnsiChar(TransporterVChars[index], Tiles[location].Color);
            }
            if (actor.Cycle > 0)
                index = (GameCycle/actor.Cycle) & 0x3;
            else
                index = 0;
            index += (actor.Vector.X << 1) + 2;
            return new AnsiChar(TransporterHChars[index], Tiles[location].Color);
        }

        public AnsiChar DrawWeb(IXyPair location)
        {
            return new AnsiChar(WebChars[Adjacent(location, Elements.WebId)], Tiles[location].Color);
        }

        public ITile EdgeTile => StateData.EdgeTile;

        public bool EditorMode
        {
            get { return StateData.EditorMode; }
            set { StateData.EditorMode = value; }
        }

        public IElement ElementAt(IXyPair location)
        {
            return Elements[TileAt(location).Id];
        }

        public IXyPair Enter
        {
            get { return BoardData.Enter; }
            set { BoardData.Enter.CopyFrom(value); }
        }

        public int ExitEast
        {
            get { return BoardData.ExitEast; }
            set { BoardData.ExitEast = value; }
        }

        public int ExitNorth
        {
            get { return BoardData.ExitNorth; }
            set { BoardData.ExitNorth = value; }
        }

        public int ExitSouth
        {
            get { return BoardData.ExitSouth; }
            set { BoardData.ExitSouth = value; }
        }

        public int ExitWest
        {
            get { return BoardData.ExitWest; }
            set { BoardData.ExitWest = value; }
        }

        public IFlagList Flags => WorldData.Flags;

        public int GameCycle
        {
            get { return StateData.GameCycle; }
            set { StateData.GameCycle = value; }
        }

        public bool GameOver
        {
            get { return StateData.GameOver; }
            set { StateData.GameOver = value; }
        }

        public bool GamePaused
        {
            get { return StateData.GamePaused; }
            set { StateData.GamePaused = value; }
        }

        public bool GameQuiet
        {
            get { return StateData.GameQuiet; }
            set { StateData.GameQuiet = value; }
        }

        public int GameWaitTime
        {
            get { return StateData.GameWaitTime; }
            set { StateData.GameWaitTime = value; }
        }

        public IXyPair GetCardinalVector(int index)
        {
            return new Vector(Vector4[index], Vector4[index + 4]);
        }

        public IXyPair GetConveyorVector(int index)
        {
            return new Vector(Vector8[index], Vector8[index + 8]);
        }

        public abstract IGrammar Grammar { get; }

        public abstract IHud Hud { get; }

        public bool Init
        {
            get { return StateData.Init; }
            set { StateData.Init = value; }
        }

        public virtual void InteractAmmo(IXyPair location, int index, IXyPair vector)
        {
            // Ammo gain is implemented per engine, do it before this
            RemoveItem(location);
            UpdateStatus();
            PlaySound(2, Sounds.Ammo);
            if (Alerts.AmmoPickup)
            {
                SetMessage(0xC8, Alerts.AmmoMessage);
                Alerts.AmmoPickup = false;
            }
        }

        public void InteractBoardEdge(IXyPair location, int index, IXyPair vector)
        {
            var target = location.Clone();
            int targetBoard;
            var oldBoard = Board;

            switch (vector.Y)
            {
                case -1:
                    targetBoard = ExitNorth;
                    target.Y = Height;
                    break;
                case 1:
                    targetBoard = ExitSouth;
                    target.Y = 1;
                    break;
                default:
                    if (vector.X == -1)
                    {
                        targetBoard = ExitWest;
                        target.X = Width;
                    }
                    else
                    {
                        targetBoard = ExitEast;
                        target.X = 1;
                    }
                    break;
            }

            if (targetBoard != 0)
            {
                SetBoard(targetBoard);
                if (TileAt(target).Id != Elements.PlayerId)
                {
                    ElementAt(target).Interact(target, index, KeyVector);
                }
                if (ElementAt(target).IsFloor || ElementAt(target).Id == Elements.PlayerId)
                {
                    if (ElementAt(target).Id != Elements.PlayerId)
                    {
                        MoveActor(0, target);
                    }
                    FadePurple();
                    vector.SetTo(0, 0);
                    EnterBoard();
                }
                else
                {
                    SetBoard(oldBoard);
                }
            }
        }

        public void InteractBomb(IXyPair location, int index, IXyPair vector)
        {
            var actor = ActorAt(location);
            if (actor.P1 == 0)
            {
                actor.P1 = 9;
                UpdateBoard(location);
                SetMessage(0xC8, Alerts.BombMessage);
                PlaySound(4, Sounds.BombActivate);
            }
            else
            {
                Push(location, vector);
            }
        }

        public void InteractDoor(IXyPair location, int index, IXyPair vector)
        {
            var color = (TileAt(location).Color & 0x70) >> 4;
            var keyIndex = color - 1;
            if (!Keys[keyIndex])
            {
                SetMessage(0xC8, Alerts.DoorLockedMessage(color));
                PlaySound(3, Sounds.DoorLocked);
            }
            else
            {
                Keys[keyIndex] = false;
                RemoveItem(location);
                SetMessage(0xC8, Alerts.DoorOpenMessage(color));
                PlaySound(3, Sounds.DoorOpen);
            }
        }

        public void InteractEnemy(IXyPair location, int index, IXyPair vector)
        {
            Attack(index, location);
        }

        public void InteractEnergizer(IXyPair location, int index, IXyPair vector)
        {
            PlaySound(9, Sounds.Energizer);
            RemoveItem(location);
            EnergyCycles = 0x4B;
            UpdateStatus();
            UpdateBoard(location);
            if (Alerts.EnergizerPickup)
            {
                Alerts.EnergizerPickup = false;
                SetMessage(0xC8, Alerts.EnergizerMessage);
            }
            BroadcastLabel(0, @"ALL:ENERGIZE", false);
        }

        public void InteractFake(IXyPair location, int index, IXyPair vector)
        {
            if (!Alerts.FakeWall) return;

            Alerts.FakeWall = false;
            SetMessage(0xC8, Alerts.FakeMessage);
        }

        public virtual void InteractForest(IXyPair location, int index, IXyPair vector)
        {
            RemoveItem(location);
            UpdateBoard(location);
            PlaySound(3, Sounds.Forest);
            if (Alerts.Forest)
            {
                SetMessage(0xC8, Alerts.ForestMessage);
                Alerts.Forest = false;
            }
        }

        public virtual void InteractGem(IXyPair location, int index, IXyPair vector)
        {
            // Health gain is implemented per engine, do it before this
            Gems += 1;
            Score += 10;
            RemoveItem(location);
            UpdateStatus();
            PlaySound(2, Sounds.Gem);
            if (Alerts.GemPickup)
            {
                SetMessage(0xC8, Alerts.GemMessage);
                Alerts.GemPickup = false;
            }
        }

        public void InteractInvisible(IXyPair location, int index, IXyPair vector)
        {
            TileAt(location).Id = Elements.NormalId;
            UpdateBoard(location);
            PlaySound(3, Sounds.Invisible);
            SetMessage(0x64, Alerts.InvisibleMessage);
        }

        public void InteractKey(IXyPair location, int index, IXyPair vector)
        {
            var color = TileAt(location).Color & 0x07;
            var keyIndex = color - 1;
            if (Keys[keyIndex])
            {
                SetMessage(0xC8, Alerts.KeyAlreadyMessage(color));
                PlaySound(2, Sounds.KeyAlready);
            }
            else
            {
                Keys[keyIndex] = true;
                RemoveItem(location);
                SetMessage(0xC8, Alerts.KeyPickupMessage(color));
                PlaySound(2, Sounds.Key);
            }
        }

        public void InteractObject(IXyPair location, int index, IXyPair vector)
        {
            var objectIndex = ActorIndexAt(location);
            BroadcastLabel(-objectIndex, @"TOUCH", false);
        }

        public void InteractPassage(IXyPair location, int index, IXyPair vector)
        {
            ExecutePassage(location);
            vector.SetTo(0, 0);
        }

        public void InteractPushable(IXyPair location, int index, IXyPair vector)
        {
            Push(location, vector);
            PlaySound(2, Sounds.Push);
        }

        public void InteractScroll(IXyPair location, int index, IXyPair vector)
        {
            var scrollIndex = ActorIndexAt(location);
            var actor = Actors[scrollIndex];

            PlaySound(2, PlayMusic(@"c-c+d-d+e-e+f-f+g-g"));
            ExecuteCode(scrollIndex, actor, @"Scroll");
            RemoveActor(scrollIndex);
        }

        public void InteractSlime(IXyPair location, int index, IXyPair vector)
        {
            var color = TileAt(location).Color;
            var slimeIndex = ActorIndexAt(location);
            Harm(slimeIndex);
            TileAt(location).SetTo(Elements.BreakableId, color);
            UpdateBoard(location);
            PlaySound(2, Sounds.SlimeDie);
        }

        public void InteractStone(IXyPair location, int index, IXyPair vector)
        {
            if (Stones < 0)
            {
                Stones = 0;
            }
            Stones++;
            Destroy(location);
            UpdateStatus();
            SetMessage(0xC8, Alerts.StoneMessage);
        }

        public void InteractTorch(IXyPair location, int index, IXyPair vector)
        {
            Torches++;
            RemoveItem(location);
            UpdateStatus();
            if (Alerts.TorchPickup)
            {
                SetMessage(0xC8, Alerts.TorchMessage);
                Alerts.TorchPickup = false;
            }
            PlaySound(3, Sounds.Torch);
        }

        public void InteractTransporter(IXyPair location, int index, IXyPair vector)
        {
            PushThroughTransporter(location.Difference(vector), vector);
            vector.SetTo(0, 0);
        }

        public void InteractWater(IXyPair location, int index, IXyPair vector)
        {
            PlaySound(3, Sounds.Water);
            SetMessage(0x64, Alerts.WaterMessage);
        }

        public bool KeyArrow
        {
            get { return StateData.KeyArrow; }
            set { StateData.KeyArrow = value; }
        }

        public IKeyboard Keyboard => _config.Keyboard;

        public IXyPair KeyVector => StateData.KeyVector;

        public IList<int> LineChars => StateData.LineChars;

        public bool Locked
        {
            get { return WorldData.Locked; }
            set { WorldData.Locked = value; }
        }

        public int MainTime
        {
            get { return StateData.MainTime; }
            set { StateData.MainTime = value; }
        }

        public IMemory Memory { get; }

        public void MoveActor(int index, IXyPair target)
        {
            var actor = Actors[index];
            var sourceLocation = actor.Location.Clone();
            var sourceTile = TileAt(actor.Location);
            var targetTile = TileAt(target);
            var underTile = actor.UnderTile.Clone();

            actor.UnderTile.CopyFrom(targetTile);
            if (targetTile.Id == Elements.EmptyId)
            {
                targetTile.SetTo(sourceTile.Id, sourceTile.Color & 0x0F);
            }
            else
            {
                targetTile.SetTo(sourceTile.Id, (targetTile.Color & 0x70) | (sourceTile.Color & 0x0F));
            }
            sourceTile.CopyFrom(underTile);
            actor.Location.CopyFrom(target);
            if (targetTile.Id == Elements.PlayerId)
            {
                ForcePlayerColor(index);
            }
            UpdateBoard(target);
            UpdateBoard(sourceLocation);
            if (index == 0 && Dark)
            {
                var squareDistanceX = (target.X - sourceLocation.X).Square();
                var squareDistanceY = (target.Y - sourceLocation.Y).Square();
                if (squareDistanceX + squareDistanceY == 1)
                {
                    var glowLocation = new Location();
                    for (var x = target.X - 11; x <= target.X + 11; x++)
                    {
                        for (var y = target.Y - 8; y <= target.Y + 8; y++)
                        {
                            glowLocation.SetTo(x, y);
                            if (glowLocation.X >= 1 && glowLocation.X <= Width && glowLocation.Y >= 1 &&
                                glowLocation.Y <= Height)
                            {
                                if ((Distance(sourceLocation, glowLocation) < 50) ^
                                    (Distance(target, glowLocation) < 50))
                                {
                                    UpdateBoard(glowLocation);
                                }
                            }
                        }
                    }
                }
            }
            if (index == 0)
            {
                UpdateCamera();
            }
        }

        public int OopByte
        {
            get { return StateData.OopByte; }
            set { StateData.OopByte = value; }
        }

        public int OopNumber
        {
            get { return StateData.OopNumber; }
            set { StateData.OopNumber = value; }
        }

        public string OopWord
        {
            get { return StateData.OopWord; }
            set { StateData.OopWord = value; }
        }

        public void PackBoard()
        {
            var board = new PackedBoard(Serializer.PackBoard(Tiles));
            Boards[Board] = board;
        }

        public int PlayerElement
        {
            get { return StateData.PlayerElement; }
            set { StateData.PlayerElement = value; }
        }

        public int PlayerTime
        {
            get { return StateData.PlayerTime; }
            set { StateData.PlayerTime = value; }
        }

        public void PlaySound(int priority, byte[] sound)
        {
        }

        public void Push(IXyPair location, IXyPair vector)
        {
            // this is here to prevent endless push loops
            // but doesn't exist in the original code
            if (vector.IsZero())
            {
                throw Exceptions.PushStackOverflow;
            }

            var tile = TileAt(location);
            if ((tile.Id == Elements.SliderEwId && vector.Y == 0) || (tile.Id == Elements.SliderNsId && vector.X == 0) ||
                Elements[tile.Id].IsPushable)
            {
                var furtherTile = TileAt(location.Sum(vector));
                if (furtherTile.Id == Elements.TransporterId)
                {
                    PushThroughTransporter(location, vector);
                }
                else if (furtherTile.Id != Elements.EmptyId)
                {
                    Push(location.Sum(vector), vector);
                }

                var furtherElement = Elements[furtherTile.Id];
                if (!furtherElement.IsFloor && furtherElement.IsDestructible && furtherTile.Id != Elements.PlayerId)
                {
                    Destroy(location.Sum(vector));
                }

                furtherElement = Elements[furtherTile.Id];
                if (furtherElement.IsFloor)
                {
                    MoveTile(location, location.Sum(vector));
                }
            }
        }

        public bool QuitZzt
        {
            get { return StateData.QuitZzt; }
            set { StateData.QuitZzt = value; }
        }

        public int RandomNumber(int max)
        {
            return Random.Next(max);
        }

        public int RandomNumberDeterministic(int max)
        {
            return RandomDeterministic.Next(max);
        }

        public int ReadActorCodeByte(int index, ICodeInstruction instructionSource)
        {
            var actor = Actors[index];
            var value = 0;

            if (instructionSource.Instruction < 0 || instructionSource.Instruction >= actor.Length)
            {
                OopByte = 0;
            }
            else
            {
                value = actor.Code[instructionSource.Instruction];
                OopByte = value;
                instructionSource.Instruction++;
            }
            return value;
        }

        public string ReadActorCodeLine(int index, ICodeInstruction instructionSource)
        {
            var result = new StringBuilder();
            ReadActorCodeByte(index, instructionSource);
            while (OopByte != 0x00 && OopByte != 0x0D)
            {
                result.Append(OopByte.ToChar());
                ReadActorCodeByte(index, instructionSource);
            }
            return result.ToString();
        }

        public int ReadActorCodeNumber(int index, ICodeInstruction instructionSource)
        {
            var result = new StringBuilder();
            var success = false;

            while (ReadActorCodeByte(index, instructionSource) == 0x20)
            {
            }

            OopByte = OopByte.ToUpperCase();
            while (OopByte >= 0x30 && OopByte <= 0x39)
            {
                success = true;
                result.Append(OopByte.ToChar());
                ReadActorCodeByte(index, instructionSource);
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            if (!success)
            {
                OopNumber = -1;
            }
            else
            {
                var resultInt = -1;
                int.TryParse(result.ToString(), out resultInt);
                OopNumber = resultInt;
            }

            return OopNumber;
        }

        public string ReadActorCodeWord(int index, ICodeInstruction instructionSource)
        {
            var result = new StringBuilder();

            while (true)
            {
                ReadActorCodeByte(index, instructionSource);
                if (OopByte != 0x20)
                {
                    break;
                }
            }

            OopByte = OopByte.ToUpperCase();

            if (!(OopByte >= 0x30 && OopByte <= 0x39))
            {
                while ((OopByte >= 0x41 && OopByte <= 0x5A) || (OopByte >= 0x30 && OopByte <= 0x39) || (OopByte == 0x3A) ||
                       (OopByte == 0x5F))
                {
                    result.Append(OopByte.ToChar());
                    ReadActorCodeByte(index, instructionSource);
                    OopByte = OopByte.ToUpperCase();
                }
            }

            if (instructionSource.Instruction > 0)
            {
                instructionSource.Instruction--;
            }

            OopWord = result.ToString();
            return OopWord;
        }

        public void RedrawBoard()
        {
            Hud.RedrawBoard();
        }

        public virtual void RemoveItem(IXyPair location)
        {
            TileAt(location).Id = Elements.EmptyId;
        }

        public bool RestartOnZap
        {
            get { return BoardData.RestartOnZap; }
            set { BoardData.RestartOnZap = value; }
        }

        public IXyPair Rnd()
        {
            var result = new Vector();
            Rnd(result);
            return result;
        }

        public IXyPair Seek(IXyPair location)
        {
            var result = new Vector();
            Seek(location, result);
            return result;
        }

        public abstract ISerializer Serializer { get; }

        public void SetBoard(int boardIndex)
        {
            var element = Elements.PlayerElement;
            TileAt(Player.Location).SetTo(element.Id, element.Color);
            PackBoard();
            UnpackBoard(boardIndex);
        }

        public void SetMessage(int duration, IMessage message)
        {
            var index = ActorIndexAt(new Location(0, 0));
            if (index >= 0)
            {
                RemoveActor(index);
                UpdateBorder();
            }

            var topMessage = message.Text[0];
            var bottomMessage = message.Text.Length > 1 ? message.Text[1] : string.Empty;

            if (!string.IsNullOrEmpty(topMessage))
            {
                SpawnActor(new Location(0, 0), new Tile(Elements.MessengerId, 0), 1, DefaultActor);
                Actors[ActorCount].P2 = duration/(GameWaitTime + 1);
            }

            Message = topMessage;
            Message2 = bottomMessage;
        }

        public int Shots
        {
            get { return BoardData.Shots; }
            set { BoardData.Shots = value; }
        }

        public IList<int> SoundBuffer => StateData.SoundBuffer;

        public bool SoundPlaying
        {
            get { return StateData.SoundPlaying; }
            set { StateData.SoundPlaying = value; }
        }

        public int SoundPriority
        {
            get { return StateData.SoundPriority; }
            set { StateData.SoundPriority = value; }
        }

        public abstract ISounds Sounds { get; }

        public int SoundTicks
        {
            get { return StateData.SoundTicks; }
            set { StateData.SoundTicks = value; }
        }

        public ISpeaker Speaker => _config.Speaker;

        public IList<int> StarChars => StateData.StarChars;

        public void Start()
        {
            if (!ThreadActive)
            {
                ThreadActive = true;
                Thread = new Thread(StartMain);
                TimerTick = CoreTimer.Tick;
                Thread.Start();
            }
        }

        public int StartBoard
        {
            get { return StateData.StartBoard; }
            set { StateData.StartBoard = value; }
        }

        public abstract IState StateData { get; }

        public abstract bool StonesEnabled { get; }

        public void Stop()
        {
            if (ThreadActive)
            {
                ThreadActive = false;
            }
        }

        public abstract ITileGrid Tiles { get; }

        public abstract bool TorchesEnabled { get; }

        public IList<int> TransporterHChars => StateData.TransporterHChars;

        public IList<int> TransporterVChars => StateData.TransporterVChars;

        public void UnpackBoard(int boardIndex)
        {
            Serializer.UnpackBoard(Tiles, Boards[boardIndex].Data);
            Board = boardIndex;
        }

        public void UpdateBoard(IXyPair location)
        {
            DrawTile(location, Draw(location));
        }

        public IList<int> Vector4 => StateData.Vector4;

        public IList<int> Vector8 => StateData.Vector8;

        public int VisibleTileCount
        {
            get { return StateData.VisibleTileCount; }
            set { StateData.VisibleTileCount = value; }
        }

        public IList<int> WebChars => StateData.WebChars;

        public abstract IWorld WorldData { get; }

        public string WorldFileName
        {
            get { return StateData.WorldFileName; }
            set { StateData.WorldFileName = value; }
        }

        public bool WorldLoaded
        {
            get { return StateData.WorldLoaded; }
            set { StateData.WorldLoaded = value; }
        }

        public int Ammo
        {
            get { return WorldData.Ammo; }
            set { WorldData.Ammo = value; }
        }

        public IXyPair Camera
        {
            get { return BoardData.Camera; }
            set { BoardData.Camera.CopyFrom(value); }
        }

        public virtual AnsiChar Draw(IXyPair location)
        {
            if (Dark && !ElementAt(location).IsAlwaysVisible &&
                (TorchCycles <= 0 || Distance(Player.Location, location) >= 50) && !EditorMode)
            {
                return new AnsiChar(0xB0, 0x07);
            }

            var tile = Tiles[location];
            var element = Elements[tile.Id];
            var elementCount = Elements.Count;

            if (tile.Id == Elements.EmptyId)
            {
                return new AnsiChar(0x20, 0x0F);
            }
            if (element.HasDrawCode)
            {
                return element.Draw(location);
            }
            if (tile.Id < elementCount - 7)
            {
                return new AnsiChar(element.Character, tile.Color);
            }
            if (tile.Id != elementCount - 1)
            {
                return new AnsiChar(tile.Color, ((tile.Id - (elementCount - 8)) << 4) | 0x0F);
            }
            return new AnsiChar(tile.Color, 0x0F);
        }

        public abstract IElementList Elements { get; }

        public int EnergyCycles
        {
            get { return WorldData.EnergyCycles; }
            set { WorldData.EnergyCycles = value; }
        }

        public int GameSpeed
        {
            get { return StateData.GameSpeed; }
            set { StateData.GameSpeed = value; }
        }

        public int Gems
        {
            get { return WorldData.Gems; }
            set { WorldData.Gems = value; }
        }

        public int Health
        {
            get { return WorldData.Health; }
            set { WorldData.Health = value; }
        }

        public virtual int Height => Tiles.Height;

        public int KeyPressed
        {
            get { return StateData.KeyPressed; }
            set { StateData.KeyPressed = value; }
        }

        public IKeyList Keys => WorldData.Keys;

        public bool KeyShift
        {
            get { return StateData.KeyShift; }
            set { StateData.KeyShift = value; }
        }

        public string Message
        {
            get { return StateData.Message; }
            set { StateData.Message = value; }
        }

        public string Message2
        {
            get { return StateData.Message2; }
            set { StateData.Message2 = value; }
        }

        public virtual IActor Player => Actors[0];

        public bool Quiet
        {
            get { return StateData.GameQuiet; }
            set { StateData.GameQuiet = value; }
        }

        public virtual void ReadInput()
        {
            KeyShift = false;
            KeyArrow = false;
            KeyPressed = 0;
            KeyVector.SetTo(0, 0);

            var key = Keyboard.GetKey();
            if (key >= 0)
            {
                KeyPressed = key;
                KeyShift = Keyboard.Shift;
                switch (key)
                {
                    case 0xCB:
                        KeyVector.CopyFrom(Vector.West);
                        KeyArrow = true;
                        break;
                    case 0xCD:
                        KeyVector.CopyFrom(Vector.East);
                        KeyArrow = true;
                        break;
                    case 0xC8:
                        KeyVector.CopyFrom(Vector.North);
                        KeyArrow = true;
                        break;
                    case 0xD0:
                        KeyVector.CopyFrom(Vector.South);
                        KeyArrow = true;
                        break;
                }
            }
        }

        public virtual int ReadKey()
        {
            var key = Keyboard.GetKey();
            KeyPressed = key > 0 ? key : 0;
            return KeyPressed;
        }

        public int Score
        {
            get { return WorldData.Score; }
            set { WorldData.Score = value; }
        }

        public int Stones
        {
            get { return WorldData.Stones; }
            set { WorldData.Stones = value; }
        }

        public virtual string StoneText
        {
            get
            {
                if (!StonesEnabled)
                    return string.Empty;

                foreach (var flag in Flags.Select(f => f.ToUpperInvariant()))
                {
                    if (flag.Length > 0 && flag.StartsWith("Z"))
                    {
                        return flag.Substring(1);
                    }
                }
                return string.Empty;
            }
        }

        public ITerminal Terminal => _config.Terminal;

        public int TimeLimit
        {
            get { return BoardData.TimeLimit; }
            set { BoardData.TimeLimit = value; }
        }

        public int TimePassed
        {
            get { return WorldData.TimePassed; }
            set { WorldData.TimePassed = value; }
        }

        public bool TitleScreen => PlayerElement != Elements.PlayerId;

        public int TorchCycles
        {
            get { return WorldData.TorchCycles; }
            set { WorldData.TorchCycles = value; }
        }

        public int Torches
        {
            get { return WorldData.Torches; }
            set { WorldData.Torches = value; }
        }

        public virtual void WaitForTick()
        {
            while (TimerTick == TimerBase && ThreadActive)
            {
                Thread.Sleep(1);
                //Thread.Sleep(0);
            }
            TimerTick++;
        }

        public virtual int Width => Tiles.Width;

        public string WorldName
        {
            get { return WorldData.Name; }
            set { WorldData.Name = value; }
        }

        internal virtual bool ActorIsLocked(int index)
        {
            return Actors[index].P2 != 0;
        }

        private bool ActSpiderAttemptDirection(int index, IXyPair vector)
        {
            var actor = Actors[index];
            var target = actor.Location.Sum(vector);
            var targetElement = ElementAt(target).Id;

            if (targetElement == Elements.WebId)
            {
                MoveActor(index, target);
                return true;
            }
            if (targetElement == Elements.PlayerId)
            {
                Attack(index, target);
                return true;
            }

            return false;
        }

        protected virtual int Adjacent(IXyPair location, int id)
        {
            return (location.Y <= 1 || Tiles[location.Sum(Vector.North)].Id == id ? 1 : 0) |
                   (location.Y >= Tiles.Height || Tiles[location.Sum(Vector.South)].Id == id ? 2 : 0) |
                   (location.X <= 1 || Tiles[location.Sum(Vector.West)].Id == id ? 4 : 0) |
                   (location.X >= Tiles.Width || Tiles[location.Sum(Vector.East)].Id == id ? 8 : 0);
        }

        internal virtual bool BroadcastLabel(int sender, string label, bool force)
        {
            var target = label;
            var external = false;
            var success = false;
            var index = 0;
            var offset = 0;

            if (sender < 0)
            {
                external = true;
                sender = -sender;
            }

            var info = new CodeSearchInfoProxy(
                () => index,
                value => { index = value; },
                () => target,
                value => { target = value; },
                () => offset,
                value => { offset = value; }
                );

            while (ExecuteLabel(sender, info, "\x000D:"))
            {
                if (!ActorIsLocked(index) || force || (sender == index && !external))
                {
                    if (sender == index)
                    {
                        success = true;
                    }
                    Actors[index].Instruction = offset;
                }
            }

            return success;
        }

        internal virtual void ClearFlag(string flag)
        {
            var index = Flags.IndexOf(flag);
            if (index >= 0)
            {
                Flags[index] = string.Empty;
            }
        }

        internal virtual int ColorMatch(ITile tile)
        {
            var element = Elements[tile.Id];

            if (element.Color < 0xF0)
                return element.Color & 7;
            if (element.Color == 0xFE)
                return ((tile.Color >> 4) & 0x0F) + 8;
            return tile.Color & 0x0F;
        }

        protected virtual int Distance(IXyPair a, IXyPair b)
        {
            return (a.Y - b.Y).Square()*2 + (a.X - b.X).Square();
        }

        protected void DrawChar(IXyPair location, AnsiChar ac)
        {
            Hud.DrawChar(location.X, location.Y, ac);
        }

        protected void DrawString(IXyPair location, string text, int color)
        {
            Hud.DrawString(location.X, location.Y, text, color);
        }

        protected void DrawTile(IXyPair location, AnsiChar ac)
        {
            Hud.DrawTile(location.X - 1, location.Y - 1, ac);
        }

        protected IElement ElementAt(int x, int y)
        {
            return ElementAt(new Location(x, y));
        }

        protected virtual void EnterBoard()
        {
            Enter.CopyFrom(Player.Location);
            if (Dark && Alerts.Dark)
            {
                SetMessage(0xC8, Alerts.DarkMessage);
                Alerts.Dark = false;
            }
            TimePassed = 0;
            UpdateStatus();
        }

        private void EnterHighScore(int score)
        {
        }

        internal virtual void ExecuteCode(int index, ICodeInstruction instructionSource, string name)
        {
            var context = new OopContext(index, instructionSource, name, this);
        }

        internal virtual bool ExecuteLabel(int sender, CodeSearchInfo search, string prefix)
        {
            var label = search.Label;
            var target = @"";
            var success = false;
            var split = label.IndexOf(':');

            if (split > 0)
            {
                target = label.Substring(0, split);
                label = label.Substring(split + 1);
                success = IsActorTargeted(sender, search, target);
            }
            else if (search.Index < sender)
            {
                search.Index = sender;
                split = 0;
                success = true;
            }
            while (true)
            {
                if (!success)
                {
                    break;
                }

                if (label.ToUpper() == @"RESTART")
                {
                    search.Offset = 0;
                }
                else
                {
                    search.Offset = SearchActorCode(search.Index, prefix + label);
                    if (search.Offset < 0 && split > 0)
                    {
                        success = IsActorTargeted(sender, search, target);
                        continue;
                    }
                }

                success = search.Offset >= 0;
                break;
            }
            return success;
        }

        private void ExecutePassage(IXyPair location)
        {
            var searchColor = TileAt(location).Color;
            var passageIndex = ActorIndexAt(location);
            var passageTarget = Actors[passageIndex].P3;
            SetBoard(passageTarget);
            var target = new Location();

            for (var x = 1; x <= Width; x++)
            {
                for (var y = 1; y <= Height; y++)
                {
                    if (TileAt(x, y).Id == Elements.PassageId)
                    {
                        if (TileAt(x, y).Color == searchColor)
                        {
                            target.SetTo(x, y);
                        }
                    }
                }
            }

            ExecutePassageCleanup();
            if (target.X != 0)
            {
                Player.Location.CopyFrom(target);
            }
            GamePaused = true;
            PlaySound(4, Sounds.Passage);
            FadePurple();
            EnterBoard();
        }

        protected virtual void ExecutePassageCleanup()
        {
            // this is what causes the black holes when using passages
            TileAt(Player.Location).SetTo(Elements.EmptyId, 0);
        }

        protected void FadeBoard(AnsiChar ac)
        {
            Hud.FadeBoard(ac);
        }

        protected void FadePurple()
        {
            FadeBoard(new AnsiChar(0xDB, 0x05));
            RedrawBoard();
        }

        protected void FadeRed()
        {
            FadeBoard(new AnsiChar(0xDB, 0x04));
            RedrawBoard();
        }

        protected virtual void ForcePlayerColor(int index)
        {
            var actor = Actors[index];
            if (TileAt(actor.Location).Color != Elements.PlayerElement.Color ||
                Elements.PlayerElement.Character != 0x02)
            {
                Elements.PlayerElement.Character = 2;
                TileAt(actor.Location).Color = Elements.PlayerElement.Color;
                UpdateBoard(actor.Location);
            }
        }

        private bool GetMainTimeElapsed(int interval)
        {
            return TimerTick%interval == 0;
            // TODO: Fix this for real.
            //var result = false;
            //while (GetTimeDifference(TimerTick, MainTime) > 0)
            //{
            //    result = true;
            //    MainTime = (MainTime + interval) & 0x7FFF;
            //}
            //return result;
        }

        private bool GetPlayerTimeElapsed(int interval)
        {
            var result = false;
            while (GetTimeDifference(TimerTick, PlayerTime) > 0)
            {
                result = true;
                PlayerTime = (PlayerTime + interval) & 0x7FFF;
            }
            return result;
        }

        private int GetTimeDifference(int now, int then)
        {
            now &= 0x7FFF;
            then &= 0x7FFF;
            if (now < 0x4000 && then >= 0x4000)
            {
                now += 0x8000;
            }
            return now - then;
        }

        protected virtual Vector GetVector4(int index)
        {
            return new Vector(Vector4[index], Vector4[index + 4]);
        }

        protected virtual Vector GetVector8(int index)
        {
            return new Vector(Vector8[index], Vector8[index + 8]);
        }

        protected virtual void Harm(int index)
        {
            var actor = Actors[index];
            if (index == 0)
            {
                if (Health > 0)
                {
                    Health -= 10;
                    UpdateStatus();
                    SetMessage(0x64, new Message(@"Ouch!"));
                    var color = TileAt(actor.Location).Color;
                    color &= 0x0F;
                    color |= 0x70;
                    TileAt(actor.Location).Color = color;
                    if (Health > 0)
                    {
                        TimePassed = 0;
                        if (RestartOnZap)
                        {
                            PlaySound(4, Sounds.TimeOut);
                            TileAt(actor.Location).Id = Elements.EmptyId;
                            UpdateBoard(actor.Location);
                            var oldLocation = actor.Location.Clone();
                            actor.Location.CopyFrom(Enter);
                            UpdateRadius(oldLocation, 0);
                            UpdateRadius(actor.Location, 0);
                            GamePaused = true;
                        }
                        PlaySound(4, Sounds.Ouch);
                    }
                    else
                    {
                        PlaySound(5, Sounds.GameOver);
                    }
                }
            }
            else
            {
                var element = TileAt(actor.Location).Id;
                if (element == Elements.BulletId)
                {
                    PlaySound(3, Sounds.BulletDie);
                }
                else if (element != Elements.ObjectId)
                {
                    PlaySound(3, Sounds.EnemyDie);
                }
                RemoveActor(index);
            }
        }

        protected virtual void InitializeElementDelegates()
        {
            foreach (var element in Elements)
            {
                if (element.Id == Elements.AmmoId)
                {
                    element.Interact = InteractAmmo;
                }
                else if (element.Id == Elements.BearId)
                {
                    element.Act = ActBear;
                    element.Interact = InteractEnemy;
                }
                else if (element.Id == Elements.BlinkWallId)
                {
                    element.Act = ActBlinkWall;
                    element.Draw = DrawBlinkWall;
                }
                else if (element.Id == Elements.BoardEdgeId)
                {
                    element.Interact = InteractBoardEdge;
                }
                else if (element.Id == Elements.BombId)
                {
                    element.Act = ActBomb;
                    element.Draw = DrawBomb;
                    element.Interact = InteractBomb;
                }
                else if (element.Id == Elements.BoulderId)
                {
                    element.Interact = InteractPushable;
                }
                else if (element.Id == Elements.BulletId)
                {
                    element.Act = ActBullet;
                    element.Interact = InteractEnemy;
                }
                else if (element.Id == Elements.ClockwiseId)
                {
                    element.Act = ActClockwise;
                    element.Draw = DrawClockwise;
                }
                else if (element.Id == Elements.CounterId)
                {
                    element.Act = ActCounter;
                    element.Draw = DrawCounter;
                }
                else if (element.Id == Elements.DoorId)
                {
                    element.Interact = InteractDoor;
                }
                else if (element.Id == Elements.DragonPupId)
                {
                    element.Act = ActDragonPup;
                    element.Draw = DrawDragonPup;
                    element.Interact = InteractEnemy;
                }
                else if (element.Id == Elements.DuplicatorId)
                {
                    element.Act = ActDuplicator;
                    element.Draw = DrawDuplicator;
                }
                else if (element.Id == Elements.EnergizerId)
                {
                    element.Interact = InteractEnergizer;
                }
                else if (element.Id == Elements.FakeId)
                {
                    element.Interact = InteractFake;
                }
                else if (element.Id == Elements.ForestId)
                {
                    element.Interact = InteractForest;
                }
                else if (element.Id == Elements.GemId)
                {
                    element.Interact = InteractGem;
                }
                else if (element.Id == Elements.HeadId)
                {
                    element.Act = ActHead;
                    element.Interact = InteractEnemy;
                }
                else if (element.Id == Elements.InvisibleId)
                {
                    element.Interact = InteractInvisible;
                }
                else if (element.Id == Elements.KeyId)
                {
                    element.Interact = InteractKey;
                }
                else if (element.Id == Elements.LavaId || element.Id == Elements.WaterId)
                {
                    element.Interact = InteractWater;
                }
                else if (element.Id == Elements.LineId)
                {
                    element.Draw = DrawLine;
                }
                else if (element.Id == Elements.LionId)
                {
                    element.Act = ActLion;
                    element.Interact = InteractEnemy;
                }
                else if (element.Id == Elements.MessengerId)
                {
                    element.Act = ActMessenger;
                }
                else if (element.Id == Elements.MonitorId)
                {
                    element.Act = ActMonitor;
                }
                else if (element.Id == Elements.ObjectId)
                {
                    element.Act = ActObject;
                    element.Draw = DrawObject;
                    element.Interact = InteractObject;
                }
                else if (element.Id == Elements.PairerId)
                {
                    element.Act = ActPairer;
                }
                else if (element.Id == Elements.PassageId)
                {
                    element.Interact = InteractPassage;
                }
                else if (element.Id == Elements.PlayerId)
                {
                    element.Act = ActPlayer;
                }
                else if (element.Id == Elements.PusherId)
                {
                    element.Act = ActPusher;
                    element.Draw = DrawPusher;
                }
                else if (element.Id == Elements.RotonId)
                {
                    element.Act = ActRoton;
                    element.Interact = InteractEnemy;
                }
                else if (element.Id == Elements.RuffianId)
                {
                    element.Act = ActRuffian;
                    element.Interact = InteractEnemy;
                }
                else if (element.Id == Elements.ScrollId)
                {
                    element.Act = ActScroll;
                    element.Interact = InteractScroll;
                }
                else if (element.Id == Elements.SegmentId)
                {
                    element.Act = ActSegment;
                    element.Interact = InteractEnemy;
                }
                else if (element.Id == Elements.SharkId)
                {
                    element.Act = ActShark;
                }
                else if (element.Id == Elements.SliderEwId || element.Id == Elements.SliderNsId)
                {
                    element.Interact = InteractPushable;
                }
                else if (element.Id == Elements.SlimeId)
                {
                    element.Act = ActSlime;
                    element.Interact = InteractSlime;
                }
                else if (element.Id == Elements.SpiderId)
                {
                    element.Act = ActSpider;
                    element.Interact = InteractEnemy;
                }
                else if (element.Id == Elements.SpinningGunId)
                {
                    element.Act = ActSpinningGun;
                    element.Draw = DrawSpinningGun;
                }
                else if (element.Id == Elements.StarId)
                {
                    element.Act = ActStar;
                    element.Draw = DrawSpinningGun;
                    element.Interact = InteractEnemy;
                }
                else if (element.Id == Elements.StoneId)
                {
                    element.Act = ActStone;
                    element.Draw = DrawStone;
                    element.Interact = InteractStone;
                }
                else if (element.Id == Elements.TigerId)
                {
                    element.Act = ActTiger;
                    element.Interact = InteractEnemy;
                }
                else if (element.Id == Elements.TorchId)
                {
                    element.Interact = InteractTorch;
                }
                else if (element.Id == Elements.TransporterId)
                {
                    element.Act = ActTransporter;
                    element.Draw = DrawTransporter;
                    element.Interact = InteractTransporter;
                }
                else if (element.Id == Elements.WebId)
                {
                    element.Draw = DrawWeb;
                }
            }
        }

        protected virtual void InitializeElements(bool showInvisibles)
        {
            // this isn't all the initializations.
            // todo: replace this with the ability to completely reinitialize engine default memory
            Elements.InvisibleElement.Character = showInvisibles ? 0xB0 : 0x20;
            Elements.InvisibleElement.Color = 0xFF;
            Elements.PlayerElement.Character = 0x02;
        }

        internal virtual bool IsActorTargeted(int sender, CodeSearchInfo info, string target)
        {
            var success = false;
            switch (target.ToUpperInvariant())
            {
                case @"ALL":
                    success = info.Index <= ActorCount;
                    break;
                case @"OTHERS":
                    if (info.Index <= ActorCount)
                    {
                        if (info.Index != sender)
                        {
                            success = true;
                        }
                        else
                        {
                            info.Index++;
                            success = info.Index <= ActorCount;
                        }
                    }
                    break;
                case @"SELF":
                    if (info.Index > 0)
                    {
                        if (info.Index <= sender)
                        {
                            info.Index = sender;
                            success = true;
                        }
                    }
                    break;
                default:
                    while (true)
                    {
                        // todo: targeted labels
                        break;
                    }
                    break;
            }
            return false;
        }

        internal virtual string KeyAlreadyMessage(int color)
        {
            return @"You already have a " + Colors[color] + @" key!";
        }

        internal virtual string KeyMessage(int color)
        {
            return @"You now have the " + Colors[color] + " key.";
        }

        protected virtual byte[] LoadFile(string filename)
        {
            try
            {
                return Disk.GetFile(filename);
            }
            catch (Exception)
            {
                // TODO: This kind of error handling is bad.
                return null;
            }
        }

        protected virtual void MainLoop(bool gameIsActive)
        {
            var alternating = false;

            Hud.CreateStatusText();
            Hud.UpdateStatus();

            if (Init)
            {
                if (!AboutShown)
                {
                    ShowAbout();
                }
                if (DefaultWorldName.Length <= 0)
                {
                    // normally we would load the world here,
                    // however it will have already been loaded in the context
                }
                StartBoard = Board;
                SetBoard(0);
                Init = false;
            }

            var element = Elements[PlayerElement];
            TileAt(Player.Location).SetTo(element.Id, element.Color);
            if (PlayerElement == Elements.MonitorId)
            {
                SetMessage(0, new Message());
                Hud.DrawTitleStatus();
            }

            if (gameIsActive)
            {
                FadePurple();
            }

            GameWaitTime = GameSpeed << 1;
            BreakGameLoop = false;
            GameCycle = RandomNumberDeterministic(0x64);
            ActIndex = ActorCount + 1;

            while (ThreadActive)
            {
                if (!GamePaused)
                {
                    if (ActIndex <= ActorCount)
                    {
                        var actorData = Actors[ActIndex];
                        if (actorData.Cycle != 0)
                        {
                            if (ActIndex%actorData.Cycle == GameCycle%actorData.Cycle)
                            {
                                Elements[TileAt(actorData.Location).Id].Act(ActIndex);
                            }
                        }
                        ActIndex++;
                    }
                }
                else
                {
                    ActIndex = ActorCount + 1;
                    if (GetMainTimeElapsed(25))
                    {
                        alternating = !alternating;
                    }
                    if (alternating)
                    {
                        var playerElement = Elements.PlayerElement;
                        DrawTile(Player.Location, new AnsiChar(playerElement.Character, playerElement.Color));
                    }
                    else
                    {
                        if (TileAt(Player.Location).Id == Elements.PlayerId)
                        {
                            DrawTile(Player.Location, new AnsiChar(0x20, 0x0F));
                        }
                        else
                        {
                            UpdateBoard(Player.Location);
                        }
                    }
                    Hud.DrawPausing();
                    ReadInput();
                    if (KeyPressed == 0x1B)
                    {
                        if (Health > 0)
                        {
                            BreakGameLoop = Hud.EndGameConfirmation();
                        }
                        else
                        {
                            BreakGameLoop = true;
                            UpdateBorder();
                        }
                        KeyPressed = 0;
                    }
                    if (!KeyVector.IsZero())
                    {
                        var target = Player.Location.Sum(KeyVector);
                        ElementAt(target).Interact(target, 0, KeyVector);
                    }
                    if (!KeyVector.IsZero())
                    {
                        var target = Player.Location.Sum(KeyVector);
                        if (ElementAt(target).IsFloor)
                        {
                            if (ElementAt(Player.Location).Id == Elements.PlayerId)
                            {
                                MoveActor(0, target);
                            }
                            else
                            {
                                UpdateBoard(Player.Location);
                                Player.Location.Add(KeyVector);
                                TileAt(Player.Location).SetTo(Elements.PlayerId, Elements.PlayerElement.Color);
                                UpdateBoard(Player.Location);
                                UpdateRadius(Player.Location, RadiusMode.Update);
                                UpdateRadius(Player.Location.Difference(KeyVector), RadiusMode.Update);
                            }
                            GamePaused = false;
                            Hud.ClearPausing();
                            GameCycle = RandomNumberDeterministic(100);
                            Locked = true;
                        }
                    }
                }

                if (ActIndex > ActorCount)
                {
                    if (!BreakGameLoop)
                    {
                        if (GameWaitTime <= 0 || GetMainTimeElapsed(GameWaitTime))
                        {
                            GameCycle++;
                            if (GameCycle > 420)
                            {
                                GameCycle = 1;
                            }
                            ActIndex = 0;
                            ReadInput();
                        }
                    }
                    WaitForTick();
                }

                if (BreakGameLoop)
                {
                    ClearSound();
                    if (PlayerElement == Elements.PlayerId)
                    {
                        if (Health <= 0)
                        {
                            EnterHighScore(Score);
                        }
                    }
                    else if (PlayerElement == Elements.MonitorId)
                    {
                        Hud.ClearTitleStatus();
                    }
                    element = Elements.PlayerElement;
                    TileAt(Player.Location).SetTo(element.Id, element.Color);
                    GameOver = false;
                    break;
                }
            }
        }

        protected virtual void MoveTile(IXyPair source, IXyPair target)
        {
            var sourceIndex = ActorIndexAt(source);
            if (sourceIndex >= 0)
            {
                MoveActor(sourceIndex, target);
            }
            else
            {
                TileAt(target).CopyFrom(TileAt(source));
                UpdateBoard(target);
                RemoveItem(source);
                UpdateBoard(source);
            }
        }

        protected byte[] PlayMusic(string music)
        {
            return new byte[0];
        }

        protected virtual void PushThroughTransporter(IXyPair location, IXyPair vector)
        {
            var actor = ActorAt(location.Sum(vector));

            if (actor.Vector.Matches(vector))
            {
                var search = actor.Location.Clone();
                var target = new Location();
                var ended = false;
                var success = true;

                while (!ended)
                {
                    search.Add(vector);
                    var element = ElementAt(search);
                    if (element.Id == Elements.BoardEdgeId)
                    {
                        ended = true;
                    }
                    else
                    {
                        if (success)
                        {
                            success = false;
                            if (!element.IsFloor)
                            {
                                Push(search, vector);
                                element = ElementAt(search);
                            }
                            if (element.IsFloor)
                            {
                                ended = true;
                                target.CopyFrom(search);
                            }
                            else
                            {
                                target.X = 0;
                            }
                        }
                    }

                    if (element.Id == Elements.TransporterId)
                    {
                        if (ActorAt(search).Vector.Matches(vector.Opposite()))
                        {
                            success = true;
                        }
                    }
                }

                if (target.X > 0)
                {
                    MoveTile(actor.Location.Difference(vector), target);
                    PlaySound(3, Sounds.Transporter);
                }
            }
        }

        private void RemoveActor(int index)
        {
            var actor = Actors[index];
            if (index < ActIndex)
            {
                ActIndex--;
            }

            TileAt(actor.Location).CopyFrom(actor.UnderTile);
            if (actor.Location.Y > 0)
            {
                UpdateBoard(actor.Location);
            }

            for (var i = 1; i <= ActorCount; i++)
            {
                var a = Actors[i];
                if (a.Follower >= index)
                {
                    if (a.Follower == index)
                    {
                        a.Follower = -1;
                    }
                    else
                    {
                        a.Follower--;
                    }
                }

                if (a.Leader >= index)
                {
                    if (a.Leader == index)
                    {
                        a.Leader = -1;
                    }
                    else
                    {
                        a.Leader--;
                    }
                }
            }

            if (index < ActorCount)
            {
                for (var i = index; i < ActorCount; i++)
                {
                    Actors[i].CopyFrom(Actors[i + 1]);
                }
            }

            ActorCount--;
        }

        private void ResetAlerts()
        {
            Alerts.AmmoPickup = true;
            Alerts.Dark = true;
            Alerts.EnergizerPickup = true;
            Alerts.FakeWall = true;
            Alerts.Forest = true;
            Alerts.GemPickup = true;
            Alerts.OutOfAmmo = true;
            Alerts.CantShootHere = true;
            Alerts.NotDark = true;
            Alerts.NoTorches = true;
            Alerts.TorchPickup = true;
        }

        private void Rnd(IXyPair result)
        {
            result.X = RandomNumberDeterministic(3) - 1;
            if (result.X == 0)
            {
                result.Y = (RandomNumberDeterministic(2) << 1) - 1;
            }
            else
            {
                result.Y = 0;
            }
        }

        private void RndP(IXyPair source, IXyPair result)
        {
            result.CopyFrom(
                RandomNumberDeterministic(2) == 0
                    ? source.Clockwise()
                    : source.CounterClockwise());
        }

        internal virtual int SearchActorCode(int index, string term)
        {
            var result = -1;
            var termBytes = term.ToBytes();
            var actor = Actors[index];
            var offset = new ByRefInstruction(0);

            while (offset.Instruction < actor.Length)
            {
                var oldOffset = offset.Instruction;
                var termOffset = 0;
                var success = false;

                while (true)
                {
                    ReadActorCodeByte(index, offset);
                    if (termBytes[termOffset].ToUpperCase() != OopByte.ToUpperCase())
                    {
                        success = false;
                        break;
                    }
                    termOffset++;
                    if (termOffset >= termBytes.Length)
                    {
                        success = true;
                        break;
                    }
                }

                if (success)
                {
                    ReadActorCodeByte(index, offset);
                    OopByte = OopByte.ToUpperCase();
                    if (!((OopByte >= 0x41 && OopByte <= 0x5A) || OopByte == 0x5F))
                    {
                        result = oldOffset;
                        break;
                    }
                }

                oldOffset++;
                offset.Instruction = oldOffset;
            }

            return result;
        }

        private void Seek(IXyPair location, IXyPair result)
        {
            result.SetTo(0, 0);
            if (RandomNumberDeterministic(2) == 0 || Player.Location.Y == location.Y)
            {
                result.X = (Player.Location.X - location.X).Polarity();
            }
            if (result.X == 0)
            {
                result.Y = (Player.Location.Y - location.Y).Polarity();
            }
            if (EnergyCycles > 0)
            {
                result.SetOpposite();
            }
        }

        protected void SetEditorMode()
        {
            InitializeElements(true);
            EditorMode = true;
        }

        protected void SetGameMode()
        {
            InitializeElements(false);
            EditorMode = false;
        }

        private void ShowAbout()
        {
            ShowHelp("ABOUT");
        }

        private void ShowHelp(string filename)
        {
        }

        protected virtual void ShowInGameHelp()
        {
            ShowHelp("GAME");
        }

        private void SpawnActor(IXyPair location, ITile tile, int cycle, IActor source)
        {
            // must reserve one actor for player, and one for messenger
            if (ActorCount < Actors.Capacity - 2)
            {
                ActorCount++;
                var actor = Actors[ActorCount];

                if (source == null)
                {
                    source = DefaultActor;
                }
                actor.CopyFrom(source);
                actor.Location.CopyFrom(location);
                actor.Cycle = cycle;
                actor.UnderTile.CopyFrom(TileAt(location));
                if (ElementAt(actor.Location).IsEditorFloor)
                {
                    var newColor = TileAt(actor.Location).Color & 0x70;
                    newColor |= tile.Color & 0x0F;
                    TileAt(actor.Location).Color = newColor;
                }
                else
                {
                    TileAt(actor.Location).Color = tile.Color;
                }
                TileAt(actor.Location).Id = tile.Id;
                if (actor.Location.Y > 0)
                {
                    UpdateBoard(actor.Location);
                }
            }
        }

        private bool SpawnProjectile(int id, IXyPair location, IXyPair vector, bool enemyOwned)
        {
            var target = location.Sum(vector);
            var element = ElementAt(target);

            if (element.IsFloor || element.Id == Elements.WaterId)
            {
                SpawnActor(target, new Tile(id, Elements[id].Color), 1, DefaultActor);
                var actor = Actors[ActorCount];
                actor.P1 = enemyOwned ? 1 : 0;
                actor.Vector.CopyFrom(vector);
                actor.P2 = 0x64;
                return true;
            }
            if (element.Id != Elements.BreakableId)
            {
                if (element.IsDestructible)
                {
                    if (enemyOwned != (element.Id == Elements.PlayerId) || EnergyCycles > 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                Destroy(target);
                PlaySound(2, Sounds.BulletDie);
                return true;
            }
            return false;
        }

        protected virtual void StartMain()
        {
            GameSpeed = 4;
            DefaultSaveName = "SAVED";
            DefaultBoardName = "TEMP";
            DefaultWorldName = "TOWN";
            Hud.GenerateFadeMatrix();
            if (!WorldLoaded)
            {
                ClearWorld();
            }
            SetGameMode();
            TitleScreenLoop();
        }

        private void StopSound()
        {
        }

        protected ITile TileAt(IXyPair l)
        {
            return Tiles[l];
        }

        private ITile TileAt(int x, int y)
        {
            return Tiles[new Location(x, y)];
        }

        internal virtual void TitleScreenLoop()
        {
            QuitZzt = false;
            Init = true;
            StartBoard = 0;
            var gameEnded = true;
            Hud.Initialize();
            while (ThreadActive)
            {
                if (!Init)
                {
                    SetBoard(0);
                }
                while (ThreadActive)
                {
                    PlayerElement = Elements.MonitorId;
                    var gameIsActive = false;
                    GamePaused = false;
                    MainLoop(gameEnded);
                    if (!ThreadActive)
                    {
                        // escape if the thread is supposed to shut down
                        break;
                    }

                    switch (KeyPressed.ToUpperCase())
                    {
                        case 0x57: // W
                            break;
                        case 0x50: // P
                            if (Locked)
                            {
                                // reload world here
                                gameIsActive = WorldLoaded;
                                StartBoard = Board;
                            }
                            else
                            {
                                gameIsActive = true;
                            }
                            if (gameIsActive)
                            {
                                SetBoard(StartBoard);
                                EnterBoard();
                            }
                            break;
                        case 0x41: // A
                            break;
                        case 0x45: // E
                            break;
                        case 0x53: // S
                            break;
                        case 0x52: // R
                            break;
                        case 0x48: // H
                            break;
                        case 0x7C: // ?
                            break;
                        case 0x1B: // esc
                        case 0x51: // Q
                            break;
                    }

                    if (gameIsActive)
                    {
                        PlayerElement = Elements.PlayerId;
                        GamePaused = true;
                        MainLoop(true);
                        gameEnded = true;
                    }

                    if (gameEnded || QuitZzt)
                    {
                        break;
                    }
                }
                if (QuitZzt)
                {
                    break;
                }
            }
        }

        private void UpdateBorder()
        {
            Hud.UpdateBorder();
        }

        private void UpdateCamera()
        {
            Hud.UpdateCamera();
        }

        private void UpdateRadius(IXyPair location, RadiusMode mode)
        {
            var source = location.Clone();
            var left = source.X - 9;
            var right = source.X + 9;
            var top = source.Y - 6;
            var bottom = source.Y + 6;
            for (var x = left; x <= right; x++)
            {
                for (var y = top; y <= bottom; y++)
                {
                    if (x >= 1 && x <= Width && y >= 1 && y <= Height)
                    {
                        var target = new Location(x, y);
                        if (mode != RadiusMode.Update)
                        {
                            if (Distance(source, target) < 50)
                            {
                                var element = ElementAt(target);
                                if (mode == RadiusMode.Explode)
                                {
                                    if (element.CodeEditText.Length > 0)
                                    {
                                        var actorIndex = ActorIndexAt(target);
                                        if (actorIndex > 0)
                                        {
                                            BroadcastLabel(-actorIndex, @"BOMBED", false);
                                        }
                                    }
                                    if (element.IsDestructible || element.Id == Elements.StarId)
                                    {
                                        Destroy(target);
                                    }
                                    if (element.Id == Elements.EmptyId || element.Id == Elements.BreakableId)
                                    {
                                        TileAt(target).SetTo(Elements.BreakableId, RandomNumberDeterministic(7) + 9);
                                    }
                                }
                                else
                                {
                                    if (TileAt(target).Id == Elements.BreakableId)
                                    {
                                        TileAt(target).Id = Elements.EmptyId;
                                    }
                                }
                            }
                        }
                        UpdateBoard(target);
                    }
                }
            }
        }

        private void UpdateStatus()
        {
            Hud.UpdateStatus();
        }
    }
}