using System;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [Context(Context.Original, 0x04)]
    [Context(Context.Super, 0x04)]
    public sealed class PlayerAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public PlayerAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = Engine.Actors[index];
            var playerElement = Engine.ElementList.Player();

            // Energizer graphics

            if (Engine.World.EnergyCycles > 0)
            {
                playerElement.Character = playerElement.Character == 1 ? 2 : 1;

                if ((Engine.State.GameCycle & 0x01) == 0)
                {
                    Engine.Tiles[actor.Location].Color = ((Engine.State.GameCycle % 7 + 1) << 4) | 0x0F;
                }
                else
                {
                    Engine.Tiles[actor.Location].Color = 0x0F;
                }

                Engine.UpdateBoard(actor.Location);
            }
            else
            {
                Engine.ForcePlayerColor(index);
            }

            // Health logic

            if (Engine.World.Health <= 0)
            {
                Engine.State.KeyVector.SetTo(0, 0);
                Engine.State.KeyShift = false;
                if (Engine.Actors.ActorIndexAt(new Location(0, 0)) == -1)
                {
                    Engine.SetMessage(0x7D00, Engine.Alerts.GameOverMessage);
                }

                Engine.State.GameWaitTime = 0;
                Engine.State.GameOver = true;
            }

            if (Engine.State.KeyVector.IsNonZero())
            {
                if ((Engine.State.KeyArrow && Engine.State.KeyShift) || Engine.State.KeyPressed == EngineKeyCode.Space)
                {
                    // Shooting logic

                    if (Engine.Board.MaximumShots > 0)
                    {
                        if (Engine.World.Ammo > 0)
                        {
                            var bulletCount =
                                Engine.Actors.Count(
                                    a => a.P1 == 0 && Engine.Tiles[a.Location].Id == Engine.ElementList.BulletId);
                            if (bulletCount < Engine.Board.MaximumShots)
                            {
                                if (Engine.SpawnProjectile(Engine.ElementList.BulletId, actor.Location,
                                    Engine.State.KeyVector, false))
                                {
                                    Engine.World.Ammo--;
                                    Engine.Hud.UpdateStatus();
                                    Engine.PlaySound(2, Engine.Sounds.Shoot);
                                }
                            }
                        }
                        else
                        {
                            if (Engine.Alerts.OutOfAmmo)
                            {
                                Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.NoAmmoMessage);
                                Engine.Alerts.OutOfAmmo = false;
                            }
                        }
                    }
                    else
                    {
                        if (Engine.Alerts.CantShootHere)
                        {
                            Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.NoShootMessage);
                            Engine.Alerts.CantShootHere = false;
                        }
                    }
                }
                else if (Engine.State.KeyArrow)
                {
                    // Movement logic

                    Engine.InteractionList.Get(Engine.Tiles[actor.Location.Sum(Engine.State.KeyVector)].Id)
                        .Interact(actor.Location.Sum(Engine.State.KeyVector), 0, Engine.State.KeyVector);
                    
                    if (!Engine.State.KeyVector.IsZero())
                    {
                        if (!Engine.State.SoundPlaying)
                        {
                            Engine.PlayStep();
                        }

                        if (Engine.Tiles.ElementAt(actor.Location.Sum(Engine.State.KeyVector)).IsFloor)
                        {
                            Engine.MoveActor(0, actor.Location.Sum(Engine.State.KeyVector));
                        }
                    }
                }
            }

            // Hotkey logic

            switch (Engine.State.KeyPressed.ToUpperCase())
            {
                case EngineKeyCode.Q:
                case EngineKeyCode.Escape:
                    Engine.State.BreakGameLoop = Engine.State.GameOver || Engine.Hud.EndGameConfirmation();
                    break;
                case EngineKeyCode.S:
                    Engine.Hud.SaveGame();
                    break;
                case EngineKeyCode.P:
                    if (Engine.World.Health > 0)
                    {
                        Engine.State.GamePaused = true;
                    }

                    break;
                case EngineKeyCode.B:
                    Engine.State.GameQuiet = !Engine.State.GameQuiet;
                    Engine.ClearSound();
                    Engine.Hud.UpdateStatus();
                    Engine.State.KeyPressed = EngineKeyCode.Space;
                    break;
                case EngineKeyCode.H:
                    Engine.ShowInGameHelp();
                    break;
                case EngineKeyCode.QuestionMark:
                    Engine.Cheat();
                    break;
                default:
                    Engine.HandlePlayerInput(actor);
                    break;
            }

            // Torch logic

            if (Engine.World.TorchCycles > 0)
            {
                Engine.World.TorchCycles--;
                if (Engine.World.TorchCycles <= 0)
                {
                    Engine.UpdateRadius(actor.Location, RadiusMode.Update);
                    Engine.PlaySound(3, Engine.Sounds.TorchOut);
                }

                if (Engine.World.TorchCycles % 40 == 0)
                {
                    Engine.Hud.UpdateStatus();
                }
            }

            // Energizer logic

            if (Engine.World.EnergyCycles > 0)
            {
                Engine.World.EnergyCycles--;
                if (Engine.World.EnergyCycles == 10)
                {
                    Engine.PlaySound(9, Engine.Sounds.EnergyOut);
                }
                else if (Engine.World.EnergyCycles <= 0)
                {
                    Engine.ForcePlayerColor(index);
                }
            }

            // Time limit logic

            if (Engine.Board.TimeLimit > 0)
            {
                if (Engine.World.Health > 0)
                {
                    if (Engine.Timers.TimeLimit.Clock(100))
                    {
                        Engine.World.TimePassed++;
                        if (Engine.Board.TimeLimit - 10 == Engine.World.TimePassed)
                        {
                            Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.TimeMessage);
                            Engine.PlaySound(3, Engine.Sounds.TimeLow);
                        }
                        else if (Engine.World.TimePassed >= Engine.Board.TimeLimit)
                        {
                            Engine.Harm(0);
                        }

                        Engine.Hud.UpdateStatus();
                    }
                }
            }

            Engine.MoveActorOnRiver(index);
        }
    }
}