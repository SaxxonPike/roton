using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Roton.Emulation
{
    internal partial class CoreBase
    {
        virtual internal int Adjacent(Location location, int id)
        {
            return ((location.Y <= 1 || Tiles[location.Sum(Vector.North)].Id == id) ? 1 : 0) |
                ((location.Y >= Tiles.Height || Tiles[location.Sum(Vector.South)].Id == id) ? 2 : 0) |
                ((location.X <= 1 || Tiles[location.Sum(Vector.West)].Id == id) ? 4 : 0) |
                ((location.X >= Tiles.Width || Tiles[location.Sum(Vector.East)].Id == id) ? 8 : 0);
        }

        virtual internal int Distance(Location a, Location b)
        {
            return ((a.Y - b.Y).Square() * 2) + ((a.X - b.X).Square());
        }

        virtual internal void Start()
        {
            if (!ThreadActive)
            {
                ThreadActive = true;
                Thread = new Thread(new ThreadStart(StartMain));
                TimerTick = CoreTimer.Tick;
                Thread.Start();
            }
        }

        virtual internal void Stop()
        {
            if (ThreadActive)
            {
                ThreadActive = false;
            }
        }

        private Thread Thread
        {
            get;
            set;
        }

        private bool ThreadActive
        {
            get;
            set;
        }
    }
}
