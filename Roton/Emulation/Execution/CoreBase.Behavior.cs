﻿using System.Linq;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Execution
{
    internal abstract partial class CoreBase
    {
        public virtual void Act_Bear(int index)
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

        public virtual void Act_BlinkWall(int index)
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
                        Vector testVector;

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

        public virtual void Act_Bomb(int index)
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

        public virtual void Act_Bullet(int index)
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
                    break;
                }
                break;
            }
        }

        public virtual void Act_Clockwise(int index)
        {
            var actor = Actors[index];
            UpdateBoard(actor.Location);
            Convey(actor.Location, 1);
        }

        public virtual void Act_Counter(int index)
        {
            var actor = Actors[index];
            UpdateBoard(actor.Location);
            Convey(actor.Location, -1);
        }

        public virtual void Act_DragonPup(int index)
        {
            UpdateBoard(Actors[index].Location);
        }

        public virtual void Act_Duplicator(int index)
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

        public virtual void Act_Head(int index)
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
            else if (actor.Vector.IsZero() || actor.P2 > (RandomNumberDeterministic(10) << 2))
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
                            if (ElementAt(origin.Difference(vector)).Id == Elements.SegmentId && ActorAt(origin.Difference(vector)).Leader <= 0)
                            {
                                segment.Follower = ActorIndexAt(origin.Difference(vector));
                            }
                            else if (ElementAt(origin.Difference(vector.Swap())).Id == Elements.SegmentId && ActorAt(origin.Difference(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = ActorIndexAt(origin.Difference(vector.Swap()));
                            }
                            else if (ElementAt(origin.Sum(vector.Swap())).Id == Elements.SegmentId && ActorAt(origin.Sum(vector.Swap())).Leader <= 0)
                            {
                                segment.Follower = ActorIndexAt(origin.Sum(vector.Swap()));
                            }
                            else
                            {
                                segment.Follower = -1;
                            }
                        }

                        // Move follower segment
                        if (segment.Follower > 0)
                        {
                            var follower = Actors[segment.Follower];
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

        public virtual void Act_Lion(int index)
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

        public virtual void Act_Messenger(int index)
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

        public virtual void Act_Monitor(int index)
        {
            if (KeyPressed != 0)
            {
                BreakGameLoop = true;
            }
        }

        public virtual void Act_Object(int index)
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

        public virtual void Act_Pairer(int index)
        {
            // pairer code only reads the actor's tile...
        }

        public virtual void Act_Player(int index)
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
                    SetMessage(0x7D00, GameOverMessage);
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
                            if (AlertNoAmmo)
                            {
                                SetMessage(0xC8, NoAmmoMessage);
                                AlertNoAmmo = false;
                            }
                        }
                    }
                    else
                    {
                        if (AlertNoShoot)
                        {
                            SetMessage(0xC8, NoShootMessage);
                            AlertNoShoot = false;
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
                                if (AlertNoTorch)
                                {
                                    SetMessage(0xC8, NoTorchMessage);
                                    AlertNoTorch = false;
                                }
                            }
                            else if (!Dark)
                            {
                                if (AlertNotDark)
                                {
                                    SetMessage(0xC8, NotDarkMessage);
                                    AlertNotDark = false;
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
                            SetMessage(0xC8, TimeMessage);
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

        public virtual void Act_Pusher(int index)
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

        public virtual void Act_Roton(int index)
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

        public virtual void Act_Ruffian(int index)
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

        public virtual void Act_Scroll(int index)
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

        public virtual void Act_Segment(int index)
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

        public virtual void Act_Shark(int index)
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

        public virtual void Act_Slime(int index)
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

        public virtual void Act_Spider(int index)
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

            if (!Act_Spider_AttemptDirection(index, vector))
            {
                var i = (RandomNumberDeterministic(2) << 1) - 1;
                if (!Act_Spider_AttemptDirection(index, vector.Product(i).Swap()))
                {
                    if (!Act_Spider_AttemptDirection(index, vector.Product(i).Swap().Opposite()))
                    {
                        Act_Spider_AttemptDirection(index, vector.Opposite());
                    }
                }
            }
        }

        internal virtual bool Act_Spider_AttemptDirection(int index, IXyPair vector)
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

        public virtual void Act_SpinningGun(int index)
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

        public virtual void Act_Star(int index)
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

        public virtual void Act_Stone(int index)
        {
            UpdateBoard(Actors[index].Location);
        }

        public virtual void Act_Tiger(int index)
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
                    SpawnProjectile(firingElement, actor.Location, new Vector(0, (Player.Location.Y - actor.Location.Y).Polarity()), true);

                if (!shot && actor.Location.Y.AbsDiff(Player.Location.Y) <= 2)
                {
                    SpawnProjectile(firingElement, actor.Location, new Vector((Player.Location.X - actor.Location.X).Polarity(), 0), true);
                }
            }

            Act_Lion(index);
        }

        public virtual void Act_Transporter(int index)
        {
            UpdateBoard(Actors[index].Location);
        }

        public virtual void Interact_Ammo(IXyPair location, int index, IXyPair vector)
        {
            // Ammo gain is implemented per engine, do it before this
            RemoveItem(location);
            UpdateStatus();
            PlaySound(2, Sounds.Ammo);
            if (AlertAmmo)
            {
                SetMessage(0xC8, AmmoMessage);
                AlertAmmo = false;
            }
        }

        public virtual void Interact_BoardEdge(IXyPair location, int index, IXyPair vector)
        {
            var target = location.Clone();
            var targetBoard = 0;
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

        public virtual void Interact_Bomb(IXyPair location, int index, IXyPair vector)
        {
            var actor = ActorAt(location);
            if (actor.P1 == 0)
            {
                actor.P1 = 9;
                UpdateBoard(location);
                SetMessage(0xC8, BombMessage);
                PlaySound(4, Sounds.BombActivate);
            }
            else
            {
                Push(location, vector);
            }
        }

        public virtual void Interact_Door(IXyPair location, int index, IXyPair vector)
        {
            var color = (TileAt(location).Color & 0x70) >> 4;
            var keyIndex = color - 1;
            if (!Keys[keyIndex])
            {
                SetMessage(0xC8, DoorClosedMessage(color));
                PlaySound(3, Sounds.DoorLocked);
            }
            else
            {
                Keys[keyIndex] = false;
                RemoveItem(location);
                SetMessage(0xC8, DoorOpenMessage(color));
                PlaySound(3, Sounds.DoorOpen);
            }
        }

        public virtual void Interact_Enemy(IXyPair location, int index, IXyPair vector)
        {
            Attack(index, location);
        }

        public virtual void Interact_Energizer(IXyPair location, int index, IXyPair vector)
        {
            PlaySound(9, Sounds.Energizer);
            RemoveItem(location);
            EnergyCycles = 0x4B;
            UpdateStatus();
            UpdateBoard(location);
            if (AlertEnergy)
            {
                AlertEnergy = false;
                SetMessage(0xC8, EnergizerMessage);
            }
            BroadcastLabel(0, @"ALL:ENERGIZE", false);
        }

        public virtual void Interact_Fake(IXyPair location, int index, IXyPair vector)
        {
            if (!AlertFake) return;

            AlertFake = false;
            SetMessage(0xC8, FakeMessage);
        }

        public virtual void Interact_Forest(IXyPair location, int index, IXyPair vector)
        {
            RemoveItem(location);
            UpdateBoard(location);
            PlaySound(3, Sounds.Forest);
            if (AlertForest)
            {
                SetMessage(0xC8, ForestMessage);
                AlertForest = false;
            }
        }

        public virtual void Interact_Gem(IXyPair location, int index, IXyPair vector)
        {
            // Health gain is implemented per engine, do it before this
            Gems += 1;
            Score += 10;
            RemoveItem(location);
            UpdateStatus();
            PlaySound(2, Sounds.Gem);
            if (AlertGem)
            {
                SetMessage(0xC8, GemMessage);
                AlertGem = false;
            }
        }

        public virtual void Interact_Invisible(IXyPair location, int index, IXyPair vector)
        {
            TileAt(location).Id = Elements.NormalId;
            UpdateBoard(location);
            PlaySound(3, Sounds.Invisible);
            SetMessage(0x64, InvisibleMessage);
        }

        public virtual void Interact_Key(IXyPair location, int index, IXyPair vector)
        {
            var color = TileAt(location).Color & 0x07;
            var keyIndex = color - 1;
            if (Keys[keyIndex])
            {
                SetMessage(0xC8, KeyAlreadyMessage(color));
                PlaySound(2, Sounds.KeyAlready);
            }
            else
            {
                Keys[keyIndex] = true;
                RemoveItem(location);
                SetMessage(0xC8, KeyMessage(color));
                PlaySound(2, Sounds.Key);
            }
        }

        public virtual void Interact_Object(IXyPair location, int index, IXyPair vector)
        {
            var objectIndex = ActorIndexAt(location);
            BroadcastLabel(-objectIndex, @"TOUCH", false);
        }

        public virtual void Interact_Passage(IXyPair location, int index, IXyPair vector)
        {
            ExecutePassage(location);
            vector.SetTo(0, 0);
        }

        public virtual void Interact_Pushable(IXyPair location, int index, IXyPair vector)
        {
            Push(location, vector);
            PlaySound(2, Sounds.Push);
        }

        public virtual void Interact_Scroll(IXyPair location, int index, IXyPair vector)
        {
            var scrollIndex = ActorIndexAt(location);
            var actor = Actors[scrollIndex];

            PlaySound(2, PlayMusic(@"c-c+d-d+e-e+f-f+g-g"));
            ExecuteCode(scrollIndex, actor, @"Scroll");
            RemoveActor(scrollIndex);
        }

        public virtual void Interact_Slime(IXyPair location, int index, IXyPair vector)
        {
            var color = TileAt(location).Color;
            var slimeIndex = ActorIndexAt(location);
            Harm(slimeIndex);
            TileAt(location).SetTo(Elements.BreakableId, color);
            UpdateBoard(location);
            PlaySound(2, Sounds.SlimeDie);
        }

        public virtual void Interact_Stone(IXyPair location, int index, IXyPair vector)
        {
            if (Stones < 0)
            {
                Stones = 0;
            }
            Stones++;
            Destroy(location);
            UpdateStatus();
            SetMessage(0xC8, @"You have found a", @"Stone of Power!");
        }

        public virtual void Interact_Torch(IXyPair location, int index, IXyPair vector)
        {
            Torches++;
            RemoveItem(location);
            UpdateStatus();
            if (AlertTorch)
            {
                SetMessage(0xC8, TorchMessage);
                AlertTorch = false;
            }
            PlaySound(3, Sounds.Torch);
        }

        public virtual void Interact_Transporter(IXyPair location, int index, IXyPair vector)
        {
            PushThroughTransporter(location.Difference(vector), vector);
            vector.SetTo(0, 0);
        }

        public virtual void Interact_Water(IXyPair location, int index, IXyPair vector)
        {
            PlaySound(3, Sounds.Water);
            SetMessage(0x64, WaterMessage);
        }
    }
}