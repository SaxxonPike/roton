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
    IHud _hud,
    IState _state,
    IActorList _actors,
    IActionList _actionList,
    ITiles _tiles,
    ITimers _timers,
    IElementList _elements,
    IBoardUpdater _boardUpdater,
    IWorld _world,
    IInputReader _inputReader,
    IInteractionList _interactions,
    IMover _mover,
    IPlayerUpdater _playerUpdater,
    IRadiusUpdater _radiusUpdater,
    IRandomizer _randomizer,
    IFacts _facts,
    IScheduler _scheduler,
    ITracer _tracer,
    ISoundUnit _soundUnit,
    IDialogs _dialogs,
    IWorldUnit _worldUnit,
    IMessenger _messenger,
    IFader _fader,
    IPlayField _playField,
    IHighScoreListFactory _highScoreListFactory,
    IGameThread _gameThread,
    IConfig _config
    )
    : IGame
{
    private void MainLoopInit(bool doFade)
    {
        if (_state.Init)
        {
            if (!_state.AboutShown)
                _dialogs.ShowAbout();

            if (!_gameThread.ThreadActive)
                return;

            if (_state.DefaultWorldName.Length > 0)
            {
                _state.AboutShown = true;
                _worldUnit.LoadWorld(_state.DefaultWorldName, false);
            }

            _state.StartBoard = _world.BoardIndex;
            _worldUnit.SetBoard(0);
            _state.Init = false;
        }

        var element = _elements[_state.PlayerElement];
        _tiles[_actors.Player.Location] = new Tile(element.Id, element.Color);
        if (_state.PlayerElement == _elements.MonitorId)
        {
            _messenger.SetMessage(0, new Message());
            _hud.DrawTitleStatus();
        }

        if (doFade)
            _fader.FadePurple();

        ResetGameSpeed();
        _state.GameCycle = _randomizer.GetNext(_facts.MainLoopRandomCycleRange);
        _state.ActIndex = _state.ActorCount + 1;
    }

    private void ResetGameSpeed() =>
        _state.GameWaitTime = _state.GameSpeed << 1;

    public void MainLoop(bool doFade)
    {
        var alternating = false;

        if (!_gameThread.Step)
        {
            _hud.CreateStatusText();
            _hud.UpdateStatus();
            MainLoopInit(doFade);
        }

        _state.BreakGameLoop = false;

        while (_gameThread.ThreadActive)
        {
            if (!_state.GamePaused)
            {
                if (_state.ActIndex <= _state.ActorCount)
                {
                    var actorData = _actors[_state.ActIndex];
                    if (actorData.Cycle != 0)
                        if (_state.ActIndex % actorData.Cycle == _state.GameCycle % actorData.Cycle)
                            _actionList.Get(_tiles[actorData.Location].Id)?.Act(_state.ActIndex);

                    _state.ActIndex++;
                }
            }
            else
            {
                _state.ActIndex = _state.ActorCount + 1;

                if (_timers.Player.Clock(1, HsecToTicks(25)) > 0)
                    alternating = !alternating;

                if (alternating)
                {
                    var playerElement = _elements.Player();
                    DrawTile(_actors.Player.Location, new AnsiChar(playerElement.Character, playerElement.Color));
                }
                else
                {
                    if (_tiles[_actors.Player.Location].Id == _elements.PlayerId)
                        DrawTile(_actors.Player.Location, new AnsiChar(0x20, 0x0F));
                    else
                        _boardUpdater.UpdateBoard(_actors.Player.Location);
                }

                _hud.DrawPausing();
                _inputReader.Read(false);
                if (_state.KeyPressed == EngineKeyCode.Escape)
                {
                    if (_world.Health > 0)
                    {
                        _state.BreakGameLoop = _hud.EndGameConfirmation();
                    }
                    else
                    {
                        _state.BreakGameLoop = true;
                        _hud.UpdateBorder();
                    }

                    _state.KeyPressed = 0;
                }

                if (!_state.KeyVector.IsZero())
                {
                    var target = _actors.Player.Location + _state.KeyVector;
                    _interactions.Get(_tiles.ElementAt(target).Id)?.Interact(target, 0, ref _state.KeyVector);
                }

                if (!_state.KeyVector.IsZero())
                {
                    var target = _actors.Player.Location + _state.KeyVector;
                    if (_tiles.ElementAt(target).IsFloor)
                    {
                        if (_tiles.ElementAt(_actors.Player.Location).Id == _elements.PlayerId)
                        {
                            _mover.MoveActor(0, target);
                        }
                        else
                        {
                            _boardUpdater.UpdateBoard(_actors.Player.Location);
                            _actors.Player.Location += _state.KeyVector;
                            _playerUpdater.CleanUpPauseMovement();
                            _tiles[_actors.Player.Location] = new Tile(_elements.PlayerId, _elements.Player().Color);
                            _boardUpdater.UpdateBoard(_actors.Player.Location);
                            _radiusUpdater.UpdateRadius(_actors.Player.Location, RadiusMode.Update);
                            _radiusUpdater.UpdateRadius(_actors.Player.Location - _state.KeyVector, RadiusMode.Update);
                        }

                        _state.GamePaused = false;
                        _hud.ClearPausing();
                        _state.GameCycle = _randomizer.GetNext(_facts.MainLoopRandomCycleRange);
                        _world.IsLocked = true;
                    }
                    else
                    {
                        // Added so that attempting to run into a wall while paused using
                        // a joystick doesn't cause the game to freeze (the original engine
                        // just added delays)
                        _scheduler.WaitForTick();
                    }
                }
            }

            if (_state.ActIndex > _state.ActorCount)
            {
                if (!_state.BreakGameLoop && !_state.GamePaused)
                    if (_state.GameWaitTime <= 0 || _timers.Player.Clock(1, _state.GameWaitTime) > 0)
                    {
                        _state.GameCycle++;
                        if (_state.GameCycle > _facts.MaxGameCycle) _state.GameCycle = 1;

                        _state.ActIndex = 0;
                        _inputReader.Read(false);
                    }

                _tracer.TraceStep();
                if (_gameThread.Step)
                    break;

                _scheduler.WaitForTick();
            }

            if (_state.BreakGameLoop)
            {
                _soundUnit.ClearSound();
                if (_state.PlayerElement == _elements.PlayerId)
                {
                    // This game speed reset isn't here in the original code,
                    // but it solves some issues with game speed when returning
                    // to the title screen.

                    ResetGameSpeed();

                    if (_world.Health <= 0)
                        EnterHighScore(_world.Score);
                }
                else if (_state.PlayerElement == _elements.MonitorId)
                {
                    _hud.ClearTitleStatus();
                }

                var element = _elements.Player();
                _tiles[_actors.Player.Location] = new Tile(element.Id, element.Color);
                _state.GameOver = false;
                break;
            }
        }
    }
    
    private void DrawTile(Location location, AnsiChar ac) => 
        _playField.DrawTile(location.X - 1, location.Y - 1, ac);

    private void EnterHighScore(int score)
    {
        if (score <= 0)
            return;

        var list = _highScoreListFactory.Load();
        var name = _hud.EnterHighScore(list, score);
        if (name == null)
            return;

        list.Add(name, score);
        _highScoreListFactory.Save(list);
        _dialogs.ShowHighScores();
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
        Math.Max(1, hsec * (_config.MasterClockDenominator / _config.MasterClockNumerator + 50) / 100);

}