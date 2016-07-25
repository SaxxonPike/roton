using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class PlayerBehavior : ElementBehavior
    {
        public override string KnownName => "Player";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            var playerElement = engine.Elements.PlayerElement;

            // Energizer graphics

            if (engine.WorldData.EnergyCycles > 0)
            {
                playerElement.Character = playerElement.Character == 1 ? 2 : 1;

                if ((engine.StateData.GameCycle & 0x01) == 0)
                {
                    engine.Tiles[actor.Location].Color = ((engine.StateData.GameCycle % 7 + 1) << 4) | 0x0F;
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

            if (engine.WorldData.Health <= 0)
            {
                engine.StateData.KeyVector.SetTo(0, 0);
                engine.StateData.KeyShift = false;
                if (engine.ActorIndexAt(new Location(0, 0)) == -1)
                {
                    engine.SetMessage(0x7D00, engine.Alerts.GameOverMessage);
                }
                engine.StateData.GameWaitTime = 0;
                engine.StateData.GameOver = true;
            }

            if (engine.StateData.KeyVector.IsNonZero())
            {
                if (engine.StateData.KeyShift || engine.StateData.KeyPressed == 0x20)
                {
                    // Shooting logic

                    if (engine.Board.Shots > 0)
                    {
                        if (engine.WorldData.Ammo > 0)
                        {
                            var bulletCount = engine.Actors.Count(a => a.P1 == 0 && engine.Tiles[a.Location].Id == engine.Elements.BulletId);
                            if (bulletCount < engine.Board.Shots)
                            {
                                if (engine.SpawnProjectile(engine.Elements.BulletId, actor.Location, engine.StateData.KeyVector, false))
                                {
                                    engine.WorldData.Ammo--;
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

                    engine.ElementAt(actor.Location.Sum(engine.StateData.KeyVector)).Interact(engine, actor.Location.Sum(engine.StateData.KeyVector), 0, engine.StateData.KeyVector);
                    if (!engine.StateData.KeyVector.IsZero())
                    {
                        if (!engine.StateData.SoundPlaying)
                        {
                            // TODO: player step sound plays here
                        }
                        if (engine.ElementAt(actor.Location.Sum(engine.StateData.KeyVector)).IsFloor)
                        {
                            engine.MoveActor(0, actor.Location.Sum(engine.StateData.KeyVector));
                        }
                    }
                }
            }

            // Hotkey logic

            switch (engine.StateData.KeyPressed.ToUpperCase())
            {
                case 0x54: // T
                    if (engine.TorchesEnabled)
                    {
                        if (engine.WorldData.TorchCycles <= 0)
                        {
                            if (engine.WorldData.Torches <= 0)
                            {
                                if (engine.Alerts.NoTorches)
                                {
                                    engine.SetMessage(0xC8, engine.Alerts.NoTorchMessage);
                                    engine.Alerts.NoTorches = false;
                                }
                            }
                            else if (!engine.Board.Dark)
                            {
                                if (engine.Alerts.NotDark)
                                {
                                    engine.SetMessage(0xC8, engine.Alerts.NotDarkMessage);
                                    engine.Alerts.NotDark = false;
                                }
                            }
                            else
                            {
                                engine.WorldData.Torches--;
                                engine.WorldData.TorchCycles = 0xC8;
                                engine.UpdateRadius(actor.Location, RadiusMode.Update);
                                engine.UpdateStatus();
                            }
                        }
                    }
                    break;
                case 0x51: // Q
                case 0x1B: // escape
                    engine.StateData.BreakGameLoop = engine.StateData.GameOver || engine.Hud.EndGameConfirmation();
                    break;
                case 0x53: // S
                    break;
                case 0x50: // P
                    if (engine.WorldData.Health > 0)
                    {
                        engine.StateData.GamePaused = true;
                    }
                    break;
                case 0x42: // B
                    engine.StateData.GameQuiet = !engine.StateData.GameQuiet;
                    engine.ClearSound();
                    engine.UpdateStatus();
                    engine.StateData.KeyPressed = 0x20;
                    break;
                case 0x48: // H
                    engine.ShowInGameHelp();
                    break;
                case 0x46: // F
                    break;
                case 0x3F: // ?
                    break;
            }

            // Torch logic

            if (engine.WorldData.TorchCycles > 0)
            {
                engine.WorldData.TorchCycles--;
                if (engine.WorldData.TorchCycles <= 0)
                {
                    engine.UpdateRadius(actor.Location, RadiusMode.Update);
                    engine.PlaySound(3, engine.SoundSet.TorchOut);
                }

                if (engine.WorldData.TorchCycles % 40 == 0)
                {
                    engine.UpdateStatus();
                }
            }

            // Energizer logic

            if (engine.WorldData.EnergyCycles > 0)
            {
                engine.WorldData.EnergyCycles--;
                if (engine.WorldData.EnergyCycles == 10)
                {
                    engine.PlaySound(9, engine.SoundSet.EnergyOut);
                }
                else if (engine.WorldData.EnergyCycles <= 0)
                {
                    engine.ForcePlayerColor(index);
                }
            }

            // Time limit logic

            if (engine.Board.TimeLimit > 0)
            {
                if (engine.WorldData.Health > 0)
                {
                    if (engine.GetPlayerTimeElapsed(100))
                    {
                        engine.WorldData.TimePassed++;
                        if (engine.Board.TimeLimit - 10 == engine.WorldData.TimePassed)
                        {
                            engine.SetMessage(0xC8, engine.Alerts.TimeMessage);
                            engine.PlaySound(3, engine.SoundSet.TimeLow);
                        }
                        else if (engine.WorldData.TimePassed >= engine.Board.TimeLimit)
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
