﻿using System.Threading;

namespace Roton.Emulation
{
    internal partial class CoreBase
    {
        internal virtual int Adjacent(Location location, int id)
        {
            return (location.Y <= 1 || Tiles[location.Sum(Vector.North)].Id == id ? 1 : 0) |
                   (location.Y >= Tiles.Height || Tiles[location.Sum(Vector.South)].Id == id ? 2 : 0) |
                   (location.X <= 1 || Tiles[location.Sum(Vector.West)].Id == id ? 4 : 0) |
                   (location.X >= Tiles.Width || Tiles[location.Sum(Vector.East)].Id == id ? 8 : 0);
        }

        internal virtual int Distance(Location a, Location b)
        {
            return (a.Y - b.Y).Square()*2 + (a.X - b.X).Square();
        }

        internal virtual void Start()
        {
            if (!ThreadActive)
            {
                ThreadActive = true;
                Thread = new Thread(StartMain);
                TimerTick = CoreTimer.Tick;
                Thread.Start();
            }
        }

        internal virtual void Stop()
        {
            if (ThreadActive)
            {
                ThreadActive = false;
            }
        }

        private Thread Thread { get; set; }

        private bool ThreadActive { get; set; }
    }
}