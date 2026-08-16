using System;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

public sealed class PackedBoard : IPackedBoard
{
    internal PackedBoard(byte[] data)
    {
        Data = new byte[data.Length];
        data.CopyTo(Data, 0);
    }

    public byte[] Data { get; set; }

    public string Name
    {
        get
        {
            if (Data.Length >= 260)
            {
                var nameLength = Data[0];
                if (nameLength == 0) return string.Empty;
                Span<char> chars = stackalloc char[nameLength];
                Cp437.BytesToChars(Data.AsSpan(1, nameLength), chars);
#if NET10_0_OR_GREATER
                return new string(chars);
#else
                return new string(chars.ToArray());
#endif
            }
            return string.Empty;
        }
        set
        {
            if (Data.Length >= 260)
            {
                var nameLength = (byte)((value?.Length ?? 0) & 0xFF);
                Data[0] = nameLength;
                if (nameLength > 0)
                {
                    value.ToBytes(Data.AsSpan(1, nameLength));
                }
            }
        }
    }

    public override string ToString()
    {
        return Name;
    }
}