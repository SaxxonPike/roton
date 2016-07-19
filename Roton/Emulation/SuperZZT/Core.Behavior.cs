namespace Roton.Emulation.SuperZZT
{
    internal sealed partial class Core
    {
        public override void Act_Monitor(int index)
        {
            base.Act_Monitor(index);
            MoveActorOnRiver(index);
        }

        public override void Act_Player(int index)
        {
            base.Act_Player(index);
            MoveActorOnRiver(index);
        }

        internal override void ForcePlayerColor(int index)
        {
            // Do nothing to override the player's color.
        }
    }
}