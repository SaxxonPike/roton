using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    sealed public class PackedBoard
    {
        public PackedBoard()
        {
            Data = new byte[0];
        }

        public PackedBoard(byte[] data)
        {
            Data = new byte[data.Length];
            data.CopyTo(Data, 0);
        }

        public byte[] Data
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                if (Data.Length >= 260)
                {
                    int nameLength = Data[0];
                    byte[] nameData = new byte[nameLength];
                    Array.Copy(Data, 1, nameData, 0, nameLength);
                    return nameData.ToStringValue();
                }
                return "";
            }
            set
            {
                if (Data.Length >= 260)
                {
                    byte[] nameData = value.ToBytes();
                    byte nameLength = (byte)(nameData.Length & 0xFF);
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
