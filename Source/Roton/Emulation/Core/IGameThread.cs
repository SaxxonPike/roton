using System;
using System.Threading;

namespace Roton.Emulation.Core;

public interface IGameThread
{
    Thread? Current { get; }
    StepMode StepMode { get; set; }
    bool ThreadActive { get; }
    bool Start(Action startup);
    void Stop();
}