using System;
using Roton.Core;

namespace Roton.Emulation.Data.Impl
{
    public sealed class PackedBoard : IPackedBoard
    {
        public PackedBoard(byte[] data)
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
                    int nameLength = Data[0];
                    var nameData = new byte[nameLength];
                    Array.Copy(Data, 1, nameData, 0, nameLength);
                    return nameData.ToStringValue();
                }
                return string.Empty;
            }
            set
            {
                if (Data.Length >= 260)
                {
                    var nameData = value.ToBytes();
                    var nameLength = (byte) (nameData.Length & 0xFF);
                    Data[0] = nameLength;
                    Array.Copy(nameData, 0, Data, 1, nameLength);
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}