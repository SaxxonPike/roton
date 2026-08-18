using System;
using System.Runtime.InteropServices;

namespace Roton.Emulation.Data.Impl;

/// <summary>
/// Wraps a 16-bit value in an endian-agnostic manner.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Word : IEquatable<Word>, IEquatable<int>, IEquatable<short>
{
    public static implicit operator short(Word word) => unchecked((short)word.Value);
    public static implicit operator int(Word word) => word.Value;
    public static implicit operator Word(int value) => new Word { Value = value };

    public static bool operator ==(Word left, Word right) => left.Equals(right);
    public static bool operator !=(Word left, Word right) => !left.Equals(right);

    private short _val;

    private int Value
    {
        get
        {
            if (BitConverter.IsLittleEndian)
                return _val;

            return unchecked((short)(((_val & 0xFF) << 8) | ((_val >> 8) & 0xFF)));
        }
        set
        {
            if (BitConverter.IsLittleEndian)
            {
                _val = unchecked((short)value);
                return;
            }

            _val = unchecked((short)(((value >> 8) & 0xFF) | ((value & 0xFF) << 8)));
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
}