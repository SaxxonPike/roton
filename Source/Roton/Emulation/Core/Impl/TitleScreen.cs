using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class TitleScreen(
    IState state,
    IHud hud,
    IWorldManager worldManager,
    IElementList elements,
    IPlayerInputHandler playerInputHandler,
    IWorld world,
    IPlayerEnterHandler playerEnterHandler,
    IGame game,
    IGameThread gameThread)
    : ITitleScreen
{
    private bool PlayWorld()
    {
        var gameIsActive = false;

        if (world.IsLocked)
        {
            worldManager.LoadWorld(world.Name, false);

            if (state.WorldLoaded)
            {
                gameIsActive = state.WorldLoaded;
                state.StartBoard = world.BoardIndex;
            }
        }
        else
        {
            gameIsActive = true;
        }

        if (gameIsActive)
            StartPlaying();

        return gameIsActive;
    }

    private void StartPlaying()
    {
        worldManager.SetBoard(state.StartBoard);
        playerEnterHandler.EnterBoard();
        state.PlayerElement = elements.PlayerId;
        state.GamePaused = true;
        game.MainLoop(true);
    }

    public void TitleScreenLoop()
    {
        state.QuitEngine = false;
        state.Init = true;
        state.StartBoard = 0;
        var gameEnded = true;
        hud.Initialize();
        while (gameThread.ThreadActive)
        {
            if (!state.Init)
                worldManager.SetBoard(0);

            while (gameThread.ThreadActive)
            {
                state.PlayerElement = elements.MonitorId;
                state.GamePaused = false;
                game.MainLoop(gameEnded);
                gameEnded = false;

                if (!gameThread.ThreadActive)
                    break;

                var startPlaying = playerInputHandler.HandleTitleInput();
                if (startPlaying)
                    gameEnded = PlayWorld();

                if (gameEnded || state.QuitEngine)
                    break;
            }

            if (state.QuitEngine) break;
        }
    }
}