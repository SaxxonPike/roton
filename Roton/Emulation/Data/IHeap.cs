﻿namespace Roton.Emulation.Data
{
    public interface IHeap
    {
        byte[] this[int index] { get; }
        int Allocate(byte[] data);
        void FreeAll();
    }
}