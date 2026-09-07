using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Bootstrap(
    IState state,
    IFacts facts,
    IConfig config,
    IWorldManager worldManager,
    IConfigFileService configFileService,
    IClock clock,
    ITitleScreen titleScreen,
    IScheduler scheduler,
    IGameThread gameThread,
    IRandomizer randomizer,
    IDrumSynthesizer drumSynthesizer,
    IDrumSoundList drumSoundList)
    : IBootstrap
{
    public event EventHandler? Exited;

    public void Start()
    {
        if (gameThread.Current != null)
            return;

        // While the randomizer is used to construct drum tables, this is done
        // prior to the program's first "randomize" call, so we need to configure
        // the random state precisely for this point to reproduce the proper
        // frequency tables. The first iteration of the randomizer is discarded.

        randomizer.Reset();
        InitializeDrums();

        randomizer.SetSeed(DateTime.Now);
        scheduler.Reset();
        gameThread.Start(StartMain);
    }

    public void Stop()
    {
        if (gameThread.Current == null)
            return;

        gameThread.Stop();
    }

    private void StartInit()
    {
        state.EditorMode = false;
        state.GameSpeed = facts.DefaultGameSpeed;
        state.GameWaitTime = 1;
        state.DefaultSaveName = facts.DefaultSavedGameName;
        state.DefaultBoardName = facts.DefaultBoardName;
        state.DefaultWorldName = config.DefaultWorld ?? facts.DefaultWorldName;
        state.ForestIndex = 2;
        state.Init = true;

        worldManager.ClearWorld();

        var cfg = configFileService.Load();
        if (config.DefaultWorld == null && cfg != null)
        {
            if (!string.IsNullOrEmpty(cfg.WorldName))
            {
                state.DefaultWorldName = (
                    cfg.WorldName?.StartsWith("*") ?? false
                        ? cfg.WorldName.Substring(1)
                        : cfg.WorldName
                ) ?? string.Empty;
            }
        }

        clock.Start();
    }

    private void StartMain()
    {
        clock.OnTick += scheduler.Advance;
        StartInit();
        titleScreen.TitleScreenLoop();
        clock.OnTick -= scheduler.Advance;
        Exited?.Invoke(this, EventArgs.Empty);
    }

    private void InitializeDrums()
    {
        for (var i = 0; i < drumSoundList.Count; i++)
            drumSynthesizer.Synthesize(i, drumSoundList[i]);
    }
}