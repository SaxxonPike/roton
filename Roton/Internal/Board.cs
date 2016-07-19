namespace Roton.Internal
{
    internal abstract class Board : IBoard
    {
        protected Board()
        {
            if (Camera == null)
            {
                Camera = new Location();
            }

            if (Enter == null)
            {
                Enter = new Location();
            }
        }

        public virtual IXyPair Camera { get; set; }
        public virtual bool Dark { get; set; }
        public virtual IXyPair Enter { get; set; }
        public virtual int ExitEast { get; set; }
        public virtual int ExitNorth { get; set; }
        public virtual int ExitSouth { get; set; }
        public virtual int ExitWest { get; set; }
        public virtual string Name { get; set; }
        public virtual bool RestartOnZap { get; set; }
        public virtual int Shots { get; set; }
        public virtual int TimeLimit { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}