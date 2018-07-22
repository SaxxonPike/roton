﻿using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x04)]
    [ContextEngine(ContextEngine.Super, 0x04)]
    public sealed class PlayerAction : IAction
    {
        private readonly IEngine _engine;

        public PlayerAction(IEngine engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            var playerElement = _engine.ElementList[_engine.ElementList.PlayerId];

            // Energizer graphics

            if (_engine.World.EnergyCycles > 0)
            {
                playerElement.Character = playerElement.Character == 1 ? 2 : 1;

                if ((_engine.State.GameCycle & 0x01) == 0)
                {
                    _engine.Tiles[actor.Location].Color = ((_engine.State.GameCycle % 7 + 1) << 4) | 0x0F;
                }
                else
                {
                    _engine.Tiles[actor.Location].Color = 0x0F;
                }

                _engine.UpdateBoard(actor.Location);
            }
            else
            {
                _engine.ForcePlayerColor(index);
            }

            // Health logic

            if (_engine.World.Health <= 0)
            {
                _engine.State.KeyVector.SetTo(0, 0);
                _engine.State.KeyShift = false;
                if (_engine.Actors.ActorIndexAt(new Location(0, 0)) == -1)
                {
                    _engine.SetMessage(0x7D00, _engine.Alerts.GameOverMessage);
                }

                _engine.State.GameWaitTime = 0;
                _engine.State.GameOver = true;
            }

            if (_engine.State.KeyVector.IsNonZero())
            {
                if (_engine.State.KeyShift || _engine.State.KeyPressed == EngineKeyCode.Space)
                {
                    // Shooting logic

                    if (_engine.Board.MaximumShots > 0)
                    {
                        if (_engine.World.Ammo > 0)
                        {
                            var bulletCount =
                                _engine.Actors.Count(
                                    a => a.P1 == 0 && _engine.Tiles[a.Location].Id == _engine.ElementList.BulletId);
                            if (bulletCount < _engine.Board.MaximumShots)
                            {
                                if (_engine.SpawnProjectile(_engine.ElementList.BulletId, actor.Location,
                                    _engine.State.KeyVector, false))
                                {
                                    _engine.World.Ammo--;
                                    _engine.Hud.UpdateStatus();
                                    _engine.PlaySound(2, _engine.Sounds.Shoot);
                                }
                            }
                        }
                        else
                        {
                            if (_engine.Alerts.OutOfAmmo)
                            {
                                _engine.SetMessage(0xC8, _engine.Alerts.NoAmmoMessage);
                                _engine.Alerts.OutOfAmmo = false;
                            }
                        }
                    }
                    else
                    {
                        if (_engine.Alerts.CantShootHere)
                        {
                            _engine.SetMessage(0xC8, _engine.Alerts.NoShootMessage);
                            _engine.Alerts.CantShootHere = false;
                        }
                    }
                }
                else
                {
                    // Movement logic

                    _engine.InteractionList.Get(_engine.Tiles[actor.Location.Sum(_engine.State.KeyVector)].Id)
                        .Interact(actor.Location.Sum(_engine.State.KeyVector), 0, _engine.State.KeyVector);
                    
                    if (!_engine.State.KeyVector.IsZero())
                    {
                        if (!_engine.State.SoundPlaying)
                        {
                            // TODO: player step sound plays here
                        }

                        if (_engine.Tiles.ElementAt(actor.Location.Sum(_engine.State.KeyVector)).IsFloor)
                        {
                            _engine.MoveActor(0, actor.Location.Sum(_engine.State.KeyVector));
                        }
                    }
                }
            }

            // Hotkey logic

            switch (_engine.State.KeyPressed.ToUpperCase())
            {
                case EngineKeyCode.Q:
                case EngineKeyCode.Escape:
                    _engine.State.BreakGameLoop = _engine.State.GameOver || _engine.Hud.EndGameConfirmation();
                    break;
                case EngineKeyCode.S:
                    _engine.Hud.SaveGame();
                    break;
                case EngineKeyCode.P:
                    if (_engine.World.Health > 0)
                    {
                        _engine.State.GamePaused = true;
                    }

                    break;
                case EngineKeyCode.B:
                    _engine.State.GameQuiet = !_engine.State.GameQuiet;
                    _engine.ClearSound();
                    _engine.Hud.UpdateStatus();
                    _engine.State.KeyPressed = EngineKeyCode.Space;
                    break;
                case EngineKeyCode.H:
                    _engine.ShowInGameHelp();
                    break;
                case EngineKeyCode.QuestionMark:
                    _engine.Hud.EnterCheat();
                    break;
                default:
                    _engine.HandlePlayerInput(actor);
                    break;
            }

            // Torch logic

            if (_engine.World.TorchCycles > 0)
            {
                _engine.World.TorchCycles--;
                if (_engine.World.TorchCycles <= 0)
                {
                    _engine.UpdateRadius(actor.Location, RadiusMode.Update);
                    _engine.PlaySound(3, _engine.Sounds.TorchOut);
                }

                if (_engine.World.TorchCycles % 40 == 0)
                {
                    _engine.Hud.UpdateStatus();
                }
            }

            // Energizer logic

            if (_engine.World.EnergyCycles > 0)
            {
                _engine.World.EnergyCycles--;
                if (_engine.World.EnergyCycles == 10)
                {
                    _engine.PlaySound(9, _engine.Sounds.EnergyOut);
                }
                else if (_engine.World.EnergyCycles <= 0)
                {
                    _engine.ForcePlayerColor(index);
                }
            }

            // Time limit logic

            if (_engine.Board.TimeLimit > 0)
            {
                if (_engine.World.Health > 0)
                {
                    if (_engine.Timers.TimeLimit.Clock(100))
                    {
                        _engine.World.TimePassed++;
                        if (_engine.Board.TimeLimit - 10 == _engine.World.TimePassed)
                        {
                            _engine.SetMessage(0xC8, _engine.Alerts.TimeMessage);
                            _engine.PlaySound(3, _engine.Sounds.TimeLow);
                        }
                        else if (_engine.World.TimePassed >= _engine.Board.TimeLimit)
                        {
                            _engine.Harm(0);
                        }

                        _engine.Hud.UpdateStatus();
                    }
                }
            }

            _engine.MoveActorOnRiver(index);
        }
    }
}