using System;

namespace Roton.Emulation.Data;

public readonly struct AnsiChar(int newChar, int newColor) : IEquatable<AnsiChar>
{
    public readonly byte Char = unchecked((byte)newChar);
    public readonly byte Color = unchecked((byte)newColor);

    public static bool operator ==(AnsiChar a, AnsiChar b) =>
        a.Char == b.Char && a.Color == b.Color;

    public static bool operator !=(AnsiChar a, AnsiChar b) =>
        a.Char != b.Char || a.Color != b.Color;

    public bool Equals(AnsiChar other) =>
        Char == other.Char && Color == other.Color;

    public override bool Equals(object? obj) =>
        obj is AnsiChar other && Equals(other);

    public override int GetHashCode() =>
        Char | (Color << 8);

    public override string ToString() =>
        $"{{ Char: {Char:X2}, Color: {Color:X2} }}";
}