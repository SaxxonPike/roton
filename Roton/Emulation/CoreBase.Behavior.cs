using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal partial class CoreBase
    {
        virtual public void Act_Bear(int index)
        {
        }

        virtual public void Act_BlinkWall(int index)
        {
        }

        virtual public void Act_Bomb(int index)
        {
            var actor = Actors[index];
            if (actor.P1 > 0)
            {
                actor.P1--;
                UpdateBoard(actor.Location);
                if (actor.P1 == 1)
                {
                    PlaySound(1, Sounds.BombExplode);
                    UpdateRadius(actor.Location, RadiusMode.Explode);
                }
                else if (actor.P1 == 0)
                {
                    Location location = actor.Location.Clone();
                    RemoveActor(index);
                    UpdateRadius(location, RadiusMode.Clear);
                }
                else if ((actor.P1 & 0x01) == 0)
                {
                    PlaySound(1, Sounds.BombTock);
                }
                else
                {
                    PlaySound(1, Sounds.BombTick);
                }
            }
        }

        virtual public void Act_Bullet(int index)
        {
            var actor = Actors[index];
            bool canRicochet = true;
            while (true)
            {
                var target = actor.Location.Sum(actor.Vector);
                var element = ElementAt(target);
                if (element.Floor || element.Index == Elements.WaterId)
                {
                    MoveActor(index, target);
                    break;
                }
                else if (canRicochet && element.Index == Elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetOpposite();
                    PlaySound(1, Sounds.Ricochet);
                    continue;
                }
                else if (element.Index == Elements.BreakableId || (element.Destructible && (element.Index == Elements.PlayerId || actor.P1 == 0)))
                {
                    if (element.Points != 0)
                    {
                        Score += element.Points;
                        UpdateStatus();
                    }
                    Attack(index, target);
                    break;
                }
                else if (canRicochet && TileAt(actor.Location.Sum(actor.Vector.Clockwise)).Id == Elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetCounterClockwise();
                    PlaySound(1, Sounds.Ricochet);
                    continue;
                }
                else if (canRicochet && TileAt(actor.Location.Sum(actor.Vector.CounterClockwise)).Id == Elements.RicochetId)
                {
                    canRicochet = false;
                    actor.Vector.SetClockwise();
                    PlaySound(1, Sounds.Ricochet);
                    continue;
                }
                else
                {
                    RemoveActor(index);
                    ActIndex--;
                    if (element.Index == Elements.ObjectId || element.Index == Elements.ScrollId)
                    {
                        SendLabel(-ActorIndexAt(target), @"SHOT", false);
                        break;
                    }
                }
                break;
            }
        }

        virtual public void Act_Clockwise(int index)
        {
            var actor = Actors[index];
            UpdateBoard(actor.Location);
            Convey(actor.Location, 1);
        }

        virtual public void Act_Counter(int index)
        {
            var actor = Actors[index];
            UpdateBoard(actor.Location);
            Convey(actor.Location, -1);
        }

        virtual public void Act_DragonPup(int index)
        {
        }

        virtual public void Act_Duplicator(int index)
        {
        }

        virtual public void Act_Head(int index)
        {
        }

        virtual public void Act_Lion(int index)
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
            if (element.Floor)
            {
                MoveActor(index, target);
            }
            else if (element.Index == Elements.PlayerId)
            {
                Attack(index, target);
            }
        }

        virtual public void Act_Messenger(int index)
        {
            var actor = Actors[index];
            if (actor.X == 0)
            {
                Display.DrawMessage(Message, (actor.P2 % 7) + 9);
                actor.P2--;
                if (actor.P2 <= 0)
                {
                    RemoveActor(index);
                    ActIndex--;
                    UpdateBorder();
                    Message = "";
                }
            }
        }

        virtual public void Act_Monitor(int index)
        {
        }

        virtual public void Act_Object(int index)
        {
            var actor = Actors[index];
            if (actor.Instruction >= 0)
            {
                ExecuteOOP(index, actor, @"Interaction");
            }
            if (!actor.Vector.IsZero)
            {
                var target = actor.Location.Sum(actor.Vector);
                if (ElementAt(target).Floor)
                {
                    MoveActor(index, target);
                }
                else
                {
                    SendLabel(-index, @"THUD", false);
                }
            }
        }

        virtual public void Act_Pairer(int index)
        {
        }

        virtual public void Act_Player(int index)
        {
            var actor = Actors[index];
            var playerElement = Elements.PlayerElement;

            if (EnergyCycles > 0)
            {
                if (playerElement.Character == 1)
                {
                    playerElement.Character = 2;
                }
                else
                {
                    playerElement.Character = 1;
                }

                if ((GameCycle & 0x01) == 0)
                {
                    TileAt(actor.Location).Color = (((GameCycle % 7) + 1) << 4) | 0x0F;
                }
                else
                {
                    TileAt(actor.Location).Color = 0x0F;
                }

                UpdateBoard(actor.Location);
            }
            else
            {
                ForcePlayerColor();
            }

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

            if (!KeyShift)
            {
                if (!KeyVector.IsZero)
                {
                    ElementAt(actor.Location.Sum(KeyVector)).Interact(actor.Location.Sum(KeyVector), 0, KeyVector);
                    if (!KeyVector.IsZero)
                    {
                        if (!SoundPlaying)
                        {
                            Speaker.PlayDrum(3);
                        }
                        if (ElementAt(actor.Location.Sum(KeyVector)).Floor)
                        {
                            MoveActor(0, actor.Location.Sum(KeyVector));
                        }
                    }
                }
            }
            else
            {
                // todo: shoot logic
            }

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
                    BreakGameLoop = Display.EndGameConfirmation();
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
                    break;
                case 0x46: // F
                    break;
                case 0x3F: // ?
                    break;
            }

            if (TorchCycles > 0)
            {
                TorchCycles--;
                if (TorchCycles <= 0)
                {
                    UpdateRadius(actor.Location, RadiusMode.Update);
                    PlaySound(3, Sounds.TorchOut);
                }

                if (TorchCycles % 40 == 0)
                {
                    UpdateStatus();
                }
            }

            if (EnergyCycles > 0)
            {
                EnergyCycles--;
                if (EnergyCycles == 10)
                {
                    PlaySound(9, Sounds.EnergyOut);
                }
                else if (EnergyCycles <= 0)
                {
                    ForcePlayerColor();
                }
            }

            if (TimeLimit > 0)
            {
                // todo: time limit stuff
            }
        }

        virtual public void Act_Pusher(int index)
        {
            var actor = Actors[index];
            var source = actor.Location.Clone();

            if (!ElementAt(actor.Location.Sum(actor.Vector)).Floor)
            {
                Push(actor.Location.Sum(actor.Vector), actor.Vector);
            }

            index = ActorIndexAt(source);
            actor = Actors[index];
            if (ElementAt(actor.Location.Sum(actor.Vector)).Floor)
            {
                MoveActor(index, actor.Location.Sum(actor.Vector));
                PlaySound(2, Sounds.Push);
                var behindLocation = actor.Location.Difference(actor.Vector);
                if (TileAt(behindLocation).Id == Elements.PusherId)
                {
                    var behindIndex = ActorIndexAt(behindLocation);
                    var behindActor = Actors[behindIndex];
                    if (behindActor.Vector.X == actor.Vector.X && behindActor.Vector.Y == actor.Vector.Y)
                    {
                        Elements.PusherElement.Act(behindIndex);
                    }
                }
            }
        }

        virtual public void Act_Roton(int index)
        {
        }

        virtual public void Act_Ruffian(int index)
        {
        }

        virtual public void Act_Scroll(int index)
        {
            var actor = Actors[index];
            var color = TileAt(actor.Location).Color;

            color++;
            if (color > 0x0F)
            {
                color = 0x09;
            }
            TileAt(actor.Location).Color = color;
        }

        virtual public void Act_Segment(int index)
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

        virtual public void Act_Shark(int index)
        {
        }

        virtual public void Act_Slime(int index)
        {
        }

        virtual public void Act_Spider(int index)
        {
        }

        virtual public void Act_SpinningGun(int index)
        {
            var actor = Actors[index];
            int firingElement = Elements.BulletId;
            bool shot = false;

            UpdateBoard(actor.Location);

            if (actor.P2 >= 0x80)
            {
                firingElement = Elements.StarId;
            }

            if ((actor.P2 & 0x7F) > RandomNumberDeterministic(9))
            {
                if (actor.P1 >= RandomNumberDeterministic(9))
                {
                    if (actor.X.AbsDiff(Player.X) <= 2)
                    {
                        shot = SpawnProjectile(firingElement, actor.Location, new Vector(0, (Player.Y - actor.Y).Polarity()), true);
                    }
                    if (!shot && actor.Y.AbsDiff(Player.Y) <= 2)
                    {
                        shot = SpawnProjectile(firingElement, actor.Location, new Vector((Player.X - actor.X).Polarity(), 0), true);
                    }
                }
                else
                {
                    shot = SpawnProjectile(firingElement, actor.Location, Rnd(), true);
                }
            }
        }

        virtual public void Act_Star(int index)
        {
        }

        virtual public void Act_Stone(int index)
        {
        }

        virtual public void Act_Tiger(int index)
        {
            var actor = Actors[index];
            int firingElement = Elements.BulletId;
            bool shot = false;

            if (actor.P2 >= 0x80)
            {
                firingElement = Elements.StarId;
            }

            if ((actor.P2 & 0x7F) > (3 * RandomNumberDeterministic(10)))
            {
                if (actor.X.AbsDiff(Player.X) <= 2)
                {
                    shot = SpawnProjectile(firingElement, actor.Location, new Vector(0, (Player.Y - actor.Y).Polarity()), true);
                }
                else
                {
                    shot = false;
                }

                if (!shot && actor.Y.AbsDiff(Player.Y) <= 2)
                {
                    shot = SpawnProjectile(firingElement, actor.Location, new Vector((Player.X - actor.X).Polarity(), 0), true);
                }
            }

            Act_Lion(index);
        }

        virtual public void Act_Transporter(int index)
        {
            UpdateBoard(Actors[index].Location);
        }

        virtual public void Interact_Ammo(Location location, int index, Vector vector)
        {
            Ammo += 5;
            TileAt(location).Id = Elements.EmptyId;
            UpdateStatus();
            PlaySound(2, Sounds.Ammo);
            if (AlertAmmo)
            {
                SetMessage(0xC8, AmmoMessage);
                AlertAmmo = false;
            }
        }

        virtual public void Interact_BoardEdge(Location location, int index, Vector vector)
        {
            Location target = location.Clone();
            int targetBoard = 0;
            int oldBoard = Board;

            if (vector.Y == -1)
            {
                targetBoard = ExitNorth;
                target.Y = Height;
            }
            else if (vector.Y == 1)
            {
                targetBoard = ExitSouth;
                target.Y = 1;
            }
            else if (vector.X == -1)
            {
                targetBoard = ExitWest;
                target.X = Width;
            }
            else
            {
                targetBoard = ExitEast;
                target.X = 1;
            }

            if (targetBoard != 0)
            {
                SetBoard(targetBoard);
                if (TileAt(target).Id != Elements.PlayerId)
                {
                    ElementAt(target).Interact(target, index, KeyVector);
                }
                if (ElementAt(target).Floor || ElementAt(target).Index == Elements.PlayerId)
                {
                    if (ElementAt(target).Index != Elements.PlayerId)
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

        virtual public void Interact_Bomb(Location location, int index, Vector vector)
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

        virtual public void Interact_Door(Location location, int index, Vector vector)
        {
            int color = (TileAt(location).Color & 0x70) >> 4;
            int keyIndex = color - 1;
            if (!Keys[keyIndex])
            {
                SetMessage(0xC8, DoorClosedMessage(color));
                PlaySound(3, Sounds.DoorLocked);
            }
            else
            {
                Keys[keyIndex] = false;
                TileAt(location).Id = Elements.EmptyId;
                SetMessage(0xC8, DoorOpenMessage(color));
                PlaySound(3, Sounds.DoorOpen);
            }
        }

        virtual public void Interact_Enemy(Location location, int index, Vector vector)
        {
            Attack(index, location);
        }

        virtual public void Interact_Energizer(Location location, int index, Vector vector)
        {
            PlaySound(9, Sounds.Energizer);
            TileAt(location).Id = Elements.EmptyId;
            EnergyCycles = 0x4B;
            UpdateStatus();
            UpdateBoard(location);
            if (AlertEnergy)
            {
                AlertEnergy = false;
                SetMessage(0xC8, EnergizerMessage);
            }
            SendLabel(0, @"ALL:ENERGIZE", false);
        }

        virtual public void Interact_Fake(Location location, int index, Vector vector)
        {
            if (AlertFake)
            {
                AlertFake = false;
                SetMessage(0xC8, FakeMessage);
            }
        }

        virtual public void Interact_Forest(Location location, int index, Vector vector)
        {
            TileAt(location).Id = Elements.EmptyId;
            UpdateBoard(location);
            PlaySound(3, Sounds.Forest);
            if (AlertForest)
            {
                SetMessage(0xC8, ForestMessage);
                AlertForest = false;
            }
        }

        virtual public void Interact_Gem(Location location, int index, Vector vector)
        {
            Gems += 1;
            Health += 1;
            Score += 10;
            TileAt(location).Id = Elements.EmptyId;
            UpdateStatus();
            PlaySound(2, Sounds.Gem);
            if (AlertGem)
            {
                SetMessage(0xC8, GemMessage);
                AlertGem = false;
            }
        }

        virtual public void Interact_Invisible(Location location, int index, Vector vector)
        {
            TileAt(location).Id = Elements.NormalId;
            UpdateBoard(location.Sum(vector));
            PlaySound(3, Sounds.Invisible);
            SetMessage(0x64, InvisibleMessage);
        }

        virtual public void Interact_Key(Location location, int index, Vector vector)
        {
            int color = (TileAt(location).Color & 0x07);
            int keyIndex = color - 1;
            if (Keys[keyIndex])
            {
                SetMessage(0xC8, KeyAlreadyMessage(color));
                PlaySound(2, Sounds.KeyAlready);
            }
            else
            {
                Keys[keyIndex] = true;
                TileAt(location).Id = Elements.EmptyId;
                SetMessage(0xC8, KeyMessage(color));
                PlaySound(2, Sounds.Key);
            }
        }

        virtual public void Interact_Object(Location location, int index, Vector vector)
        {
            var objectIndex = ActorIndexAt(location);
            var actor = Actors[objectIndex];
            SendLabel(-objectIndex, @"TOUCH", false);
        }

        virtual public void Interact_Passage(Location location, int index, Vector vector)
        {
            ExecutePassage(location);
            vector.SetTo(0, 0);
        }

        virtual public void Interact_Pushable(Location location, int index, Vector vector)
        {
            Push(location, vector);
            PlaySound(2, Sounds.Push);
        }

        virtual public void Interact_Scroll(Location location, int index, Vector vector)
        {
            var scrollIndex = ActorIndexAt(location);
            var actor = Actors[scrollIndex];

            PlaySound(2, PlayMusic(@"c-c+d-d+e-e+f-f+g-g"));
            ExecuteOOP(scrollIndex, actor, @"Scroll");
            RemoveActor(scrollIndex);
        }

        virtual public void Interact_Slime(Location location, int index, Vector vector)
        {
            int color = TileAt(location).Color;
            int slimeIndex = ActorIndexAt(location);
            Harm(slimeIndex);
            TileAt(location).SetTo(Elements.BreakableId, color);
            UpdateBoard(location);
            PlaySound(2, Sounds.SlimeDie);
        }

        virtual public void Interact_Stone(Location location, int index, Vector vector)
        {
        }

        virtual public void Interact_Torch(Location location, int index, Vector vector)
        {
            Torches++;
            TileAt(location).Id = Elements.EmptyId;
            UpdateStatus();
            if (AlertTorch)
            {
                SetMessage(0xC8, TorchMessage);
                AlertTorch = false;
            }
            PlaySound(3, Sounds.Torch);
        }

        virtual public void Interact_Transporter(Location location, int index, Vector vector)
        {
            PushThroughTransporter(location.Difference(vector), vector);
            vector.SetTo(0, 0);
        }

        virtual public void Interact_Water(Location location, int index, Vector vector)
        {
            PlaySound(3, Sounds.Water);
            SetMessage(0x64, WaterMessage);
        }
    }
}
