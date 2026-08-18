using System;
using System.Runtime.InteropServices;

namespace Roton.Emulation.Data.Impl;

/// <summary>
/// Wraps a signed 16-bit value in an endian-agnostic manner.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct Word(int value) : IEquatable<Word>, IEquatable<int>, IEquatable<short>
{
    public static implicit operator short(Word word) => unchecked((short)word.Value);
    public static implicit operator int(Word word) => word.Value;
    public static implicit operator Word(int value) => new(value);

    public static bool operator ==(Word left, Word right) => left.Equals(right);
    public static bool operator !=(Word left, Word right) => !left.Equals(right);

    private readonly short _val = BitConverter.IsLittleEndian
        ? unchecked((short)value)
        : unchecked((short)(((value >> 8) & 0xFF) | ((value & 0xFF) << 8)));

    private int Value
    {
        get
        {
            if (BitConverter.IsLittleEndian)
                return _val;

            return unchecked((short)(((_val & 0xFF) << 8) | ((_val >> 8) & 0xFF)));
        }
    }

    public bool Equals(Word other) =>
        _val == other._val;

    public bool Equals(int other) =>
        Value == other;

    public bool Equals(short other) =>
        Value == other;

    public override bool Equals(object obj) =>
        obj is Word other && Equals(other);

    public override int GetHashCode() =>
        BitConverter.IsLittleEndian
            ? _val.GetHashCode()
            : unchecked((short)(((_val & 0xFF) << 8) | ((_val >> 8) & 0xFF))).GetHashCode();
    
    public override string ToString() => $"{Value}";
}