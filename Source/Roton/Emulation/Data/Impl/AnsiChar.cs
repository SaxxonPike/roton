using System;

namespace Roton.Emulation.Data.Impl;

public struct AnsiChar : IEquatable<AnsiChar>
{
    public readonly int Char;
    public readonly int Color;

    public AnsiChar(int newChar, int newColor)
    {
        Char = newChar;
        Color = newColor;
    }

    public static bool operator ==(AnsiChar a, AnsiChar b) => 
        a.Char == b.Char && a.Color == b.Color;
        
    public static bool operator !=(AnsiChar a, AnsiChar b) => 
        a.Char != b.Char || a.Color != b.Color;
        
    public bool Equals(AnsiChar other) => 
        Char == other.Char && Color == other.Color;

    public override bool Equals(object obj) => 
        obj is AnsiChar other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Char * 397) ^ Color;
        }
    }
}