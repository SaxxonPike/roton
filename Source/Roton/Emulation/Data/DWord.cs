using System;
using System.Runtime.InteropServices;

namespace Roton.Emulation.Data;

[StructLayout(LayoutKind.Sequential)]
public struct DWord : IEquatable<DWord>, IEquatable<int>
{
    public static implicit operator int(DWord word) => word.Value;
    public static implicit operator DWord(int value) => new() { Value = value };

    public static bool operator ==(DWord left, DWord right) => left.Equals(right);
    public static bool operator !=(DWord left, DWord right) => !left.Equals(right);

    private int _val;

    private int Value
    {
        get
        {
            if (BitConverter.IsLittleEndian)
                return _val;

            return ((_val & 0xFF) << 24) |
                   ((_val & 0xFF00) << 8) |
                   ((_val & 0xFF0000) >> 8) |
                   ((_val >> 24) & 0xFF);
        }
        set
        {
            if (BitConverter.IsLittleEndian)
            {
                _val = value;
                return;
            }

            _val = ((value & 0xFF) << 24) |
                   ((value & 0xFF00) << 8) |
                   ((value & 0xFF0000) >> 8) |
                   ((value >> 24) & 0xFF);
        }
    }

    public bool Equals(DWord other) =>
        _val == other._val;

    public bool Equals(int other) =>
        Value == other;

    public override bool Equals(object? obj) =>
        obj is DWord other && Equals(other);

    public override int GetHashCode() =>
        BitConverter.IsLittleEndian
            ? _val.GetHashCode()
            : ((_val & 0xFF) << 24) |
              ((_val & 0xFF00) << 8) |
              ((_val & 0xFF0000) >> 8) |
              ((_val >> 24) & 0xFF).GetHashCode();

    public override string ToString() => $"{_val}";
}