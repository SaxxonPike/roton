using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Data
{
    public interface IFacts
    {
        int AmmoPerPickup { get; }
        int HealthPerGem { get; }
        int ScorePerGem { get; }
        int PauseFlashInterval { get; }
        int MainLoopRandomCycleRange { get; }
        string DefaultSavedGameName { get; }
        string DefaultBoardName { get; }
        string DefaultWorldName { get; }
        string UntitledWorldName { get; }
        string HintLabel { get; }
        int DefaultGameSpeed { get; }
        int DefaultAmmo { get; }
        int DefaultGems { get; }
        int DefaultHealth { get; }
        int DefaultEnergyCycles { get; }
        int DefaultTorches { get; }
        int DefaultTorchCycles { get; }
        int DefaultScore { get; }
        int DefaultTimePassed { get; }
        int DefaultStones { get; }
        string DefaultBoardTitle { get; }
        string DefaultWorldTitle { get; }
        AnsiChar DarknessTile { get; }
        AnsiChar EmptyTile { get; }
        int TorchRadius { get; }
        string RestartLabel { get; }
        AnsiChar FadeTile { get; }
        int PlayerCharacter { get; }
        int HealthLostPerHit { get; }
        int ShortMessageDuration { get; }
        int TorchDrawBoxVerticalSize { get; }
        int TorchDrawBoxHorizontalSize { get; }
        int LongMessageDuration { get; }
        string BombedLabel { get; }
        int MaxGameCycle { get; }
        AnsiChar ErrorFadeTile { get; }
        string EnterLabel { get; }
        int HighScoreNameLength { get; }
        int HighScoreNameCount { get; }
    }
}