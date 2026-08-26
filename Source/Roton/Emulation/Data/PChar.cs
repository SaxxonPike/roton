using System;
using System.Runtime.InteropServices;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data;

/// <summary>
/// Wraps an unsigned 8-bit value.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct PChar(byte value) : IEquatable<PChar>, IEquatable<int>, IEquatable<byte>
{
    public PChar(char value) : this(Cp437.CharToByte(value))
    {
    }

    public static implicit operator byte(PChar ch) => ch._val;
    public static implicit operator int(PChar ch) => ch._val;
    public static implicit operator char(PChar ch) => Cp437.ByteToChar(ch._val);
    public static implicit operator PChar(char value) => new(value);
    public static implicit operator PChar(byte value) => new(value);

    public static bool operator ==(PChar left, PChar right) => left.Equals(right);
    public static bool operator !=(PChar left, PChar right) => !left.Equals(right);

    private readonly byte _val = value;

    public bool Equals(PChar other) =>
        _val == other._val;

    public bool Equals(int other) =>
        _val == other;

    public bool Equals(byte other) =>
        _val == other;

    public override bool Equals(object? obj) =>
        obj is PChar other && Equals(other);

    public override int GetHashCode() =>
        _val.GetHashCode();

    public override string ToString() => $"{_val}";

    public PChar ToUpper() =>
        _val switch
        {
            >= 0x61 and <= 0x7A => new PChar(unchecked((byte)(_val - 0x20))),
            _ => this
        };
}