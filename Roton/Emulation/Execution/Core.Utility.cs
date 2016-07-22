using System.Threading;
using Roton.Core;
using Roton.Emulation.Timing;
using Roton.Extensions;

namespace Roton.Emulation.Execution
{
    internal partial class Core
    {
        protected virtual int Adjacent(IXyPair location, int id)
        {
            return (location.Y <= 1 || Tiles[location.Sum(Vector.North)].Id == id ? 1 : 0) |
                   (location.Y >= Tiles.Height || Tiles[location.Sum(Vector.South)].Id == id ? 2 : 0) |
                   (location.X <= 1 || Tiles[location.Sum(Vector.West)].Id == id ? 4 : 0) |
                   (location.X >= Tiles.Width || Tiles[location.Sum(Vector.East)].Id == id ? 8 : 0);
        }

        protected virtual int Distance(IXyPair a, IXyPair b)
        {
            return (a.Y - b.Y).Square()*2 + (a.X - b.X).Square();
        }

        public virtual void Start()
        {
            if (!ThreadActive)
            {
                ThreadActive = true;
                Thread = new Thread(StartMain);
                TimerTick = CoreTimer.Tick;
                Thread.Start();
            }
        }

        public virtual void Stop()
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