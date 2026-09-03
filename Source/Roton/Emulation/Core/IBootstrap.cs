using System;

namespace Roton.Emulation.Core;

public interface IBootstrap
{
    event EventHandler? Exited;

    void Start();
    void Stop();
}