﻿namespace Roton.Emulation.Data
{
    public interface IConfig
    {
        string DefaultWorld { get; }
        string HomePath { get; }
        int? RandomSeed { get; }
        int AudioSampleRate { get; }
        int AudioDrumRate { get; }
        int AudioBufferSize { get; }
        int VideoScale { get; }
        int MasterClockNumerator { get; }
        int MasterClockDenominator { get; }
        bool FastMode { get; }
    }
}