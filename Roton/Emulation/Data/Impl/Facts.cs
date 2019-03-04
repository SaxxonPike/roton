using Roton.Emulation.Core.Impl;

namespace Roton.Emulation.Data.Impl
{
    public abstract class Facts : IFacts
    {
        public abstract int AmmoPerPickup { get; }
        public abstract int HealthPerGem { get; }
        public int ScorePerGem => 10;
        public int PauseFlashInterval => 25;
        public int MainLoopRandomCycleRange => 100;
        public string DefaultSavedGameName => "SAVED";
        public string DefaultBoardName => "TEMP";
        public abstract string DefaultWorldName { get; }
        public string UntitledWorldName => "Untitled";
        public string HintLabel => "ALL:HINT";
        public string EnterLabel => "ALL:ENTER";
        public abstract int HighScoreNameLength { get; }
        public int HighScoreNameCount => 30;
        public int DefaultGameSpeed => 4;
        public int DefaultAmmo => 0;
        public int DefaultGems => 0;
        public int DefaultHealth => 100;
        public int DefaultEnergyCycles => 0;
        public int DefaultTorches => 0;
        public int DefaultTorchCycles => 0;
        public int DefaultScore => 0;
        public int DefaultTimePassed => 0;
        public int DefaultStones => -1;
        public string DefaultBoardTitle => "Introduction screen";
        public string DefaultWorldTitle => string.Empty;
        public AnsiChar DarknessTile => new AnsiChar(0xB0, 0x07);
        public AnsiChar EmptyTile => new AnsiChar(0x20, 0x0F);
        public int TorchRadius => 50;
        public string RestartLabel => "RESTART";
        public AnsiChar FadeTile => new AnsiChar(0xDB, 0x05);
        public int PlayerCharacter => 0x02;
        public int HealthLostPerHit => 10;
        public int ShortMessageDuration => 0x64;
        public int TorchDrawBoxVerticalSize => 11;
        public int TorchDrawBoxHorizontalSize => 8;
        public int LongMessageDuration => 0xC8;
        public string BombedLabel => "BOMBED";
        public int MaxGameCycle => 420;
        public AnsiChar ErrorFadeTile => new AnsiChar(0xDB, 0x04);
    }
}