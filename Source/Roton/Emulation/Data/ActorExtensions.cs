namespace Roton.Emulation.Data;

public static class ActorExtensions
{
    extension(IActor self)
    {
        public void CopyFromByRaw(IActor actor)
        {
            // Use raw copy if both the source and destination have raw
            // data representation. Use the fallback if it is not available
            // for either actor.

            if (!self.Raw.IsEmpty && !actor.Raw.IsEmpty)
                actor.Raw.TryCopyTo(self.Raw);
            else
                self.CopyFrom(actor);
        }

        public void CopyFrom(IActor actor)
        {
            // Field-by-field actor copy.

            self.Cycle = actor.Cycle;
            self.Follower = actor.Follower;
            self.Instruction = actor.Instruction;
            self.Leader = actor.Leader;
            self.Length = actor.Length;
            self.Location = actor.Location;
            self.P1 = actor.P1;
            self.P2 = actor.P2;
            self.P3 = actor.P3;
            self.Pointer = actor.Pointer;
            self.UnderTile = actor.UnderTile;
            self.Vector = actor.Vector;
            self.Code = actor.Code;
            actor.Reserved.TryCopyTo(self.Reserved);
        }
    }
}