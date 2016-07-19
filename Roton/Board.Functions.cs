namespace Roton
{
    public abstract partial class Board
    {
        public Board()
        {
            Initialize();

            if (Camera == null)
            {
                Camera = new Location();
            }

            if (Enter == null)
            {
                Enter = new Location();
            }
        }

        protected virtual void Initialize()
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}