using System;
using System.Runtime.InteropServices;

namespace Roton.Emulation.Data.Impl;

/// <summary>
/// Wraps an unsigned 8-bit value.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct HWord(int value) : IEquatable<HWord>, IEquatable<int>, IEquatable<byte>
{
    public static implicit operator byte(HWord word) => word._val;
    public static implicit operator int(HWord word) => word._val;
    public static implicit operator HWord(int value) => new(value);

    public static bool operator ==(HWord left, HWord right) => left.Equals(right);
    public static bool operator !=(HWord left, HWord right) => !left.Equals(right);

    private readonly byte _val = unchecked((byte)value);

    public bool Equals(HWord other) =>
        _val == other._val;

    public bool Equals(int other) =>
        _val == other;

    public bool Equals(byte other) =>
        _val == other;

    public override bool Equals(object obj) =>
        obj is HWord other && Equals(other);

    public override int GetHashCode() =>
        _val.GetHashCode();

    public override string ToString() => $"{_val}";
}