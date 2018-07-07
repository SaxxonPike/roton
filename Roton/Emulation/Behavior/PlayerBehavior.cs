using System.Linq;
using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.SuperZZT;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class PlayerBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly IState _state;
        private readonly ITiles _tiles;
        private readonly IWorld _world;
        private readonly IEngine _engine;
        private readonly IAlerts _alerts;
        private readonly IBoard _board;
        private readonly ISounds _sounds;
        private readonly IHud _hud;
        private readonly ISounder _sounder;
        private readonly IMover _mover;
        private readonly IDrawer _drawer;
        private readonly IPlotter _plotter;
        private readonly IMessager _messager;
        private readonly ISpawner _spawner;
        private readonly IRadius _radius;
        private readonly IMisc _misc;

        public override string KnownName => KnownNames.Player;

        public PlayerBehavior(IActors actors, IElements elements, IState state, ITiles tiles, IWorld world,
            IEngine engine, IAlerts alerts, IBoard board, ISounds sounds, IHud hud, ISounder sounder,
            IMover mover, IDrawer drawer, IPlotter plotter, IMessager messager, ISpawner spawner, IRadius radius,
            IMisc misc)
        {
            _actors = actors;
            _elements = elements;
            _state = state;
            _tiles = tiles;
            _world = world;
            _engine = engine;
            _alerts = alerts;
            _board = board;
            _sounds = sounds;
            _hud = hud;
            _sounder = sounder;
            _mover = mover;
            _drawer = drawer;
            _plotter = plotter;
            _messager = messager;
            _spawner = spawner;
            _radius = radius;
            _misc = misc;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            var playerElement = _elements[_elements.PlayerId];

            // Energizer graphics

            if (_world.EnergyCycles > 0)
            {
                playerElement.Character = playerElement.Character == 1 ? 2 : 1;

                if ((_state.GameCycle & 0x01) == 0)
                {
                    _tiles[actor.Location].Color = ((_state.GameCycle % 7 + 1) << 4) | 0x0F;
                }
                else
                {
                    _tiles[actor.Location].Color = 0x0F;
                }

                _drawer.UpdateBoard(actor.Location);
            }
            else
            {
                _plotter.ForcePlayerColor(index);
            }

            // Health logic

            if (_world.Health <= 0)
            {
                _state.KeyVector.SetTo(0, 0);
                _state.KeyShift = false;
                if (_actors.ActorIndexAt(new Location(0, 0)) == -1)
                {
                    _messager.SetMessage(0x7D00, _alerts.GameOverMessage);
                }

                _state.GameWaitTime = 0;
                _state.GameOver = true;
            }

            if (_state.KeyVector.IsNonZero())
            {
                if (_state.KeyShift || _state.KeyPressed == 0x20)
                {
                    // Shooting logic

                    if (_board.MaximumShots > 0)
                    {
                        if (_world.Ammo > 0)
                        {
                            var bulletCount =
                                _actors.Count(
                                    a => a.P1 == 0 && _tiles[a.Location].Id == _elements.BulletId);
                            if (bulletCount < _board.MaximumShots)
                            {
                                if (_spawner.SpawnProjectile(_elements.BulletId, actor.Location,
                                    _state.KeyVector, false))
                                {
                                    _world.Ammo--;
                                    _hud.UpdateStatus();
                                    _sounder.Play(2, _sounds.Shoot);
                                }
                            }
                        }
                        else
                        {
                            if (_alerts.OutOfAmmo)
                            {
                                _messager.SetMessage(0xC8, _alerts.NoAmmoMessage);
                                _alerts.OutOfAmmo = false;
                            }
                        }
                    }
                    else
                    {
                        if (_alerts.CantShootHere)
                        {
                            _messager.SetMessage(0xC8, _alerts.NoShootMessage);
                            _alerts.CantShootHere = false;
                        }
                    }
                }
                else
                {
                    // Movement logic

                    _tiles.ElementAt(actor.Location.Sum(_state.KeyVector))
                        .Interact(actor.Location.Sum(_state.KeyVector), 0, _state.KeyVector);
                    if (!_state.KeyVector.IsZero())
                    {
                        if (!_state.SoundPlaying)
                        {
                            // TODO: player step sound plays here
                        }

                        if (_tiles.ElementAt(actor.Location.Sum(_state.KeyVector)).IsFloor)
                        {
                            _mover.MoveActor(0, actor.Location.Sum(_state.KeyVector));
                        }
                    }
                }
            }

            // Hotkey logic

            var hotkey = _state.KeyPressed.ToUpperCase();
            switch (hotkey)
            {
                case 0x51: // Q
                case 0x1B: // escape
                    _state.BreakGameLoop = _state.GameOver || _hud.EndGameConfirmation();
                    break;
                case 0x53: // S
                    _hud.SaveGame();
                    break;
                case 0x50: // P
                    if (_world.Health > 0)
                    {
                        _state.GamePaused = true;
                    }

                    break;
                case 0x42: // B
                    _state.GameQuiet = !_state.GameQuiet;
                    _sounder.Clear();
                    _hud.UpdateStatus();
                    _state.KeyPressed = 0x20;
                    break;
                case 0x48: // H
                    _engine.ShowInGameHelp();
                    break;
                case 0x3F: // ?
                    _hud.EnterCheat();
                    break;
                default:
                    _misc.HandlePlayerInput(actor, hotkey);
                    break;
            }

            // Torch logic

            if (_world.TorchCycles > 0)
            {
                _world.TorchCycles--;
                if (_world.TorchCycles <= 0)
                {
                    _radius.Update(actor.Location, RadiusMode.Update);
                    _sounder.Play(3, _sounds.TorchOut);
                }

                if (_world.TorchCycles % 40 == 0)
                {
                    _hud.UpdateStatus();
                }
            }

            // Energizer logic

            if (_world.EnergyCycles > 0)
            {
                _world.EnergyCycles--;
                if (_world.EnergyCycles == 10)
                {
                    _sounder.Play(9, _sounds.EnergyOut);
                }
                else if (_world.EnergyCycles <= 0)
                {
                    _plotter.ForcePlayerColor(index);
                }
            }

            // Time limit logic

            if (_board.TimeLimit > 0)
            {
                if (_world.Health > 0)
                {
                    if (_engine.GetPlayerTimeElapsed(100))
                    {
                        _world.TimePassed++;
                        if (_board.TimeLimit - 10 == _world.TimePassed)
                        {
                            _messager.SetMessage(0xC8, _alerts.TimeMessage);
                            _sounder.Play(3, _sounds.TimeLow);
                        }
                        else if (_world.TimePassed >= _board.TimeLimit)
                        {
                            _mover.Harm(0);
                        }

                        _hud.UpdateStatus();
                    }
                }
            }

            _mover.MoveActorOnRiver(index);
        }
    }
}