namespace Roton
{
    public struct AnsiChar
    {
        public int Char;
        public int Color;

        public AnsiChar(int newChar, int newColor)
        {
            Char = newChar;
            Color = newColor;
        }
    }
}