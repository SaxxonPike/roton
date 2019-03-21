﻿namespace Roton.Emulation.Data
{
    public interface IMemory
    {
        byte[] Dump();
        byte[] Read(int offset, int length);
        int Read8(int offset);
        void Write(int offset, byte[] data);
        void Write8(int offset, int value);
    }
}