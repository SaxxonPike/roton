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
                    MoveThing(index, target);
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
                        SendLabel(-ActorIndex(target), @"SHOT", false);
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
                MoveThing(index, target);
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
                Display.DrawMessage(" " + Message + " ", (actor.P2 % 7) + 9);
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
                if (ActorIndex(new Location(0, 0)) == -1)
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
                            MoveThing(0, actor.Location.Sum(KeyVector));
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
                    break;
                case 0x51: // Q
                case 0x1B: // escape
                    BreakGameLoop = Display.EndGameConfirmation();
                    break;
                case 0x53: // S
                    break;
                case 0x50: // P
                    break;
                case 0x42: // B
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
            UpdateBoard(actor.Location);

            // todo: the rest of spinning gun code
        }

        virtual public void Act_Star(int index)
        {
        }

        virtual public void Act_Stone(int index)
        {
        }

        virtual public void Act_Tiger(int index)
        {
            // todo: shooting logic for tigers
            Act_Lion(index);
        }

        virtual public void Act_Transporter(int index)
        {
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
                        MoveThing(0, target);
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
        }

        virtual public void Interact_Door(Location location, int index, Vector vector)
        {
        }

        virtual public void Interact_Enemy(Location location, int index, Vector vector)
        {
        }

        virtual public void Interact_Energizer(Location location, int index, Vector vector)
        {
        }

        virtual public void Interact_Fake(Location location, int index, Vector vector)
        {
        }

        virtual public void Interact_Forest(Location location, int index, Vector vector)
        {
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
        }

        virtual public void Interact_Object(Location location, int index, Vector vector)
        {
        }

        virtual public void Interact_Passage(Location location, int index, Vector vector)
        {
            ExecutePassage(location);
            vector.SetTo(0, 0);
        }

        virtual public void Interact_Pushable(Location location, int index, Vector vector)
        {
        }

        virtual public void Interact_Scroll(Location location, int index, Vector vector)
        {
        }

        virtual public void Interact_Slime(Location location, int index, Vector vector)
        {
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
        }

        virtual public void Interact_Water(Location location, int index, Vector vector)
        {
            PlaySound(3, Sounds.Water);
            SetMessage(0x64, WaterMessage);
        }
    }
}
