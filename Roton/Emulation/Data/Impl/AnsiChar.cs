namespace Roton.Emulation.Data.Impl
{
    public struct AnsiChar
    {
        public readonly int Char;
        public readonly int Color;

        public AnsiChar(int newChar, int newColor)
        {
            Char = newChar;
            Color = newColor;
        }
    }
}