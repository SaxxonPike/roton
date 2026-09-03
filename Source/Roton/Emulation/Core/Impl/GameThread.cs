using System;
using System.Threading;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class GameThread : IGameThread
{
    public Thread? Current { get; private set; }
    public bool Step { get; set; }

    public bool ThreadActive => Current != null || Step;

    public bool Start(Action startup)
    {
        if (Current != null)
            return false;

        Current = new Thread(new ThreadStart(startup));
        Current.Start();
        return true;
    }

    public void Stop() =>
        Current = null;

    public void SetCurrent(Thread? thread) =>
        Current = thread;
}