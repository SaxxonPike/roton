using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Interactions;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the player element.
/// </summary>
[Context(Context.Original, 0x04)]
[Context(Context.Super, 0x04)]
internal sealed class PlayerAction(
    IEngineAccessor engine,
    IActorList actors,
    IElementList elements,
    IWorld world,
    IState state,
    ITiles tiles,
    IAlerts alerts,
    IBoard board,
    IHud hud,
    ISounds sounds,
    IFacts facts,
    IInteractionList interactions,
    ITimers timers,
    IConfig config,
    ISoundUnit soundUnit,
    IWorldUnit worldUnit,
    IBoardUpdater boardUpdater,
    IRadiusUpdater radiusUpdater,
    ISpawner spawner,
    IMover mover,
    IPlayerUpdater playerUpdater,
    IMessenger messenger,
    IDialogs dialogs,
    IPlayerInputHandler playerInputHandler,
    ICheater cheater)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actors[index];
        var playerElement = elements.Player();

        // Energizer graphics

        if (world.EnergyCycles > 0)
        {
            playerElement.Character = playerElement.Character == 1 ? 2 : 1;

            if ((state.GameCycle & 0x01) == 0)
                tiles[actor.Location].Color = ((state.GameCycle % 7 + 1) << 4) | 0x0F;
            else
                tiles[actor.Location].Color = 0x0F;

            boardUpdater.UpdateBoard(actor.Location);
        }
        else
        {
            playerUpdater.ForcePlayerColor(index);
        }

        // Health logic

        if (world.Health <= 0)
        {
            state.KeyVector = new Vector(0, 0);
            state.KeyShift = false;

            if (actors.ActorIndexAt(new Location(0, 0)) == -1)
                messenger.SetMessage(0x7D00, alerts.GameOverMessage);

            state.GameWaitTime = 0;
            state.GameOver = true;
        }

        // In the Original engine, the check for player shooting is a little more complex.
        // In the Super engine, pressing Space is reinterpreted as Shift + Last Direction.
        // We use the Super method here for simplicity.

        if (state.KeyPressed == EngineKeyCode.Space)
        {
            state.KeyVector = state.KeyLastVector;
            state.KeyShift = true;
        }

        if (state.KeyVector.IsNonZero() && state.KeyShift)
        {
            // Shooting logic

            if (board.MaximumShots > 0)
            {
                if (world.Ammo > 0)
                {
                    var bulletCount = actors.Count(a => a.P1 == 0 && tiles[a.Location].Id == elements.BulletId);

                    if (bulletCount < board.MaximumShots)
                    {
                        if (spawner.SpawnProjectile(elements.BulletId, actor.Location, state.KeyVector, false))
                        {
                            world.Ammo--;
                            hud.UpdateStatus();
                            soundUnit.PlaySound(2, sounds.Shoot);
                        }
                    }
                }
                else
                {
                    if (alerts.OutOfAmmo)
                    {
                        messenger.SetMessage(facts.LongMessageDuration, alerts.NoAmmoMessage);
                        alerts.OutOfAmmo = false;
                    }
                }
            }
            else
            {
                if (alerts.CantShootHere)
                {
                    messenger.SetMessage(facts.LongMessageDuration, alerts.NoShootMessage);
                    alerts.CantShootHere = false;
                }
            }
        }
        else if (state.KeyVector.IsNonZero())
        {
            // Movement logic

            interactions.Get(tiles[actor.Location + state.KeyVector].Id)?
                .Interact(actor.Location + state.KeyVector, 0, ref state.KeyVector);

            if (!state.KeyVector.IsZero())
            {
                if (!state.SoundPlaying)
                    soundUnit.PlayStep();

                if (tiles.ElementAt(actor.Location + state.KeyVector).IsFloor)
                    mover.MoveActor(0, actor.Location + state.KeyVector);
            }
        }

        // Hotkey logic

        switch (state.KeyPressed.ToUpperCase())
        {
            case EngineKeyCode.Q:
            case EngineKeyCode.Escape:
            {
                state.BreakGameLoop = state.GameOver || hud.EndGameConfirmation();
                break;
            }
            case EngineKeyCode.S:
            {
                if (hud.SaveGame() is { } saveFileName)
                    worldUnit.SaveWorld(saveFileName);

                break;
            }
            case EngineKeyCode.P:
            {
                if (world.Health > 0)
                    state.GamePaused = true;

                break;
            }
            case EngineKeyCode.B:
            {
                state.GameQuiet = !state.GameQuiet;
                soundUnit.ClearSound();
                hud.UpdateStatus();
                state.KeyPressed = EngineKeyCode.Space;
                break;
            }
            case EngineKeyCode.H:
            {
                dialogs.ShowHelp();
                break;
            }
            case EngineKeyCode.QuestionMark:
            {
                cheater.Cheat();
                break;
            }
            default:
            {
                playerInputHandler.HandlePlayerInput(actor);
                break;
            }
        }

        // Torch logic

        if (world.TorchCycles > 0)
        {
            world.TorchCycles--;

            if (world.TorchCycles <= 0)
            {
                radiusUpdater.UpdateRadius(actor.Location, RadiusMode.Update);
                soundUnit.PlaySound(3, sounds.TorchOut);
            }

            if (world.TorchCycles % 40 == 0)
                hud.UpdateStatus();
        }

        // Energizer logic

        if (world.EnergyCycles > 0)
        {
            world.EnergyCycles--;

            if (world.EnergyCycles == 10)
                soundUnit.PlaySound(9, sounds.EnergyOut);
            else if (world.EnergyCycles <= 0)
                playerUpdater.ForcePlayerColor(index);
        }

        // Time limit logic

        if (board.TimeLimit > 0)
        {
            if (world.Health > 0)
            {
                if (timers.TimeLimit.Clock(Engine.ResetBoardTimeHsec(), 100) > 0)
                {
                    world.TimePassed++;

                    if (!config.NoPesterMode && board.TimeLimit - 10 == world.TimePassed)
                    {
                        messenger.SetMessage(facts.LongMessageDuration, alerts.TimeMessage);
                        soundUnit.PlaySound(3, sounds.TimeLow);
                    }
                    else if (world.TimePassed >= board.TimeLimit)
                    {
                        Engine.Harm(0);
                    }

                    hud.UpdateStatus();
                }
            }
        }

        mover.MoveActorOnRiver(index);
    }
}