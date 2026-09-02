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
    IWorldUnit worldUnit,
    IConfigFileService configFileService,
    IClock clock,
    ITitleScreen titleScreen,
    IScheduler scheduler,
    IGameThread gameThread,
    IElementList elements)
    : IBootstrap
{
    public event EventHandler? Exited;

    public void Start()
    {
        if (gameThread.Current != null)
            return;

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
        state.GameSpeed = facts.DefaultGameSpeed;
        state.GameWaitTime = 1;
        state.DefaultSaveName = facts.DefaultSavedGameName;
        state.DefaultBoardName = facts.DefaultBoardName;
        state.DefaultWorldName = config.DefaultWorld ?? facts.DefaultWorldName;
        state.ForestIndex = 2;
        state.Init = true;

        worldUnit.ClearWorld();

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

        SetGameMode();
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

    private void SetGameMode()
    {
        InitializeElements(false);
        state.EditorMode = false;
    }

    private void InitializeElements(bool showInvisibleTiles)
    {
        elements.Reset();
        elements.Invisible().Character = showInvisibleTiles ? 0xB0 : 0x20;
        elements.Invisible().Color = 0xFF;
        elements.Player().Character = 0x02;
    }
}