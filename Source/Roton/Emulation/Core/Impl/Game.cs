using System;
using Roton.Emulation.Actions;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Interactions;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Game(
    IHud hud,
    IState state,
    IActorList actors,
    IActionList actions,
    ITiles tiles,
    ITimers timers,
    IElementList elements,
    IBoardUpdater boardUpdater,
    IWorld world,
    IInputReader inputReader,
    IInteractionList interactions,
    IMover mover,
    IPlayerUpdater playerUpdater,
    IRadiusUpdater radiusUpdater,
    IRandomizer randomizer,
    IFacts facts,
    IScheduler scheduler,
    ITracer tracer,
    ISoundPlayer soundPlayer,
    IDialogs dialogs,
    IWorldManager worldManager,
    IMessenger messenger,
    IFader fader,
    IPlayField playField,
    IHighScoreListFactory highScoreListFactory,
    IGameThread gameThread,
    IConfig config,
    IHighScoreHud highScoreHud
)
    : IGame
{
    private void MainLoopInit(bool doFade)
    {
        if (state.Init)
        {
            if (!state.AboutShown)
                dialogs.ShowAbout();

            if (!gameThread.ThreadActive)
                return;

            if (state.DefaultWorldName.Length > 0)
            {
                state.AboutShown = true;
                worldManager.LoadWorld(state.DefaultWorldName, false);
            }

            state.StartBoard = world.BoardIndex;
            worldManager.SetBoard(0);
            state.Init = false;
        }

        var element = elements[state.PlayerElement];
        tiles[actors.Player.Location] = new Tile(element.Id, element.Color);
        if (state.PlayerElement == elements.MonitorId)
        {
            messenger.SetMessage(0, new Message());
            hud.DrawTitleStatus();
        }

        if (doFade)
            fader.FadePurple();

        ResetGameSpeed();
        state.GameCycle = randomizer.GetNext(facts.MainLoopRandomCycleRange);
        state.ActIndex = state.ActorCount + 1;
    }

    private void ResetGameSpeed() =>
        state.GameWaitTime = state.GameSpeed << 1;

    public void MainLoop(bool doFade)
    {
        var alternating = false;

        if (gameThread.StepMode == StepMode.Normal)
        {
            hud.CreateStatusText();
            hud.UpdateStatus();
            MainLoopInit(doFade);
        }

        state.BreakGameLoop = false;

        while (gameThread.ThreadActive)
        {
            if (!state.GamePaused)
            {
                if (state.ActIndex <= state.ActorCount)
                {
                    var actorData = actors[state.ActIndex];
                    if (actorData.Cycle != 0)
                        if (state.ActIndex % actorData.Cycle == state.GameCycle % actorData.Cycle)
                            actions.Get(tiles[actorData.Location].Id)?.Act(state.ActIndex);

                    state.ActIndex++;
                }
            }
            else
            {
                state.ActIndex = state.ActorCount + 1;

                if (timers.Player.Clock(1, HsecToTicks(25)) > 0)
                    alternating = !alternating;

                if (alternating)
                {
                    var playerElement = elements.Player();
                    DrawTile(actors.Player.Location, new AnsiChar(playerElement.Character, playerElement.Color));
                }
                else
                {
                    if (tiles[actors.Player.Location].Id == elements.PlayerId)
                        DrawTile(actors.Player.Location, new AnsiChar(0x20, 0x0F));
                    else
                        boardUpdater.UpdateBoard(actors.Player.Location);
                }

                hud.DrawPausing();
                inputReader.Read(false);
                if (state.KeyPressed == EngineKeyCode.Escape)
                {
                    if (world.Health > 0)
                    {
                        state.BreakGameLoop = hud.EndGameConfirmation();
                    }
                    else
                    {
                        state.BreakGameLoop = true;
                        hud.UpdateBorder();
                    }

                    state.KeyPressed = 0;
                }

                if (!state.KeyVector.IsZero())
                {
                    var target = actors.Player.Location + state.KeyVector;
                    interactions.Get(tiles.ElementAt(target).Id)?.Interact(target, 0, ref state.KeyVector);
                }

                if (!state.KeyVector.IsZero())
                {
                    var target = actors.Player.Location + state.KeyVector;
                    if (tiles.ElementAt(target).IsFloor)
                    {
                        if (tiles.ElementAt(actors.Player.Location).Id == elements.PlayerId)
                        {
                            mover.Move(0, target);
                        }
                        else
                        {
                            boardUpdater.UpdateBoard(actors.Player.Location);
                            actors.Player.Location += state.KeyVector;
                            playerUpdater.CleanUpPauseMovement();
                            tiles[actors.Player.Location] = new Tile(elements.PlayerId, elements.Player().Color);
                            boardUpdater.UpdateBoard(actors.Player.Location);
                            radiusUpdater.UpdateRadius(actors.Player.Location, RadiusMode.Update);
                            radiusUpdater.UpdateRadius(actors.Player.Location - state.KeyVector, RadiusMode.Update);
                        }

                        state.GamePaused = false;
                        hud.ClearPausing();
                        state.GameCycle = randomizer.GetNext(facts.MainLoopRandomCycleRange);
                        world.IsLocked = true;
                    }
                    else
                    {
                        // Added so that attempting to run into a wall while paused using
                        // a joystick doesn't cause the game to freeze (the original engine
                        // just added delays)
                        scheduler.WaitForTick();
                    }
                }
            }

            if (state.ActIndex > state.ActorCount)
            {
                if (!state.BreakGameLoop && !state.GamePaused)
                    if (state.GameWaitTime <= 0 || timers.Player.Clock(1, state.GameWaitTime) > 0)
                    {
                        state.GameCycle++;
                        if (state.GameCycle > facts.MaxGameCycle) state.GameCycle = 1;

                        state.ActIndex = 0;
                        inputReader.Read(false);
                    }

                tracer.TraceStep();
                if (gameThread.StepMode != StepMode.Normal)
                    break;

                scheduler.WaitForTick();
            }

            if (state.BreakGameLoop)
            {
                soundPlayer.ClearSound();
                if (state.PlayerElement == elements.PlayerId)
                {
                    // This game speed reset isn't here in the original code,
                    // but it solves some issues with game speed when returning
                    // to the title screen.

                    ResetGameSpeed();

                    if (world.Health <= 0)
                        EnterHighScore(world.Score);
                }
                else if (state.PlayerElement == elements.MonitorId)
                {
                    hud.ClearTitleStatus();
                }

                var element = elements.Player();
                tiles[actors.Player.Location] = new Tile(element.Id, element.Color);
                state.GameOver = false;
                break;
            }
        }
    }

    private void DrawTile(Location location, AnsiChar ac) =>
        playField.DrawTile(location.X - 1, location.Y - 1, ac);

    private void EnterHighScore(int score)
    {
        if (score <= 0)
            return;

        var list = highScoreListFactory.Load();
        var name = highScoreHud.EnterHighScore(list, score);
        if (name == null)
            return;

        list.Add(name, score);
        highScoreListFactory.Save(list);
        highScoreHud.ShowHighScores(list);
        dialogs.ShowHighScores();
    }

    /// <summary>
    /// Converts hundredths of seconds to ticks.
    /// </summary>
    /// <param name="hsec">
    /// Duration in hundredths of seconds.
    /// </param>
    /// <returns>
    /// The equivalent number of ticks.
    /// </returns>
    private int HsecToTicks(int hsec) =>
        Math.Max(1, hsec * (config.MasterClockDenominator / config.MasterClockNumerator + 50) / 100);

    public void StepOnce()
    {
        var lastStep = gameThread.StepMode; 
        gameThread.StepMode = StepMode.Once;
        MainLoop(true);
        gameThread.StepMode = lastStep;
    }
}