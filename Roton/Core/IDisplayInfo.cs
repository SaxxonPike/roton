namespace Roton.Core
{
    public interface IDisplayInfo
    {
        int Ammo { get; }
        IXyPair Camera { get; }
        IElementList Elements { get; }
        int EnergyCycles { get; }
        int GameSpeed { get; }
        int Gems { get; }
        int Height { get; }
        int KeyPressed { get; }
        IKeyList Keys { get; }
        bool KeyShift { get; }
        string Message { get; }
        string Message2 { get; }
        IActor Player { get; }
        bool Quiet { get; }
        int Score { get; }
        int Stones { get; }
        string StoneText { get; }
        ITerminal Terminal { get; }
        int TimeLimit { get; }
        int TimePassed { get; }
        bool TitleScreen { get; }
        int TorchCycles { get; }
        int Torches { get; }
        int Width { get; }
        string WorldName { get; }
        int Health { get; set; }
        AnsiChar Draw(IXyPair location);
        void ReadInput();
        int ReadKey();
        void WaitForTick();
    }
}