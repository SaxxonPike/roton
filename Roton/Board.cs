namespace Roton
{
    public abstract partial class Board
    {
        public virtual Location Camera { get; set; }
        public virtual bool Dark { get; set; }
        public virtual Location Enter { get; set; }
        public virtual int ExitEast { get; set; }
        public virtual int ExitNorth { get; set; }
        public virtual int ExitSouth { get; set; }
        public virtual int ExitWest { get; set; }
        public virtual string Name { get; set; }
        public virtual bool RestartOnZap { get; set; }
        public virtual int Shots { get; set; }
        public virtual int TimeLimit { get; set; }
    }
}