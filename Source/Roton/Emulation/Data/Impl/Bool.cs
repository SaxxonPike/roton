using System;
using System.Runtime.InteropServices;

namespace Roton.Emulation.Data.Impl;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Bool(bool value) : IEquatable<Bool>, IEquatable<bool>
{
    public static implicit operator bool(Bool b) => b._val != 0;
    public static implicit operator Bool(bool value) => new(value);

    public static bool operator ==(Bool left, Bool right) => left.Equals(right);
    public static bool operator !=(Bool left, Bool right) => !left.Equals(right);
    private readonly byte _val = unchecked((byte)(value ? 1 : 0));

    public bool Equals(Bool other) =>
        _val != 0 == (other._val != 0);

    public bool Equals(bool other) =>
        _val != 0 == other;

    public override bool Equals(object? obj) =>
        (obj is Bool other && Equals(other)) ||
        (obj is bool b && Equals(b));

    public override int GetHashCode() =>
        (_val != 0).GetHashCode();

    public override string ToString() => $"{_val}";
}