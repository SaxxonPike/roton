using System;

namespace Roton.Emulation.Core;

public interface IClock : IDisposable
{
    event Action? OnTick;
    void Start();
    void Stop();
}