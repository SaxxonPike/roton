using System;

namespace Roton.Emulation.Core
{
    public interface IClock : IDisposable
    {
        event EventHandler OnTick;
        void Start();
        void Stop();
    }
}