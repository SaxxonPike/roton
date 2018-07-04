using System.Linq;
using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.Behavior
{
    public sealed class PlayerBehavior : ElementBehavior
    {
        public override string KnownName => "Player";

        public override void Act(int index)
        {
            var actor = _actorList[index];
            var playerElement = engine.Elements[engine.Elements.PlayerId];

            // Energizer graphics

            if (engine.World.EnergyCycles > 0)
            {
                playerElement.Character = playerElement.Character == 1 ? 2 : 1;

                if ((engine.State.GameCycle & 0x01) == 0)
                {
                    engine.Tiles[actor.Location].Color = ((engine.State.GameCycle%7 + 1) << 4) | 0x0F;
                }
                else
                {
                    engine.Tiles[actor.Location].Color = 0x0F;
                }

                engine.UpdateBoard(actor.Location);
            }
            else
            {
                engine.ForcePlayerColor(index);
            }

            // Health logic

            if (engine.World.Health <= 0)
            {
                engine.State.KeyVector.SetTo(0, 0);
                engine.State.KeyShift = false;
                if (engine.ActorIndexAt(new Location(0, 0)) == -1)
                {
                    engine.SetMessage(0x7D00, engine.Alerts.GameOverMessage);
                }
                engine.State.GameWaitTime = 0;
                engine.State.GameOver = true;
            }

            if (engine.State.KeyVector.IsNonZero())
            {
                if (engine.State.KeyShift || engine.State.KeyPressed == 0x20)
                {
                    // Shooting logic

                    if (engine.Board.MaximumShots > 0)
                    {
                        if (engine.World.Ammo > 0)
                        {
                            var bulletCount =
                                engine.Actors.Count(
                                    a => a.P1 == 0 && engine.Tiles[a.Location].Id == engine.Elements.BulletId);
                            if (bulletCount < engine.Board.MaximumShots)
                            {
                                if (engine.SpawnProjectile(engine.Elements.BulletId, actor.Location,
                                    engine.State.KeyVector, false))
                                {
                                    engine.World.Ammo--;
                                    engine.UpdateStatus();
                                    engine.PlaySound(2, engine.SoundSet.Shoot);
                                }
                            }
                        }
                        else
                        {
                            if (engine.Alerts.OutOfAmmo)
                            {
                                engine.SetMessage(0xC8, engine.Alerts.NoAmmoMessage);
                                engine.Alerts.OutOfAmmo = false;
                            }
                        }
                    }
                    else
                    {
                        if (engine.Alerts.CantShootHere)
                        {
                            engine.SetMessage(0xC8, engine.Alerts.NoShootMessage);
                            engine.Alerts.CantShootHere = false;
                        }
                    }
                }
                else
                {
                    // Movement logic

                    engine.ElementAt(actor.Location.Sum(engine.State.KeyVector))
                        .Interact(engine, actor.Location.Sum(engine.State.KeyVector), 0, engine.State.KeyVector);
                    if (!engine.State.KeyVector.IsZero())
                    {
                        if (!engine.State.SoundPlaying)
                        {
                            // TODO: player step sound plays here
                        }
                        if (engine.ElementAt(actor.Location.Sum(engine.State.KeyVector)).IsFloor)
                        {
                            engine.MoveActor(0, actor.Location.Sum(engine.State.KeyVector));
                        }
                    }
                }
            }

            // Hotkey logic

            var hotkey = engine.State.KeyPressed.ToUpperCase();
            switch (hotkey)
            {
                case 0x51: // Q
                case 0x1B: // escape
                    engine.State.BreakGameLoop = engine.State.GameOver || engine.Hud.EndGameConfirmation();
                    break;
                case 0x53: // S
                    engine.Hud.SaveGame();
                    break;
                case 0x50: // P
                    if (engine.World.Health > 0)
                    {
                        engine.State.GamePaused = true;
                    }
                    break;
                case 0x42: // B
                    engine.State.GameQuiet = !engine.State.GameQuiet;
                    engine.ClearSound();
                    engine.UpdateStatus();
                    engine.State.KeyPressed = 0x20;
                    break;
                case 0x48: // H
                    engine.ShowInGameHelp();
                    break;
                case 0x3F: // ?
                    engine.Hud.EnterCheat();
                    break;
                default:
                    engine.HandlePlayerInput(actor, hotkey);
                    break;
            }

            // Torch logic

            if (engine.World.TorchCycles > 0)
            {
                engine.World.TorchCycles--;
                if (engine.World.TorchCycles <= 0)
                {
                    engine.UpdateRadius(actor.Location, RadiusMode.Update);
                    engine.PlaySound(3, engine.SoundSet.TorchOut);
                }

                if (engine.World.TorchCycles%40 == 0)
                {
                    engine.UpdateStatus();
                }
            }

            // Energizer logic

            if (engine.World.EnergyCycles > 0)
            {
                engine.World.EnergyCycles--;
                if (engine.World.EnergyCycles == 10)
                {
                    engine.PlaySound(9, engine.SoundSet.EnergyOut);
                }
                else if (engine.World.EnergyCycles <= 0)
                {
                    engine.ForcePlayerColor(index);
                }
            }

            // Time limit logic

            if (engine.Board.TimeLimit > 0)
            {
                if (engine.World.Health > 0)
                {
                    if (engine.GetPlayerTimeElapsed(100))
                    {
                        engine.World.TimePassed++;
                        if (engine.Board.TimeLimit - 10 == engine.World.TimePassed)
                        {
                            engine.SetMessage(0xC8, engine.Alerts.TimeMessage);
                            engine.PlaySound(3, engine.SoundSet.TimeLow);
                        }
                        else if (engine.World.TimePassed >= engine.Board.TimeLimit)
                        {
                            engine.Harm(0);
                        }
                        engine.UpdateStatus();
                    }
                }
            }

            engine.MoveActorOnRiver(index);
        }
    }
}