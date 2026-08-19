using System;
using System.Runtime.InteropServices;

namespace Roton.Emulation.Data.Impl;

[StructLayout(LayoutKind.Sequential)]
public struct Tile : IEquatable<Tile>
{
    public static bool operator ==(Tile a, Tile b) => a.Equals(b);
    public static bool operator !=(Tile a, Tile b) => !a.Equals(b);

    private byte _id;
    private byte _color;
    
    public Tile(int id, int color)
    {
        Id = id;
        Color = color;
    }

    public int Color
    {
        get => _color;
        set => _color = unchecked((byte)(value & 0xFF));
    }

    public int Id
    {
        get => _id;
        set => _id = unchecked((byte)(value & 0xFF));
    }

    public override string ToString()
    {
        return $"Id: {Id:x2}, Color: {Color:x2}";
    }

    public bool Equals(Tile other) => 
        _id == other._id && _color == other._color;

    public override bool Equals(object? obj) => 
        obj is Tile other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (_id.GetHashCode() * 397) ^ _color.GetHashCode();
        }
    }
}